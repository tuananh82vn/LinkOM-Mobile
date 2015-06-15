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
using com.refractored.fab;

namespace LinkOM
{
	[Activity (Label = "Issues", Theme = "@style/Theme.Customtheme")]					
	public class IssuesActivity : Activity, TextView.IOnEditorActionListener
	{
		public LinearLayout LinearLayout_Master;
		public List<Button> buttonList;
		public List<IssuesList> issuesList;
		public IssuesListAdapter issuesListAdapter;

		public ListView issuesListView ;

		public EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;

		public InputMethodManager inputManager;

		public IssuesDetailList IssuesSelected;

		public FrameLayout frame_IssuesDetail;

		public FloatingActionButton fab;

		public IssuesCommentListAdapter issuesCommentListAdapter; 

		public ListView issuesCommentListView;

		public ProgressDialog progress;

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

					issuesListView = FindViewById<ListView> (Resource.Id.IssuesListView);

				}

				progress = new ProgressDialog (this,Resource.Style.StyledDialog);
				progress.Indeterminate = true;
				progress.SetMessage("Please wait...");
				progress.SetCancelable (true);
				progress.Show ();




				//Lock Orientation
				if (Settings.Orientation.Equals ("Portrait")) {
					RequestedOrientation = ScreenOrientation.SensorPortrait;
				} else {
					RequestedOrientation = ScreenOrientation.SensorLandscape;
				}

				ThreadPool.QueueUserWorkItem (o => GetIssuesStatus ());
		}

		void Fab_Click (object sender, EventArgs e)
		{
			Intent Intent2 = new Intent (this, typeof(IssuesAddActivity));
			Intent2.SetFlags (ActivityFlags.ClearWhenTaskReset);
			StartActivity(Intent2);
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

		public void GetIssuesStatus ()
		{

			if (!Settings.Orientation.Equals ("Portrait")) 
			{
				fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
				if (fab != null) {
					fab.Visibility = ViewStates.Invisible;
					fab.Hide ();
					fab.AttachToListView (issuesListView);
					fab.Click += Fab_Click;
				}
			}

			IssuesFilter objFilter = new IssuesFilter ();

			issuesList = IssuesHelper.GetIssuesList (objFilter);


			//Init layout
			LinearLayout_Master = FindViewById<LinearLayout>(Resource.Id.linearLayout_Main);


				var statusList = IssuesHelper.GetIssuesStatusList ();

				if(statusList!=null){

					if (statusList.Count > 0) {

						List<Status> SortedList = statusList.OrderBy(o=>o.DisplayOrder).ToList();

						buttonList = new List<Button> (SortedList.Count);


						for (int i = 0; i < SortedList.Count; i++) {
							//Init button
							Button button = new Button (this);

							int NumberOfIssues = 0;

							//Get number of task
							if(issuesList!=null)
								NumberOfIssues = CheckIssues (SortedList [i].Name, issuesList);

							//Add button into View
							AddRow (SortedList [i].Id ,SortedList [i].Name,ColorHelper.GetColor(SortedList [i].ColourName),button, NumberOfIssues);

							//button.Text = NumberOfIssues.ToString ();
							
							RunOnUiThread (() => button.Text =  NumberOfIssues.ToString());

							buttonList.Add (button);
						}
					}
				}
				RunOnUiThread (() => progress.Dismiss ());
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
			textView.TextSize = 18;

			if (Settings.Orientation.Equals ("Portrait"))
				textView.Text = Title;
			else 
			{
					textView.Text = Title + " (" + NumberOfIssues.ToString () + ")";
			}

			textView.Tag = id;
			textView.Touch += (sender, TouchEventArgs)=>{
				textViewOnTouch(NumberOfIssues, id, LinearLayout_Inside , color, TouchEventArgs);
			};

			TableRow.LayoutParams layoutParams_button = new TableRow.LayoutParams (dpToPx(70), dpToPx(70));
			button.LayoutParameters = layoutParams_button;
			button.Background =  Resources.GetDrawable(Resource.Drawable.RoundButton);
			button.Text="0";
			button.Gravity = GravityFlags.Center;
			if (color == Color.White) {
				button.SetBackgroundColor (Color.Gray);
			}
			else			
				button.SetBackgroundColor (color);


			if (color == Color.Black || color == Color.Blue || color == Color.Purple) {
				button.SetTextColor (Color.White);
			}
			else
				button.SetTextColor (Color.Black);

			button.Tag = id;
			button.Click += (sender, eventArgs) => {
				if(NumberOfIssues!=0)
				{
					if (Settings.Orientation.Equals ("Portrait")) 
					{
						var activity = new Intent (this, typeof(IssuesListActivity));
						activity.PutExtra ("IssuesStatusId", id);
						StartActivity (activity);
					}
					else
					{
						GetIssuesListByStatusId (id);
					}
				}
				else
				{
					Toast.MakeText (this, "No Issue Available.", ToastLength.Short).Show ();

				}
			};


			View view = new View (this);
			TableRow.LayoutParams layoutParams_view = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, dpToPx(1));
			view.LayoutParameters = layoutParams_view;
			view.SetBackgroundColor (Color.ParseColor("#AEAEAE"));


			RunOnUiThread (() => LinearLayout_Inside.AddView (textView));

			if (Settings.Orientation.Equals ("Portrait"))
				RunOnUiThread (() => LinearLayout_Inside.AddView (button));

			RunOnUiThread (() => tableRow.AddView (LinearLayout_Inside));
			RunOnUiThread (() => LinearLayout_Master.AddView (tableRow));
			RunOnUiThread (() => LinearLayout_Master.AddView (view));
		}


		private void textViewOnTouch(int NumberOfIssues ,int id, LinearLayout LinearLayout_Inside,Color color,  View.TouchEventArgs touchEventArgs)
		{

			switch (touchEventArgs.Event.Action & MotionEventActions.Mask) 
			{
			case MotionEventActions.Down:

			case MotionEventActions.Move:
				LinearLayout_Inside.SetBackgroundColor (color);
				break;

			case MotionEventActions.Up:
				LinearLayout_Inside.SetBackgroundColor (Color.White);

				if (NumberOfIssues != 0) {

					if (Settings.Orientation.Equals ("Portrait")) {
						var activity = new Intent (this, typeof(IssuesListActivity));
						activity.PutExtra ("IssuesStatusId", id);
						StartActivity (activity);
					}
					//Landscape
					else 
					{
						GetIssuesListByStatusId (id);
					}
				} 
				else 
				{
					Toast.MakeText (this, "No Issue Available.", ToastLength.Short).Show ();
				}
				break;

			default:
				break;
			}

		}	

		private int dpToPx(int dp)
		{
			float density = Resources.DisplayMetrics.Density;
			return Int32.Parse(Math.Round((float)dp * density).ToString());
		}

