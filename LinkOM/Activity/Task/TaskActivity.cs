
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
using Android.Content.PM;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Android.Graphics;
using System.Threading.Tasks;
using System.Threading;
using Android.Support.V4.Widget;

namespace LinkOM
{
	[Activity (Label = "Task")]				
	public class TaskActivity : Activity
	{
		public bool loading;

		public TaskList obj ;
		public ImageButton bt_Add;
		public Button bt_Open;
		public Button bt_Closed;
		public Button bt_Wating;
		public Button bt_Progress;
		public Button bt_Hold;
		public ProgressDialog progress;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Task);
			// Create your application here

			var BackButton = FindViewById(Resource.Id.BackButton);
			BackButton.Click += btBackClick;

			var AddButton = FindViewById(Resource.Id.AddButton);
			AddButton.Click += btAddClick;

			bt_Open = FindViewById<Button>(Resource.Id.bt_Open);
			bt_Open.SetBackgroundColor (Color.Blue);
			bt_Open.Click += btOpenClick;

			bt_Closed = FindViewById<Button>(Resource.Id.bt_Close);
			bt_Closed.SetBackgroundColor (Color.Green);
			bt_Closed.Click += btClosedClick;

			bt_Wating = FindViewById<Button>(Resource.Id.bt_Waiting);
			bt_Wating.SetBackgroundColor (Color.YellowGreen);
			bt_Wating.Click += WaitingTaskClick;

			bt_Progress = FindViewById<Button>(Resource.Id.bt_Progress);
			bt_Progress.SetBackgroundColor (Color.Orange);
			bt_Progress.Click += ProgressTaskClick;

			bt_Hold = FindViewById<Button>(Resource.Id.bt_Hold);
			bt_Hold.SetBackgroundColor (Color.BlueViolet);
			bt_Hold.Click += HoldTaskClick;

			progress = new ProgressDialog (this);
			progress.Indeterminate = true;
			progress.SetProgressStyle(ProgressDialogStyle.Spinner);
			progress.SetMessage("Loading Task...");
			progress.SetCancelable(false);
			progress.Show();

			ThreadPool.QueueUserWorkItem (o => InitData ());

		}


		public void InitData ()
		{


				string url = Settings.InstanceURL;
				// Get all Task
				url=url+"/api/TaskList";

				List<objSort> objSort = new List<objSort>{
					new objSort{ColumnName = "T.ProjectName", Direction = "1"},
					new objSort{ColumnName = "T.EndDate", Direction = "2"}
				};


				var objTask = new
				{
					Title = string.Empty,
					AssignedToId = string.Empty,
					ClientId = string.Empty,
					TaskStatusId = string.Empty,
					PriorityId = string.Empty,
					DueBeforeDate = string.Empty,
					DepartmentId = string.Empty,
					ProjectId = string.Empty,
					AssignByMe = true,
					Filter = string.Empty,
					Label = string.Empty,
				};

				var objsearch = (new
					{
						objApiSearch = new
						{
							UserId = Settings.UserId,
							TokenNumber =Settings.Token,
							PageSize = 100,
							PageNumber = 1,
							Sort = objSort,
							Item = objTask
						}
					});

				string results= ConnectWebAPI.Request(url,objsearch);

				if (results != null && results != "") {

					TaskList obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskList> (results);

					if (obj.Items != null) {

						var OpenTask = CheckTask ("Open", obj.Items).ToString ();
						var ClosedTask = CheckTask ("Closed", obj.Items).ToString ();
						var WaitingTask = CheckTask ("Waiting On Client", obj.Items).ToString ();
						var ProgressTask = CheckTask ("In Progress", obj.Items).ToString ();
						var OnHoldTask = CheckTask ("On Hold", obj.Items).ToString ();

						RunOnUiThread (() => bt_Open.Text =  OpenTask);
						RunOnUiThread (() => bt_Closed.Text =  ClosedTask);
						RunOnUiThread (() => bt_Wating.Text =  WaitingTask);
						RunOnUiThread (() => bt_Progress.Text =  ProgressTask);
						RunOnUiThread (() => bt_Hold.Text =  OnHoldTask);

						RunOnUiThread (() => progress.Dismiss());
					} 
				}
		}

		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}

		public void btAddClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(AddTaskActivity));
			StartActivity (activity);
		}


		public void btOpenClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(TaskListActivity));
			activity.PutExtra ("StatusId", 1);
			StartActivity (activity);
		}

		public void btClosedClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(TaskListActivity));
			activity.PutExtra ("StatusId", 2);
			StartActivity (activity);
		}

		public void WaitingTaskClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(TaskListActivity));
			activity.PutExtra ("StatusId", 3);
			StartActivity (activity);
		}

		public void ProgressTaskClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(TaskListActivity));
			activity.PutExtra ("StatusId", 4);
			StartActivity (activity);
		}

		public void HoldTaskClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(TaskListActivity));
			activity.PutExtra ("StatusId", 5);
			StartActivity (activity);
		}


		private int CheckTask(string status, List<TaskObject>  list_Task){
			int count = 0;
			foreach (var task in list_Task) {
				if (task.StatusName == status)
					count++;
			}
			return count;
		}

	
	}
}

