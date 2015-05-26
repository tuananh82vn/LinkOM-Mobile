
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

namespace LinkOM
{
	[Activity (Label = "IssuesList", Theme = "@style/Theme.Customtheme")]			
	public class IssuesListActivity : Activity, TextView.IOnEditorActionListener
	{
		public List<IssuesList> _IssuesList;
		public IssuesListAdapter issuesList;
		public int StatusId;
		public SwipeRefreshLayout refresher;
		public bool loading;

		private EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;
		public ListView issuesListView ;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.IssuesListContainer);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.issues_title_list);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);



			issuesListView = FindViewById<ListView> (Resource.Id.IssuesListView);

			mSearch = FindViewById<EditText>(Resource.Id.etSearch);
			mSearch.Alpha = 0;
			mSearch.SetOnEditorActionListener (this);
			mSearch.TextChanged += InputSearchOnTextChanged;



			StatusId= Intent.GetIntExtra ("IssuesStatusId",0);

			InitData ();

			refresher = FindViewById<SwipeRefreshLayout> (Resource.Id.refresher);

			refresher.SetColorScheme (Resource.Color.golden,Resource.Color.ginger_brown,Resource.Color.french_blue,Resource.Color.fern_green);

			refresher.Refresh += HandleRefresh;

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

			var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
			fab.AttachToListView(issuesListView);
			fab.Click += Fab_Click;
		}

		void Fab_Click (object sender, EventArgs e)
		{
			Intent Intent = new Intent (this, typeof(IssuesAddActivity));
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
			issuesList.Filter.InvokeFilter(mSearch.Text);
		}

		async void HandleRefresh (object sender, EventArgs e)
		{
			await InitData ();
			refresher.Refreshing = false;
		}

		private async Task InitData(){

			if (StatusId != 0) {

				if (loading)
					return;
				loading = true;

				IssuesFilter objFilter = new IssuesFilter ();
				objFilter.IssueStatusId = StatusId;

				issuesList = new IssuesListAdapter (this, IssuesHelper.GetIssuesList(objFilter));

				issuesListView.Adapter = issuesList;

				issuesListView.ItemClick += listView_ItemClick;


				loading = false;

			}
		}

		public override void OnBackPressed()
		{
			this.Finish ();
			base.OnBackPressed();
		}

		public void btSearchClick()
		{
			if (!mAnimatedDown)
			{
				//Listview is up
				MyAnimation anim = new MyAnimation(issuesListView, issuesListView.Height - mSearch.Height);
				anim.Duration = 500;
				issuesListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartDown;
				anim.AnimationEnd += anim_AnimationEndDown;
				refresher.Animate().TranslationYBy(mSearch.Height).SetDuration(500).Start();
			}

			else
			{
				//Listview is down
				MyAnimation anim = new MyAnimation(issuesListView, issuesListView.Height + mSearch.Height);
				anim.Duration = 500;
				issuesListView.StartAnimation(anim);
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

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Search) {
				if (!string.IsNullOrEmpty (mSearch.Text)) {
					issuesList.Filter.InvokeFilter(mSearch.Text);
				} 
				return true;
				//next action will set focus to password edit text.
			} 
			return false;
		}

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{

			IssuesList model = this.issuesList.GetItemAtPosition (e.Position);

			var activity = new Intent (this, typeof(IssuesDetailActivity));

			activity.PutExtra ("Issue", Newtonsoft.Json.JsonConvert.SerializeObject(model));

			StartActivity (activity);

		}
	}
}

