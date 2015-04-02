
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

namespace LinkOM
{
	[Activity (Label = "TaskList")]			
	public class TaskListActivity : Activity
	{
		public List<TaskObject> _TaskList;
		public TaskListAdapter taskList;
		public int StatusId;
		public SwipeRefreshLayout refresher;
		public bool loading;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.TaskListContainer);

			var buttonBack = FindViewById(Resource.Id.BackButton);
			buttonBack.Click += btBackClick;

			StatusId= Intent.GetIntExtra ("StatusId",0);

			InitData ();

			refresher = FindViewById<SwipeRefreshLayout> (Resource.Id.refresher);

			refresher.SetColorScheme (Resource.Color.xam_green,Resource.Color.xam_purple,Resource.Color.xam_gray,Resource.Color.xam_dark_blue);

			refresher.Refresh += HandleRefresh;
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

						var taskListView = FindViewById<ListView> (Resource.Id.TaskListView);

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

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{

			TaskObject model = this.taskList.GetItemAtPosition (e.Position);

			var activity = new Intent (this, typeof(EditTaskActivity));

			activity.PutExtra ("Task", Newtonsoft.Json.JsonConvert.SerializeObject(model));

			StartActivity (activity);

			this.Finish ();

		}
	}
}

