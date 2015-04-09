
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using System.Threading.Tasks;
using Android.Views.InputMethods;
using Android.Text;

namespace LinkOM
{
	[Activity (Label = "TicketList")]			
	public class TicketListActivity : Activity, TextView.IOnEditorActionListener
	{
		public List<TicketObject> _TicketList;
		public TicketListAdapter ticketList;
		public int StatusId;
		public SwipeRefreshLayout refresher;
		public bool loading;

		private EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;
		public ListView ticketListView ;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.TicketListContainer);

			ticketListView = FindViewById<ListView> (Resource.Id.TicketListView);

			mSearch = FindViewById<EditText>(Resource.Id.etSearch);
			mSearch.Alpha = 0;
			mSearch.SetOnEditorActionListener (this);
			mSearch.TextChanged += InputSearchOnTextChanged;



			var buttonBack = FindViewById(Resource.Id.BackButton);
			buttonBack.Click += btBackClick;

			var SearchButton = FindViewById(Resource.Id.SearchButton);
			SearchButton.Click += btSearchClick;

			StatusId= Intent.GetIntExtra ("StatusId",0);

			InitData ();

			refresher = FindViewById<SwipeRefreshLayout> (Resource.Id.refresher);

			refresher.SetColorScheme (Resource.Color.xam_green,Resource.Color.xam_purple,Resource.Color.xam_gray,Resource.Color.xam_dark_blue);

			refresher.Refresh += HandleRefresh;
		}

		private void InputSearchOnTextChanged(object sender, TextChangedEventArgs args)
		{
			ticketList.Filter.InvokeFilter(mSearch.Text);
		}

		async void HandleRefresh (object sender, EventArgs e)
		{
			await InitData ();
			refresher.Refreshing = false;
		}

		private async Task InitData(){

			if (StatusId != 0) {

				Console.WriteLine ("Begin load data");

				if (loading)
					return;
				loading = true;

				string url = Settings.InstanceURL;

				url=url+"/api/TicketList";

				var objTicket = new
				{
					ProjectId = string.Empty,
					TicketStatusId = StatusId,
					DepartmentId = string.Empty,
					Title = string.Empty,
					PriorityId = string.Empty,
					Label= string.Empty,
					DueBefore = string.Empty,
					AssignTo = string.Empty,
					AssignByMe = string.Empty,
				};

				var objsearch = (new
					{
						objApiSearch = new
						{
							UserId = Settings.UserId,
							TokenNumber =Settings.Token,
							PageSize = 100,
							PageNumber = 1,
							SortMember ="",
							SortDirection = "",
							MainStatusId=1,
							Item = objTicket
						}
					});
				
				string results = ConnectWebAPI.Request (url, objsearch);

				if (results != null && results != "") {

					TicketList obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TicketList> (results);

					if (obj.Items != null) {

						ticketList = new TicketListAdapter (this, obj.Items);

						ticketListView.Adapter = ticketList;

						//ticketListView.ItemClick += listView_ItemClick;
					} 
				}

				await Task.Delay (2000);

				loading = false;

				Console.WriteLine ("End load data");
			}
		}

		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}

		public void btSearchClick(object sender, EventArgs e)
		{
			if (!mAnimatedDown)
			{
				//Listview is up
				MyAnimation anim = new MyAnimation(ticketListView, ticketListView.Height - mSearch.Height);
				anim.Duration = 500;
				ticketListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartDown;
				anim.AnimationEnd += anim_AnimationEndDown;
				refresher.Animate().TranslationYBy(mSearch.Height).SetDuration(500).Start();
			}

			else
			{
				//Listview is down
				MyAnimation anim = new MyAnimation(ticketListView, ticketListView.Height + mSearch.Height);
				anim.Duration = 500;
				ticketListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartUp;
				anim.AnimationEnd += anim_AnimationEndUp;
				refresher.Animate().TranslationYBy(-mSearch.Height).SetDuration(500).Start();
			}

			mAnimatedDown = !mAnimatedDown;
		}

		void anim_AnimationEndUp(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
		{
			mIsAnimating = false;
			mSearch.ClearFocus();
			InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
			inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
		}

		void anim_AnimationEndDown(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
		{
			mIsAnimating = false;
		}

		void anim_AnimationStartDown(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
		{
			mIsAnimating = true;
			mSearch.Animate().AlphaBy(1.0f).SetDuration(500).Start();
		}

		void anim_AnimationStartUp(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
		{
			mIsAnimating = true;
			mSearch.Animate().AlphaBy(-1.0f).SetDuration(300).Start();
		}

//		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
//		{
//
//			TaskObject model = this.ticketList.GetItemAtPosition (e.Position);
//
//			var activity = new Intent (this, typeof(EditTaskActivity));
//
//			activity.PutExtra ("Task", Newtonsoft.Json.JsonConvert.SerializeObject(model));
//
//			StartActivity (activity);
//
//			this.Finish ();
//
//		}

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Search) {
				if (!string.IsNullOrEmpty (mSearch.Text)) {
					ticketList.Filter.InvokeFilter(mSearch.Text);
				} 
				return true;
				//next action will set focus to password edit text.
			} 
			return false;
		}
	}
}

