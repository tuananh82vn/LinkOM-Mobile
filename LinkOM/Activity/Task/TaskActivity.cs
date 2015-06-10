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
	[Activity (Label = "Task", Theme = "@style/Theme.Customtheme")]					
	public class TaskActivity : Activity, TextView.IOnEditorActionListener
	{
		public LinearLayout LinearLayout_Master;

		public List<Button> buttonList;
		public List<TaskList> taskList;
		public TaskListAdapter taskListAdapter;

		public ListView taskListView ;
		public EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;

		public InputMethodManager inputManager;

		public TaskDetailList TaskSelected;
		public FrameLayout frame_TaskDetail;
		public FloatingActionButton fab;

		public TaskCommentListAdapter taskCommentListAdapter;

		public ListView taskCommentListView;

		public ProgressDialog progress;

		protected override void OnCreate (Bundle bundle)
		{

				base.OnCreate (bundle);

				RequestWindowFeature (WindowFeatures.ActionBar);


				SetContentView (Resource.Layout.Task);

				
				// Create your application here

				ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
				ActionBar.SetTitle (Resource.String.task_title);
				ActionBar.SetDisplayShowTitleEnabled (true);
				ActionBar.SetDisplayHomeAsUpEnabled (true);
				ActionBar.SetHomeButtonEnabled (true);

				progress = new ProgressDialog (this,Resource.Style.StyledDialog);
				progress.Indeterminate = true;
				progress.SetMessage("Please wait...");
				progress.SetCancelable (true);
				progress.Show ();

				//Lock Orientation
				if (Settings.Orientation.Equals ("Portrait")) 
				{
					RequestedOrientation = ScreenOrientation.SensorPortrait;
				} 
				else 
				{

					mSearch = FindViewById<EditText>(Resource.Id.etSearch);
					mSearch.Alpha = 0;
					mSearch.SetOnEditorActionListener (this);
					mSearch.Focusable = false;
					mSearch.FocusableInTouchMode = false;
					mSearch.TextChanged += InputSearchOnTextChanged;
					inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
					frame_TaskDetail  = FindViewById<FrameLayout> (Resource.Id.frame_taskdetail);
					frame_TaskDetail.Visibility = ViewStates.Invisible;

					RequestedOrientation = ScreenOrientation.SensorLandscape;
					
					taskListView = FindViewById<ListView> (Resource.Id.TaskListView);


					
				}

				ThreadPool.QueueUserWorkItem (o => GetTaskStatus ());

		}

		void Fab_Click (object sender, EventArgs e)
		{
			Intent Intent2 = new Intent (this, typeof(TaskAddActivity));
			Intent2.SetFlags (ActivityFlags.ClearWhenTaskReset);
			StartActivity(Intent2);
		}

		private void InputSearchOnTextChanged(object sender, TextChangedEventArgs args)
		{
			taskListAdapter.Filter.InvokeFilter(mSearch.Text);
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
					Intent Intent2 = new Intent (this, typeof(TaskAddActivity));
					Intent2.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity(Intent2);
					break;
				case Resource.Id.edit:
				if (TaskSelected != null) 
				{
					Intent Intent = new Intent (this, typeof(TaskEditActivity));
					Intent.PutExtra ("Task", Newtonsoft.Json.JsonConvert.SerializeObject (TaskSelected));
					Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity (Intent);
				}
				else
					Toast.MakeText (this, "No Task Selected.", ToastLength.Short).Show ();
					
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

		public void GetTaskStatus ()
		{

			if (!Settings.Orientation.Equals ("Portrait")) 
			{
				fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
				if (fab != null) {
					fab.Visibility = ViewStates.Invisible;
					fab.AttachToListView (taskListView);
					fab.Click += Fab_Click;
				}
			}

			TaskFilter objFilter = new TaskFilter ();
			objFilter.AssignedToId = Settings.UserId;

			taskList = TaskHelper.GetTaskList (objFilter);

			//Init layout
			LinearLayout_Master = FindViewById<LinearLayout>(Resource.Id.linearLayout_Main);


			var statusList = TaskHelper.GetTaskStatus ();

			if (statusList != null) {

				if (statusList.Count > 0) {

					List<Status> SortedList = statusList.OrderBy(o=>o.DisplayOrder).ToList();

					buttonList = new List<Button> (SortedList.Count);


					for (int i = 0; i < SortedList.Count; i++) {
						//Init button
						Button button = new Button (this);
						int NumberOfTask = 0;

						if (taskList != null)
					//Get number of task
							NumberOfTask = CheckTask (SortedList [i].Name, taskList);

						//Add button into View
						AddRow (SortedList [i].Id, SortedList [i].Name, ColorHelper.GetColor (SortedList [i].ColourName), button, NumberOfTask);

						//button.Text = NumberOfTask.ToString ();
						
						RunOnUiThread (() => button.Text = NumberOfTask.ToString ());

						buttonList.Add (button);
					}
				}
			}
			RunOnUiThread (() => progress.Dismiss ());
		}


		private int CheckTask(string status, List<TaskList>  list_Task){
			int count = 0;
			foreach (var task in list_Task) {
				if (task.StatusName == status)
					count++;
			}
			return count;
		}

		private void AddRow(int id,string Title, Color color, Button button, int NumberOfTask){

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
			else {
				textView.Text = Title + " (" + NumberOfTask.ToString () + ")";
			}
			textView.Tag = id;
			textView.Click += (sender, eventArgs) =>{
				if (NumberOfTask != 0) {

					if (Settings.Orientation.Equals ("Portrait")) {
						var activity = new Intent (this, typeof(TaskListActivity));
						activity.PutExtra ("TaskStatusId", id);
						StartActivity (activity);
					}
					//Landscape
					else {
						GetTaskList (id);
					}
				} 
				else {
					Toast.MakeText (this, "No Task Available.", ToastLength.Short).Show ();
				}
			};


			TableRow.LayoutParams layoutParams_button = new TableRow.LayoutParams (dpToPx(70), dpToPx(70));
			button.LayoutParameters = layoutParams_button;
			button.Background =  Resources.GetDrawable(Resource.Drawable.RoundButton);
			button.Text="0";
			button.Gravity = GravityFlags.Center;
			button.Tag = id;
			button.Click += (sender, eventArgs) =>{
				if (NumberOfTask != 0) {

					if (Settings.Orientation.Equals ("Portrait")) {
						var activity = new Intent (this, typeof(TaskListActivity));
						activity.PutExtra ("TaskStatusId", id);
						StartActivity (activity);
					}
					//Landscape
					else {
						GetTaskList (id);
					}
				} 
				else {
					Toast.MakeText (this, "No Task Available.", ToastLength.Short).Show ();
				}
			};

			if (color == Color.White) {
				button.SetBackgroundColor (Color.Gray);
			}
			else
				button.SetBackgroundColor (color);
			
			if (color == Color.Black) {
				button.SetTextColor (Color.White);
			}
			else
				if (color == Color.Blue) {
					button.SetTextColor (Color.White);
				}
				else
					if (color == Color.Purple) {
						button.SetTextColor (Color.White);
					}
					else
						button.SetTextColor (Color.Black);

			button.Tag = id;
	//		button.Click += HandleMyButton;


			View view = new View (this);
			TableRow.LayoutParams layoutParams_view = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, dpToPx(1));
			view.LayoutParameters = layoutParams_view;
			view.SetBackgroundColor (Color.ParseColor("#E6E6E6"));

			RunOnUiThread (() => LinearLayout_Inside.AddView (textView));

			if (Settings.Orientation.Equals ("Portrait"))
				//LinearLayout_Inside.AddView (button);
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

		public override void OnBackPressed()
		{
			this.Finish ();
			base.OnBackPressed();
		}



		private void GetTaskList(int StatusId){

			if (StatusId != 0) {

				TaskFilter objFilter = new TaskFilter ();
				objFilter.AssignedToId = Settings.UserId;
				objFilter.TaskStatusId = StatusId.ToString();

				var objReturn = TaskHelper.GetTaskList (objFilter);

				if (objReturn != null && objReturn.Count !=0) 
				{

					taskListAdapter = new TaskListAdapter (this, objReturn);

					taskListView.Adapter = taskListAdapter;

					taskListView.ItemClick += listView_ItemClick;

					fab.Visibility = ViewStates.Visible;
					fab.Show ();
				} 
				else 
				{
					fab.Visibility = ViewStates.Invisible;
					fab.Hide ();
					taskListView.Adapter = null;
					Toast.MakeText (this, "No Task Available.", ToastLength.Short).Show ();
				}

				frame_TaskDetail.Visibility = ViewStates.Invisible;
			}
		}

		//handle list item clicked
		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			TaskSelected = TaskHelper.GetTaskDetail(this.taskListAdapter.GetItemAtPosition (e.Position).Id);

			frame_TaskDetail.Visibility = ViewStates.Visible;

			DisplayTask (TaskSelected);

			LoadTaskComment (TaskSelected.Id);
		}

		public void LoadTaskComment(int Id){

			taskCommentListAdapter = new TaskCommentListAdapter (this, TaskHelper.GetTaskCommentList(Id));

			taskCommentListView = FindViewById<ListView> (Resource.Id.TaskCommentListView);

			taskCommentListView.Adapter = taskCommentListAdapter;

			taskCommentListView.DividerHeight = 0;

			Utility.setListViewHeightBasedOnChildren (taskCommentListView);
		}

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Search) {
				if (!string.IsNullOrEmpty (mSearch.Text)) {
					taskListAdapter.Filter.InvokeFilter(mSearch.Text);
				} 
				return true;
				//next action will set focus to password edit text.
			} 
			return false;
		}

		public void btSearchClick()
		{
			if (taskListAdapter != null) {
				if (!mAnimatedDown) {
					mSearch.Focusable = true;
					mSearch.FocusableInTouchMode = true;
					mSearch.RequestFocus ();
					MyAnimation anim = new MyAnimation (taskListView, taskListView.Height - mSearch.Height);
					anim.Duration = 500;
					taskListView.StartAnimation (anim);
					anim.AnimationStart += anim_AnimationStartDown;
					anim.AnimationEnd += anim_AnimationEndDown;
					taskListView.Animate ().TranslationYBy (mSearch.Height).SetDuration (500).Start ();

					inputManager.ShowSoftInput (mSearch, ShowFlags.Implicit);

				} 
				else 
				{

					mSearch.Focusable = false;
					mSearch.FocusableInTouchMode = false;

					MyAnimation anim = new MyAnimation (taskListView, taskListView.Height + mSearch.Height);
					anim.Duration = 500;
					taskListView.StartAnimation (anim);
					anim.AnimationStart += anim_AnimationStartUp;
					anim.AnimationEnd += anim_AnimationEndUp;
					taskListView.Animate ().TranslationYBy (-mSearch.Height).SetDuration (500).Start ();

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

		public void DisplayTask(TaskDetailList obj)
		{

			var TaskName = FindViewById<TextView> (Resource.Id.tv_TaskDetailName);
			TaskName.Text = obj.Title;

			var Code = FindViewById<TextView> (Resource.Id.tv_Code);
			Code.Text = obj.Code;

			var Status = FindViewById<TextView> (Resource.Id.tv_StatusName);
			Status.Text = obj.StatusName;

			var Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);
			Internal.Checked = obj.IsInternal;
			Internal.Enabled = false;

			var Management = FindViewById<CheckBox> (Resource.Id.cb_Management);
			Management.Checked = obj.IsManagerial;
			Management.Enabled = false;

			var tv_Phase = FindViewById<TextView> (Resource.Id.tv_Phase);
			tv_Phase.Text = obj.ProjectPhaseName;

			var tv_Label = FindViewById<TextView> (Resource.Id.tv_Label);
			tv_Label.Text = obj.Label;

			 
			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectDetailName);
			ProjectName.Text = obj.ProjectName;

			var tv_ProjectDetailManager = FindViewById<TextView> (Resource.Id.tv_ProjectDetailManager);
			tv_ProjectDetailManager.Text = obj.ProjectManager;

			var tv_Department = FindViewById<TextView> (Resource.Id.tv_Department);
			tv_Department.Text = obj.DepartmentName;

			var tv_AssignedTo = FindViewById<TextView> (Resource.Id.tv_AssignedTo);
			tv_AssignedTo.Text = obj.AssignedToName;

			var tv_Owner = FindViewById<TextView> (Resource.Id.tv_Owner);
			tv_Owner.Text = obj.OwnerName;



			var AlloHours = FindViewById<TextView> (Resource.Id.tv_AlloHours);
			if (obj.AllocatedHours.HasValue) {
				AlloHours.Text = obj.AllocatedHours.Value.ToString ();
			} else
				AlloHours.Text = "0";


			var StartDate = FindViewById<TextView> (Resource.Id.tv_StartDate);
			if(obj.StartDate!=null)
				StartDate.Text = obj.StartDateString;

			var EndDate = FindViewById<TextView> (Resource.Id.tv_EndDate);
			if(obj.EndDate!=null)
				EndDate.Text = obj.EndDateString;

			var ActualStartDate = FindViewById<TextView> (Resource.Id.tv_ActualStartDate);
			if(obj.ActualStartDate!=null)
				ActualStartDate.Text = obj.ActualStartDateString;

			var ActualEndDate = FindViewById<TextView> (Resource.Id.tv_ActualEndDate);
			if(obj.ActualEndDate!=null)
				ActualEndDate.Text = obj.ActualEndDateString;

			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
			if(obj.TaskDescription!=null)
				Description.Text = obj.TaskDescription;

		}
	}
}