//		private void HandleMyText(object sender, EventArgs e)
//		{
//			
//		}

//		private void HandleMyButton(object sender, EventArgs e)
//		{
//			int whichOne = 0;
//			Button myObject2 = (Button)sender;
//			whichOne = (int)myObject2.Tag;
//
//			if (Settings.Orientation.Equals ("Portrait")) {
//				var activity = new Intent (this, typeof(IssuesListActivity));
//				activity.PutExtra ("IssuesStatusId", whichOne);
//				StartActivity (activity);
//			}
//
//			else
//			{
//				GetIssuesListByStatusId (whichOne);
//			}
//		}


		private void GetIssuesListByStatusId(int StatusId){

			if (StatusId != 0) {

				IssuesFilter objFilter = new IssuesFilter ();
				objFilter.IssueStatusId = StatusId;

				var objReturn = IssuesHelper.GetIssuesList (objFilter);

					if (objReturn != null && objReturn.Count>0) {

						issuesListAdapter = new IssuesListAdapter (this, objReturn);

						issuesListView.Adapter = issuesListAdapter;

						issuesListView.ItemClick += listView_ItemClick;

						fab.Visibility = ViewStates.Visible;
						fab.Show ();
					} 
					else 
					{
						fab.Visibility = ViewStates.Invisible;
						fab.Hide ();

						issuesListView.Adapter = null;
						Toast.MakeText (this, "No Issues Available.", ToastLength.Short).Show ();

					}

					frame_IssuesDetail.Visibility = ViewStates.Invisible;
				}
		}

		//handle list item clicked
		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var temp = this.issuesListAdapter.GetItemAtPosition (e.Position);
			frame_IssuesDetail.Visibility = ViewStates.Visible;

			IssuesSelected = IssuesHelper.GetIssuesDetail (temp.Id.Value);

			DisplayIssues (IssuesSelected);

			LoadIssuesComment (IssuesSelected.Id.Value);
		}

		public void LoadIssuesComment(int IssuesId){

			issuesCommentListAdapter = new IssuesCommentListAdapter (this, IssuesHelper.GetIssuesCommentList(IssuesId));

			issuesCommentListView = FindViewById<ListView> (Resource.Id.IssuesCommentListView);

			issuesCommentListView.Adapter = issuesCommentListAdapter;

			issuesCommentListView.DividerHeight = 0;

			Utility.SetListViewHeightBasedOnChildren (issuesCommentListView);

			//ticketCommentListView.ItemClick += listView_ItemClick;

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

		public void DisplayIssues(IssuesDetailList obj)
		{

			var IssuesName = FindViewById<TextView> (Resource.Id.tv_IssuesDetailName);
			IssuesName.Text = obj.Title;

			var Status = FindViewById<TextView> (Resource.Id.tv_DetailStatus);
			Status.Text = obj.StatusName;

			var tv_CodeIssues = FindViewById<TextView> (Resource.Id.tv_CodeIssues);
			tv_CodeIssues.Text = obj.IssueCode;

			var Priority = FindViewById<TextView> (Resource.Id.tv_Priority);
			Priority.Text = obj.PriorityName;


			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectDetailName);
			ProjectName.Text = obj.ProjectName;

			var tv_AssignedTo = FindViewById<TextView> (Resource.Id.tv_AssignedTo);
			tv_AssignedTo.Text = obj.AssignedToName;

			var tv_Owner = FindViewById<TextView> (Resource.Id.tv_Owner);
			tv_Owner.Text = obj.OwnerName;

			var AlloHours = FindViewById<TextView> (Resource.Id.tv_AlloHours);
			AlloHours.Text = obj.AllocatedHours.ToString();

			var OpenDate = FindViewById<TextView> (Resource.Id.tv_OpenDate);
			if(obj.OpenDateString!=null)
				OpenDate.Text = obj.OpenDateString;
			
			var tv_ClosedDate = FindViewById<TextView> (Resource.Id.tv_ClosedDate);
			if(obj.CloseDateString!=null)
			tv_ClosedDate.Text = obj.CloseDateString;

			var tv_ResDate = FindViewById<TextView> (Resource.Id.tv_ResDate);
			if(obj.RessolutionTargetDateString!=null)
				tv_ResDate.Text = obj.RessolutionTargetDateString;

			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
			if(obj.IssueDescription!=null)
				Description.Text = obj.IssueDescription;


			var Action = FindViewById<TextView> (Resource.Id.tv_Action);
			if(obj.IssueAction!=null)
				Action.Text = obj.IssueAction;
		}

		public override void OnBackPressed()
		{
			this.Finish ();
			base.OnBackPressed();
		}
	}
}

