

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
	[Activity (Label = "Milestone", Theme = "@style/Theme.Customtheme")]			
	public class MilestoneActivity : Activity, TextView.IOnEditorActionListener
	{
		public List<MilestonesList> _MilestoneList;
		public MilestoneListAdapter milestoneList;

		public int MilestoneId;
		public bool loading;


		public EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;
		public ListView milestoneListView;

		public InputMethodManager inputManager;
		public FrameLayout frameLayout1;
		public MilestonesDetailList MilestoneSelected;

		public string results;
		public FloatingActionButton fab;
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

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} 
			else 
			{

				frameLayout1  = FindViewById<FrameLayout> (Resource.Id.frameLayout1);

				LoadMilestone ();

				if (MilestoneSelected != null) {
					DisplayMilestone (MilestoneSelected);
					frameLayout1.Visibility = ViewStates.Visible;
				}
				else
					frameLayout1.Visibility = ViewStates.Invisible;
				
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

			var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
			fab.AttachToListView(milestoneListView);
			fab.Click += Fab_Click;

		}

		void Fab_Click (object sender, EventArgs e)
		{
//			Intent Intent2 = new Intent (this, typeof(MilestoneAddActivity));
//			Intent2.SetFlags (ActivityFlags.ClearWhenTaskReset);
//			StartActivity(Intent2);
			Toast.MakeText (this, "Coming soon.", ToastLength.Short).Show ();
		}

		public void LoadMilestone(){

			results= Intent.GetStringExtra ("Milestone");

			if (results != null) {
				var temp  = Newtonsoft.Json.JsonConvert.DeserializeObject<MilestonesList> (results);

				MilestoneSelected = MilestonesHelper.GetMilestonesDetail (temp.Id.Value);
			}

		}

		//Loading data
		public void InitData(){

			var objMilestone = new MilestoneFilter ();

			milestoneList = new MilestoneListAdapter (this, MilestonesHelper.GetMilestonesList(objMilestone));

			milestoneListView.Adapter = milestoneList;

			milestoneListView.ItemClick += listView_ItemClick;
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
			case Resource.Id.edit:
				if (MilestoneSelected != null) {
					Intent Intent = new Intent (this, typeof(MilestoneEditActivity));
					Intent.PutExtra ("Milestone", Newtonsoft.Json.JsonConvert.SerializeObject (MilestoneSelected));
					Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity (Intent);
				}
				else
					Toast.MakeText (this, "No Issues Selected.", ToastLength.Short).Show ();
				break;
			case Resource.Id.search:
				btSearchClick ();
				break;
			default:
				break;
			}

			return true;
		}

		//handle list item clicked
		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var temp = this.milestoneList.GetItemAtPosition (e.Position);

			if (Settings.Orientation.Equals ("Portrait")) {
				//Get our item from the list adapter

				Intent addAccountIntent = new Intent (this, typeof(MilestoneDetailActivity));

				addAccountIntent.PutExtra ("Milestone", Newtonsoft.Json.JsonConvert.SerializeObject (temp));

				StartActivity (addAccountIntent);
			}
			else {

				MilestoneSelected = MilestonesHelper.GetMilestonesDetail (temp.Id.Value);

				DisplayMilestone (MilestoneSelected);
			}

		}

		public void DisplayMilestone(MilestonesDetailList obj){
			
			frameLayout1.Visibility = ViewStates.Visible;

			var MilestoneName = FindViewById<TextView> (Resource.Id.tv_MilestoneDetailName);
			MilestoneName.Text = obj.Title;


			var MilestoneStatus = FindViewById<TextView> (Resource.Id.tv_Status);
			MilestoneStatus.Text = obj.StatusName;

			var cb_Internal  = FindViewById<CheckBox> (Resource.Id.cb_Internal); 
			if(obj.IsInternal.HasValue)
			cb_Internal.Checked = obj.IsInternal.Value;

			var tv_ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectName);
			tv_ProjectName.Text = obj.ProjectName;

			var DueDate = FindViewById<TextView> (Resource.Id.tv_DueDate);
			if(obj.EndDateString!=null)
				DueDate.Text = obj.EndDateString;

			var ExpectedEndDate = FindViewById<TextView> (Resource.Id.tv_CompletedDate);
			if(obj.ActualEndDateString!=null)
				ExpectedEndDate.Text = obj.ActualEndDateString;


			//			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
			//			if(obj.Description!=null)
			//				Description.Text = obj.Description;

			//			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
			//			DepartmentName.Text = obj.DepartmentName;

		}

		//Init menu on action bar
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			if (Settings.Orientation.Equals ("Portrait")) {
				inflater.Inflate (Resource.Menu.SearchMenu, menu);
			}
			else
				inflater.Inflate (Resource.Menu.EditSearchMenu, menu);
			
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

