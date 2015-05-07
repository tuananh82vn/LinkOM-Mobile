using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Graphics;
using Android.Support.V4.Widget;
using Android.Util;

using RadialProgress;
using System.Timers;
using Android.Views.InputMethods;
using Android.Text;

namespace LinkOM
{
	[Activity (Label = "Issues", Theme = "@style/Theme.Customtheme")]					
	public class IssuesActivity : Activity, TextView.IOnEditorActionListener
	{
		public LinearLayout LinearLayout_Master;
//		public ProgressDialog progress;
		public List<Button> buttonList;
		public List<IssuesList> issuesList;
		public IssuesListAdapter issuesListAdapter;

		public ListView issuesListView ;

		public RadialProgressView progressView;
		private System.Timers.Timer _timer;

		public EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;

		public InputMethodManager inputManager;

		public IssuesList IssuesSelected;

		public FrameLayout frame_IssuesDetail;

		protected override void OnCreate (Bundle bundle)
		{

				base.OnCreate (bundle);

				RequestWindowFeature (WindowFeatures.ActionBar);


				SetContentView (Resource.Layout.Issues);

				
				// Create your application here

				ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
				ActionBar.SetTitle (Resource.String.issues_title);
				ActionBar.SetDisplayShowTitleEnabled (true);
				ActionBar.SetDisplayHomeAsUpEnabled (true);
				ActionBar.SetHomeButtonEnabled (true);

				if(!Settings.Orientation.Equals("Portrait"))
				{
					mSearch = FindViewById<EditText>(Resource.Id.etSearch);
					mSearch.Alpha = 0;
					mSearch.SetOnEditorActionListener (this);
					mSearch.Focusable = false;
					mSearch.FocusableInTouchMode = false;
					mSearch.TextChanged += InputSearchOnTextChanged;
					inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
					frame_IssuesDetail  = FindViewById<FrameLayout> (Resource.Id.frame_taskdetail);
					frame_IssuesDetail.Visibility = ViewStates.Invisible;

				}

				progressView = FindViewById<RadialProgressView> (Resource.Id.tinyProgress);
				progressView.MinValue = 0;
				progressView.MaxValue = 100;




				//Lock Orientation
				if (Settings.Orientation.Equals ("Portrait")) {
					RequestedOrientation = ScreenOrientation.SensorPortrait;
				} else {
					RequestedOrientation = ScreenOrientation.SensorLandscape;
				}

				_timer = new System.Timers.Timer (10);
				_timer.Elapsed += HandleElapsed;
				_timer.Start ();

				ThreadPool.QueueUserWorkItem (o => GetIssuesStatus ());
		}

		private void InputSearchOnTextChanged(object sender, TextChangedEventArgs args)
		{
			issuesListAdapter.Filter.InvokeFilter(mSearch.Text);
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
				case Resource.Id.add:
					Intent Intent2 = new Intent (this, typeof(IssuesAddActivity));
					Intent2.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity(Intent2);
					break;

				case Resource.Id.edit:
					if (IssuesSelected != null) {
						Intent Intent = new Intent (this, typeof(IssuesEditActivity));
						Intent.PutExtra ("Issues", Newtonsoft.Json.JsonConvert.SerializeObject (IssuesSelected));
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

		//Init menu on action bar
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			if (Settings.Orientation.Equals ("Portrait")) {
				inflater.Inflate (Resource.Menu.AddMenu, menu);
			}
			else
				inflater.Inflate (Resource.Menu.AddEditSearchMenu, menu);
			return true;
		}

		void HandleElapsed (object sender, ElapsedEventArgs e)
		{
			progressView.Value ++;
			if (progressView.Value >= 100) {
				progressView.Value = 0;
			}
		}

		public void GetIssuesStatus ()
		{
			
			IssuesFilter objFilter = new IssuesFilter ();

			issuesList = IssuesHelper.GetIssuesList (objFilter);


			//Init layout
			LinearLayout_Master = FindViewById<LinearLayout>(Resource.Id.linearLayout_Main);

			string url = Settings.InstanceURL;

			string url_IssuesStatusList= url+"/api/IssueStatusList";

			string results_IssuesList= ConnectWebAPI.Request(url_IssuesStatusList,"");

			if (results_IssuesList != null && results_IssuesList != "") {

				JsonData data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results_IssuesList);

				StatusList statusList = Newtonsoft.Json.JsonConvert.DeserializeObject<StatusList> (data.Data);

				if (statusList.Items.Count > 0) {

					buttonList = new List<Button> (statusList.Items.Count);


					for (int i = 0; i < statusList.Items.Count; i++) {
						//Init button
						Button button = new Button (this);

						//Get number of task
						int NumberOfIssues = CheckIssues (statusList.Items [i].Name, issuesList);

						//Add button into View
						AddRow (statusList.Items [i].Id ,statusList.Items [i].Name,ColorHelper.GetColor(statusList.Items [i].ColourName),button, NumberOfIssues);

						RunOnUiThread (() => button.Text =  NumberOfIssues.ToString());

						buttonList.Add (button);
					}
				}
			}



			RunOnUiThread (() => progressView.Visibility=ViewStates.Invisible);
		}


