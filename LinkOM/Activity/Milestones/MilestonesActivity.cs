

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
	[Activity (Label = "Milestone", Theme = "@style/Theme.Customtheme")]			
	public class MilestonesActivity : Activity, TextView.IOnEditorActionListener
	{
		public List<MilestoneObject> _MilestoneList;
		public MilestoneListAdapter milestoneList;

		public int MilestoneId;
		public SwipeRefreshLayout refresher;
		public bool loading;


		public EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;
		public ListView milestoneListView;

		public InputMethodManager inputManager;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.Milestones);
			// Create your application here

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.milestone_title);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			milestoneListView = FindViewById<ListView> (Resource.Id.MilestoneListView);

			mSearch = FindViewById<EditText>(Resource.Id.etSearch);
			mSearch.Alpha = 0;
			mSearch.SetOnEditorActionListener (this);
			mSearch.Focusable = false;
			mSearch.FocusableInTouchMode = false;
			mSearch.TextChanged += InputSearchOnTextChanged;

			InitData ();


			inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);

		}

		//Refesh data
		async void HandleRefresh (object sender, EventArgs e)
		{
			await InitData ();
			refresher.Refreshing = false;
		}

		//Loading data
		public async Task InitData(){

			if (loading)
				return;
			loading = true;

			string url = Settings.InstanceURL;

			url=url+"/api/MilestoneList";


			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "T.Title", Direction = "1"},
				new objSort{ColumnName = "T.ProjectName", Direction = "2"}
			};

			var objMilestone = new
			{
				ProjectId = string.Empty,
				StatusId = string.Empty,
				DepartmentId = string.Empty,
				Title = string.Empty,
				PriorityId= string.Empty,
				Label= string.Empty,
				DueBefore= string.Empty,
				AssignTo= string.Empty,
				AssignByMe= string.Empty,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						PageSize = 100,
						PageNumber = 1,
						Sort = objSort,
						Item = objMilestone
					}
				});

			string results=  ConnectWebAPI.Request(url,objsearch);

			if (results != null) {

				MilestoneListJson MilestoneList = Newtonsoft.Json.JsonConvert.DeserializeObject<MilestoneListJson> (results);

				milestoneList = new MilestoneListAdapter (this, MilestoneList.Items);

				milestoneListView.Adapter = milestoneList;

//			milestoneListView.ItemClick += listView_ItemClick;

				loading = false;
			}

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
//			case Resource.Id.add:
//				Intent Intent = new Intent (this, typeof(MilestoneAddActivity));
//				Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
//				StartActivity(Intent);

				break;
			default:
				break;
			}

			return true;
		}

//		//handle list item clicked
//		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
//		{
//			//Get our item from the list adapter
//			MilestoneObject Milestone = this.milestoneList.GetItemAtPosition(e.Position);
//
//			Intent addAccountIntent = new Intent (this, typeof(MilestoneDetailActivity));
//			//			addAccountIntent.SetFlags (ActivityFlags.ClearWhenTaskReset);
//
//			addAccountIntent.PutExtra ("Milestone", Newtonsoft.Json.JsonConvert.SerializeObject(Milestone));
//
//			StartActivity(addAccountIntent);
//
//		}

		//Init menu on action bar
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			inflater.Inflate (Resource.Menu.AddSearchMenu, menu);

			return true;
		}

		public void btSearchClick()
		{
			if (!mAnimatedDown)
			{
				mSearch.Focusable = true;
				mSearch.FocusableInTouchMode= true;
				mSearch.RequestFocus ();
				MyAnimation anim = new MyAnimation(milestoneListView, milestoneListView.Height - mSearch.Height);
				anim.Duration = 500;
				milestoneListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartDown;
				anim.AnimationEnd += anim_AnimationEndDown;
				milestoneListView.Animate().TranslationYBy(mSearch.Height).SetDuration(500).Start();


				inputManager.ShowSoftInput(mSearch, ShowFlags.Implicit);

			}

			else
			{
				mSearch.Focusable = false;
				mSearch.FocusableInTouchMode= false;

				MyAnimation anim = new MyAnimation(milestoneListView, milestoneListView.Height + mSearch.Height);
				anim.Duration = 500;
				milestoneListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartUp;
				anim.AnimationEnd += anim_AnimationEndUp;
				milestoneListView.Animate().TranslationYBy(-mSearch.Height).SetDuration(500).Start();

				inputManager.HideSoftInputFromWindow(this.mSearch.WindowToken, 0);


			}

			mAnimatedDown = !mAnimatedDown;
		}

		private void InputSearchOnTextChanged(object sender, TextChangedEventArgs args)
		{
			milestoneList.Filter.InvokeFilter(mSearch.Text);
		}

		void anim_AnimationEndUp(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
		{
			mIsAnimating = false;
			mSearch.ClearFocus();
		}

		void anim_AnimationEndDown(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
		{
			mIsAnimating = false;
		}

		void anim_AnimationStartDown(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
		{
			mIsAnimating = true;
			mSearch.Animate().AlphaBy(1.0f).SetDuration(1000).Start();
		}

		void anim_AnimationStartUp(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
		{
			mIsAnimating = true;
			mSearch.Animate().AlphaBy(-1.0f).SetDuration(1000).Start();
		}

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Search) {
				if (!string.IsNullOrEmpty (mSearch.Text)) {
					milestoneList.Filter.InvokeFilter(mSearch.Text);
				} 
				return true;
				//next action will set focus to password edit text.
			} 
			return false;
		}
	}
}

