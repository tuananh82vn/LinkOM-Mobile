
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
	[Activity (Label = "TaskList")]			
	public class TaskListActivity : Activity, TextView.IOnEditorActionListener
	{
		public List<IssuesObject> _TaskList;
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

			SetContentView (Resource.Layout.TaskListContainer);

			taskListView = FindViewById<ListView> (Resource.Id.TaskListView);

			mSearch = FindViewById<EditText>(Resource.Id.etSearch);
			mSearch.Alpha = 0;
			mSearch.SetOnEditorActionListener (this);
			mSearch.TextChanged += InputSearchOnTextChanged;



			var buttonBack = FindViewById(Resource.Id.BackButton);
			buttonBack.Click += btBackClick;

			var SearchButton = FindViewById(Resource.Id.SearchButton);
			SearchButton.Click += btSearchClick;

			StatusId= Intent.GetIntExtra ("TaskStatusId",0);

			InitData ();

			refresher = FindViewById<SwipeRefreshLayout> (Resource.Id.refresher);

			refresher.SetColorScheme (Resource.Color.xam_green,Resource.Color.xam_purple,Resource.Color.xam_gray,Resource.Color.xam_dark_blue);

			refresher.Refresh += HandleRefresh;
		}

		private void InputSearchOnTextChanged(object sender, TextChangedEventArgs args)
		{
			taskList.Filter.InvokeFilter(mSearch.Text);
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

				url=url+"/api/TaskList";

				var objTask = new
				{
					Title = "",
					AssignedToId = string.Empty,
					ClientId = string.Empty,
					TaskStatusId = StatusId,
					PriorityId = string.Empty,
					DueBeforeDate = string.Empty,
					DepartmentId = string.Empty,
					ProjectId = string.Empty,
					AssignByMe = string.Empty,
					Filter = string.Empty,
					Label = string.Empty,
				};

				var objsearch = (new
					{
						objApiSearch = new
						{
							UserId = Settings.UserId,
							TokenNumber = Settings.Token,
							PageSize = 100,
							PageNumber = 1,
							SortMember = string.Empty,
							SortDirection = string.Empty,
							Item = objTask
						}
					});

				string results = ConnectWebAPI.Request (url, objsearch);

				if (results != null && results != "") {

					TaskList obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskList> (results);

					if (obj.Items != null) {

						taskList = new TaskListAdapter (this, obj.Items);

						taskListView.Adapter = taskList;

						taskListView.ItemClick += listView_ItemClick;
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
				MyAnimation anim = new MyAnimation(taskListView, taskListView.Height - mSearch.Height);
				anim.Duration = 500;
				taskListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartDown;
				anim.AnimationEnd += anim_AnimationEndDown;
				refresher.Animate().TranslationYBy(mSearch.Height).SetDuration(500).Start();
			}

			else
			{
				//Listview is down
				MyAnimation anim = new MyAnimation(taskListView, taskListView.Height + mSearch.Height);
				anim.Duration = 500;
				taskListView.StartAnimation(anim);
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

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{

			TaskObject model = this.taskList.GetItemAtPosition (e.Position);

			var activity = new Intent (this, typeof(EditTaskActivity));

			activity.PutExtra ("Task", Newtonsoft.Json.JsonConvert.SerializeObject(model));

			StartActivity (activity);

			this.Finish ();

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