		private int CheckIssues(string status, List<IssuesList>  list_Issues){
			int count = 0;
			foreach (var task in list_Issues) {
				if (task.StatusName == status)
					count++;
			}
			return count;
		}

		private void AddRow(int id,string Title, Color color, Button button, int NumberOfIssues){

			TableRow tableRow = new TableRow (this);
			TableRow.LayoutParams layoutParams_TableRow = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent,dpToPx(70));
			layoutParams_TableRow.TopMargin = dpToPx(1);
			layoutParams_TableRow.BottomMargin = dpToPx(1);
			tableRow .LayoutParameters = layoutParams_TableRow;

			LinearLayout LinearLayout_Inside = new LinearLayout (this);
			TableRow.LayoutParams layoutParams_Linear = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent,dpToPx(70));
			LinearLayout_Inside.LayoutParameters = layoutParams_Linear;
			LinearLayout_Inside.Orientation = Orientation.Horizontal;

			TextView textView = new TextView (this);
			TableRow.LayoutParams layoutParams_textView = new TableRow.LayoutParams (dpToPx(280), dpToPx(70));
			layoutParams_textView.LeftMargin = dpToPx (10);
			textView.LayoutParameters = layoutParams_textView;
			textView.Gravity = GravityFlags.CenterVertical;
			textView.TextSize = 20;

			if (Settings.Orientation.Equals ("Portrait"))
				textView.Text = Title;
			else {
				textView.Text = Title + " (" + NumberOfIssues.ToString () + ")";
			}

			textView.Click += HandleMyButton;
			textView.Tag = id;

			TableRow.LayoutParams layoutParams_button = new TableRow.LayoutParams (dpToPx(70), dpToPx(70));
			button.LayoutParameters = layoutParams_button;
			button.Background =  Resources.GetDrawable(Resource.Drawable.RoundButton);
			button.Text="0";
			button.Gravity = GravityFlags.Center;
			button.SetTextColor (Color.Black);
			button.SetBackgroundColor (color);
			button.Tag = id;
			button.Click += HandleMyButton;


