
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
	[Activity (Label = "TaskList", Theme = "@style/Theme.Customtheme")]			
	public class TaskListActivity : Activity, TextView.IOnEditorActionListener
	{
		public List<IssuesList> _TaskList;
		public TaskListAdapter taskList;

		public int StatusId;
		public SwipeRefreshLayout refresher;
		public bool loading;

		private EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;

		public ListView taskListView ;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.TaskListContainer);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.task_title);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			taskListView = FindViewById<ListView> (Resource.Id.TaskListView);

			mSearch = FindViewById<EditText>(Resource.Id.etSearch);
			mSearch.Alpha = 0;
			mSearch.SetOnEditorActionListener (this);
			mSearch.TextChanged += InputSearchOnTextChanged;

//
//			var inputManager = (InputMethodManager)GetSystemService(InputMethodService);
//			inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.None);

			StatusId= Intent.GetIntExtra ("TaskStatusId",0);

			InitData ();

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} 
			else 
			{
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

			var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
			fab.AttachToListView(taskListView);
			fab.Click += Fab_Click;
		}

		void Fab_Click (object sender, EventArgs e)
		{
			Intent Intent2 = new Intent (this, typeof(TaskAddActivity));
			Intent2.SetFlags (ActivityFlags.ClearWhenTaskReset);
			StartActivity(Intent2);
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
			taskList.Filter.InvokeFilter(mSearch.Text);
		}


		private void InitData(){
			
			if (StatusId != 0) {

				if (loading)
					return;
				loading = true;
				
				TaskFilter objFilter = new TaskFilter ();
				objFilter.AssignedToId = Settings.UserId;
				objFilter.MainStatusId = StatusId;
				objFilter.TaskStatusId = StatusId.ToString ();
					
				taskList = new TaskListAdapter (this, TaskHelper.GetTaskList(objFilter));

				taskListView.Adapter = taskList;

				taskListView.ItemClick += listView_ItemClick;


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
				MyAnimation anim = new MyAnimation(taskListView, taskListView.Height - mSearch.Height);
				anim.Duration = 500;
				taskListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartDown;
				anim.AnimationEnd += anim_AnimationEndDown;
				taskListView.Animate().TranslationYBy(mSearch.Height).SetDuration(500).Start();
			}

			else
			{
				//Listview is down
				MyAnimation anim = new MyAnimation(taskListView, taskListView.Height + mSearch.Height);
				anim.Duration = 500;
				taskListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartUp;
				anim.AnimationEnd += anim_AnimationEndUp;
				taskListView.Animate().TranslationYBy(-mSearch.Height).SetDuration(500).Start();
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
			mSearch.Animate().AlphaBy(1.0f).SetDuration(1000).Start();
		}

		void anim_AnimationStartUp(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
		{
			mIsAnimating = true;
			mSearch.Animate().AlphaBy(-1.0f).SetDuration(1000).Start();
		}

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			TaskList model = this.taskList.GetItemAtPosition (e.Position);

			var activity = new Intent (this, typeof(TaskDetailActivity));

			activity.PutExtra ("TaskId", model.Id);

			ActivityOptions opts = ActivityOptions.MakeCustomAnimation (this, Resource.Animation.fade, Resource.Animation.hold);

			StartActivity (activity, opts.ToBundle ());
		}

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Search) {
				if (!string.IsNullOrEmpty (mSearch.Text)) {
					taskList.Filter.InvokeFilter(mSearch.Text);
				} 
				return true;
				//next action will set focus to password edit text.
			} 
			return false;
		}
	}
}

