
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
using Android.Content.PM;
using com.refractored.fab;
using Android.Graphics;

namespace LinkOM
{
	[Activity (Label = "TicketList", Theme = "@style/Theme.Customtheme")]				
	public class TicketListActivity : Activity, TextView.IOnEditorActionListener
	{
		public List<TicketList> _TicketList;
		public TicketListAdapter ticketList;
		public int StatusId;
//		public SwipeRefreshLayout refresher;
		public bool loading;

		private EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;
		public ListView ticketListView ;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.TicketListContainer);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.ticket_title);

			var upArrow = Resources.GetDrawable(Resource.Drawable.abc_ic_ab_back_mtrl_am_alpha);
			upArrow.SetColorFilter(Resources.GetColor(Resource.Color.white), PorterDuff.Mode.SrcIn);
			ActionBar.SetHomeAsUpIndicator (upArrow);


			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);


			ticketListView = FindViewById<ListView> (Resource.Id.TicketListView);

			mSearch = FindViewById<EditText>(Resource.Id.etSearch);
			mSearch.Alpha = 0;
			mSearch.SetOnEditorActionListener (this);
			mSearch.TextChanged += InputSearchOnTextChanged;

			StatusId= Intent.GetIntExtra ("TicketStatusId",0);

			InitData ();

//			refresher = FindViewById<SwipeRefreshLayout> (Resource.Id.refresher);
//
//			refresher.SetColorScheme (Resource.Color.golden,Resource.Color.ginger_brown,Resource.Color.french_blue,Resource.Color.fern_green);
//
//			refresher.Refresh += HandleRefresh;

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) 
			{
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} 
			else 
			{
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

			var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
			fab.AttachToListView(ticketListView);
			fab.Click += Fab_Click;
		}

		void Fab_Click (object sender, EventArgs e)
		{
			Intent Intent = new Intent (this, typeof(TicketAddActivity));
			Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
			StartActivity(Intent);
		}

		//Handle item on action bar clicked
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			base.OnOptionsItemSelected (item);

			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				OnBackPressed ();
				break;
			case Resource.Id.search:
				btSearchClick ();
				break;
			default:
				break;
			}

			return true;
		}

		//Init menu on action bar
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			inflater.Inflate (Resource.Menu.SearchMenu, menu);

			return true;
		}


		private void InputSearchOnTextChanged(object sender, TextChangedEventArgs args)
		{
			ticketList.Filter.InvokeFilter(mSearch.Text);
		}

//		async void HandleRefresh (object sender, EventArgs e)
//		{
//			await InitData ();
//			refresher.Refreshing = false;
//		}

		private void InitData(){

			if (StatusId != 0) {

				if (loading)
					return;
				loading = true;

				TicketFilter objFilter = new TicketFilter ();
				objFilter.AssignedToId = Settings.UserId;
				objFilter.TicketStatusId = StatusId;

				ticketList = new TicketListAdapter (this, TicketHelper.GetTicketList(objFilter));

				ticketListView.Adapter = ticketList;

				ticketListView.ItemClick += listView_ItemClick;


				loading = false;

			}
		}


		public void btSearchClick()
		{
			if (!mAnimatedDown)
			{
				//Listview is up
				MyAnimation anim = new MyAnimation(ticketListView, ticketListView.Height - mSearch.Height);
				anim.Duration = 500;
				ticketListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartDown;
				anim.AnimationEnd += anim_AnimationEndDown;
				ticketListView.Animate().TranslationYBy(mSearch.Height).SetDuration(500).Start();
			}

			else
			{
				//Listview is down
				MyAnimation anim = new MyAnimation(ticketListView, ticketListView.Height + mSearch.Height);
				anim.Duration = 500;
				ticketListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartUp;
				anim.AnimationEnd += anim_AnimationEndUp;
				ticketListView.Animate().TranslationYBy(-mSearch.Height).SetDuration(500).Start();
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

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{

			TicketList model = this.ticketList.GetItemAtPosition (e.Position);

			var activity = new Intent (this, typeof(TicketDetailActivity));

			activity.PutExtra ("Ticket", Newtonsoft.Json.JsonConvert.SerializeObject(model));

			StartActivity (activity);

		}

		public override void OnBackPressed()
		{
			this.Finish ();
			base.OnBackPressed();
		}

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

