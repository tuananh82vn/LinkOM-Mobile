
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

namespace LinkOM
{
	[Activity (Label = "TaskList")]			
	public class TaskListActivity : Activity
	{
		public List<Task> _TaskList;
		public TaskListAdapter taskList;
		public int StatusId;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.TaskListView);

			ImageButton buttonBack = FindViewById<ImageButton>(Resource.Id.bt_Back);

			buttonBack.Click += btBackClick;

			StatusId= Intent.GetIntExtra ("StatusId",0);

			InitData ();
		}

		private void InitData(){
			
			if (StatusId != 0) {
				
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
			}
		}

//		protected override void OnResume(){
//			InitData ();
//			base.OnResume();
//		}

		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{

			Task model = this.taskList.GetItemAtPosition (e.Position);

			var activity = new Intent (this, typeof(EditTaskActivity));

			activity.PutExtra ("Task", Newtonsoft.Json.JsonConvert.SerializeObject(model));

			StartActivity (activity);

			this.Finish ();

		}
	}
}