			View view = new View (this);
			TableRow.LayoutParams layoutParams_view = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, dpToPx(1));
			view.LayoutParameters = layoutParams_view;
			view.SetBackgroundColor (Color.ParseColor("#AEAEAE"));

			RunOnUiThread (() => LinearLayout_Inside.AddView (textView));

			if(Settings.Orientation.Equals("Portrait"))
				RunOnUiThread (() => LinearLayout_Inside.AddView (button));

			RunOnUiThread (() => tableRow.AddView (LinearLayout_Inside));
			RunOnUiThread (() => LinearLayout_Master.AddView (tableRow));
			RunOnUiThread (() => LinearLayout_Master.AddView (view));
		}

		private int dpToPx(int dp)
		{
			float density = Resources.DisplayMetrics.Density;
			return Int32.Parse(Math.Round((float)dp * density).ToString());
		}

		public void btBackClick(object sender, EventArgs e)
		{
			_timer.Stop ();
			this.Finish ();
			OnBackPressed ();
		}

		private void HandleMyButton(object sender, EventArgs e)
		{
			int whichOne = 0;
			if (Settings.Orientation.Equals ("Portrait")) {
				Button myObject1 = (Button)sender;
				whichOne = (int)myObject1.Tag;

				var activity = new Intent (this, typeof(IssuesListActivity));
				activity.PutExtra ("IssuesStatusId", whichOne);
				StartActivity (activity);
			}
			else
			{
					issuesListView = FindViewById<ListView> (Resource.Id.IssuesListView);
					TextView myObject2 = (TextView)sender;
					whichOne = (int)myObject2.Tag;
					GetIssuesListByStatusId (whichOne);
			}
		}


		private void GetIssuesListByStatusId(int StatusId){

			if (StatusId != 0) {

				IssuesFilter objFilter = new IssuesFilter ();
				objFilter.IssueStatusId = StatusId;

				var objReturn = IssuesHelper.GetIssuesList (objFilter);

					if (objReturn != null && objReturn.Count>0) {

						issuesListAdapter = new IssuesListAdapter (this, objReturn);

						issuesListView.Adapter = issuesListAdapter;

						issuesListView.ItemClick += listView_ItemClick;
					} 
					else 
					{
						issuesListView.Adapter = null;
						Toast.MakeText (this, "No Issues Available.", ToastLength.Short).Show ();

					}

					frame_IssuesDetail.Visibility = ViewStates.Invisible;
				}
		}

		//handle list item clicked
		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			IssuesSelected = this.issuesListAdapter.GetItemAtPosition (e.Position);
			frame_IssuesDetail.Visibility = ViewStates.Visible;
			DisplayIssues (IssuesSelected);
		}

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Search) {
				if (!string.IsNullOrEmpty (mSearch.Text)) {
					issuesListAdapter.Filter.InvokeFilter(mSearch.Text);
				} 
				return true;
				//next action will set focus to password edit text.
			} 
			return false;
		}

		public void btSearchClick()
		{
			if (issuesListAdapter != null) {
				if (!mAnimatedDown) {
					mSearch.Focusable = true;
					mSearch.FocusableInTouchMode = true;
					mSearch.RequestFocus ();
					MyAnimation anim = new MyAnimation (issuesListView, issuesListView.Height - mSearch.Height);
					anim.Duration = 500;
					issuesListView.StartAnimation (anim);
					anim.AnimationStart += anim_AnimationStartDown;
					anim.AnimationEnd += anim_AnimationEndDown;
					issuesListView.Animate ().TranslationYBy (mSearch.Height).SetDuration (500).Start ();

					inputManager.ShowSoftInput (mSearch, ShowFlags.Implicit);

				} else {

					mSearch.Focusable = false;
					mSearch.FocusableInTouchMode = false;

					MyAnimation anim = new MyAnimation (issuesListView, issuesListView.Height + mSearch.Height);
					anim.Duration = 500;
					issuesListView.StartAnimation (anim);
					anim.AnimationStart += anim_AnimationStartUp;
					anim.AnimationEnd += anim_AnimationEndUp;
					issuesListView.Animate ().TranslationYBy (-mSearch.Height).SetDuration (500).Start ();

					inputManager.HideSoftInputFromWindow (this.mSearch.WindowToken, 0);


				}

				mAnimatedDown = !mAnimatedDown;
			}
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

		public void DisplayIssues(IssuesList obj)
		{

			var IssuesName = FindViewById<TextView> (Resource.Id.tv_IssuesDetailName);
			IssuesName.Text = obj.Title;

			var Status = FindViewById<TextView> (Resource.Id.tv_DetailStatus);
			Status.Text = obj.StatusName;

			var Priority = FindViewById<TextView> (Resource.Id.tv_Priority);
			Priority.Text = obj.PriorityName;

			//			var Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);
			//			Internal.Checked = obj.IsInternal;

			//			var Management = FindViewById<CheckBox> (Resource.Id.cb_Management);
			//			Management.Checked = obj.IsManagement;


			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectDetailName);
			ProjectName.Text = obj.ProjectName;

			var tv_AssignedTo = FindViewById<TextView> (Resource.Id.tv_AssignedTo);
			tv_AssignedTo.Text = obj.AssignedToName;


			//			var Label = FindViewById<TextView> (Resource.Id.tv_Label);
			//			Label.Text = obj.Label;
			//
			//			var Type = FindViewById<TextView> (Resource.Id.tv_Type);
			//			Type.Text = obj.IssuesTypeName;
			//
			//			var Receive  = FindViewById<TextView> (Resource.Id.tv_Receive);
			//			Receive.Text = obj.IssuesReceivedMethodName;


			var AlloHours = FindViewById<TextView> (Resource.Id.tv_AlloHours);
			AlloHours.Text = obj.AllocatedHours.ToString();

			//			var ActualHours  = FindViewById<TextView> (Resource.Id.tv_ActualHours);
			//			ActualHours.Text = obj.ActualHours.ToString();

			//			var StartDate = FindViewById<TextView> (Resource.Id.tv_StartDate);
			//			if(obj.StartDate!=null)
			//				StartDate.Text = obj.StartDateString;
			//
			//			var EndDate = FindViewById<TextView> (Resource.Id.tv_EndDate);
			//			if(obj.EndDate!=null)
			//				EndDate.Text = obj.EndDateString;


			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
			if(obj.Description!=null)
				Description.Text = obj.Description;

			//			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
			//			DepartmentName.Text = obj.DepartmentName;

		}
	}
}

