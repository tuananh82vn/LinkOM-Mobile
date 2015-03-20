
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

namespace LinkOM
{
	[Activity (Label = "Task",ScreenOrientation = ScreenOrientation.Portrait)]				
	public class TaskActivity : Activity
	{
		public TaskListDetailJson obj2 ;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.TaskView);
			// Create your application here

			ImageButton button = FindViewById<ImageButton>(Resource.Id.bt_Back);
			button.Click += btBackClick;

			ImageButton bt_Add = FindViewById<ImageButton>(Resource.Id.bt_Add);
			bt_Add.Click += btAddClick;

			Button bt_Open = FindViewById<Button>(Resource.Id.bt_Open);
			bt_Open.Click += btOpenClick;

			Button bt_Closed = FindViewById<Button>(Resource.Id.bt_Close);
			bt_Closed.Click += btClosedClick;

			Button bt_Wating = FindViewById<Button>(Resource.Id.bt_Waiting);
			bt_Wating.Click += WaitingTaskClick;

			Button bt_Progress = FindViewById<Button>(Resource.Id.bt_Progress);
			bt_Progress.Click += ProgressTaskClick;

			Button bt_Query = FindViewById<Button>(Resource.Id.bt_Query);
			bt_Query.Click += QueryTaskClick;

			Button bt_Complete = FindViewById<Button>(Resource.Id.bt_Complete);
			bt_Complete.Click += CompleteTaskClick;

			Button bt_Future= FindViewById<Button>(Resource.Id.bt_Future);
			bt_Future.Click += FutureTaskClick;

//			TextView tv_AllTask = FindViewById<TextView>(Resource.Id.tv_AllTask);
//			tv_AllTask.Click += AllTaskClick;
//
//			TextView tv_OpenTask = FindViewById<TextView>(Resource.Id.tv_OpenTask);
//			tv_OpenTask.Click += OpenTaskClick;
//
//			TextView tv_OverDueTask = FindViewById<TextView>(Resource.Id.tv_OverDueTask);
//			tv_OverDueTask.Click += OverDueTaskClick;
//
//			TextView tv_AssignByMeTask = FindViewById<TextView>(Resource.Id.tv_AssignByMeTask);
//			tv_AssignByMeTask.Click += AssignByMeTaskClick;


//			string url = Settings.InstanceURL;
//
//			url=url+"/api/TaskList";
//
//
//			var objTask = new
//			{
//				Title = "Test",
//				AssignedToId = 2,
//				ClientId = string.Empty,
//				TaskStatusId = 1,
//				PriorityId = string.Empty,
//				DueBeforeDate = string.Empty,
//				DepartmentId = string.Empty,
//				ProjectId = string.Empty,
//				AssignByMe = string.Empty,
//				Filter = string.Empty,
//				Label = string.Empty,
//			};
//
//			var objsearch = (new
//				{
//					objApiSearch = new
//					{
//						UserId = 2,
//						PageSize = 20,
//						PageNumber = 1,
//						SortMember = string.Empty,
//						SortDirection = string.Empty,
//						MainStatusId = 1,
//						Item = objTask
//					}
//				});
//
//			string results= ConnectWebAPI.Request(url,objsearch);
//
//			if (results != null && results != "") {
//
//				DataJson obj = Newtonsoft.Json.JsonConvert.DeserializeObject<DataJson> (results);
//
//				obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskListDetailJson> (obj.Data);
//
//				tv_AllTask.Text = obj2.TotalRecords.ToString ();
//
//				if (obj2.Items != null) {
//					tv_OpenTask.Text = CheckTask ("Open", obj2.Items).ToString ();
//					tv_OverDueTask.Text = CheckTask ("OverDue", obj2.Items).ToString ();
//					tv_AssignByMeTask.Text = CheckTask ("AssignByMe", obj2.Items).ToString ();
//				} else {
//					tv_OpenTask.Text = "0";
//					tv_OverDueTask.Text = "0";
//					tv_AssignByMeTask.Text = "0";
//				}
//			}
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
			Toast.MakeText (this, "Open Task Clicked", ToastLength.Short).Show ();
		}

		public void btClosedClick(object sender, EventArgs e)
		{
			Toast.MakeText (this, "Closed Task Clicked", ToastLength.Short).Show ();
		}

		public void WaitingTaskClick(object sender, EventArgs e)
		{
			Toast.MakeText (this, "Waiting Task Clicked", ToastLength.Short).Show ();
		}

		public void ProgressTaskClick(object sender, EventArgs e)
		{
			Toast.MakeText (this, "In Progress Task Clicked", ToastLength.Short).Show ();
		}

		public void QueryTaskClick(object sender, EventArgs e)
		{
			Toast.MakeText (this, "Query Task Clicked", ToastLength.Short).Show ();
		}

		public void CompleteTaskClick(object sender, EventArgs e)
		{
			Toast.MakeText (this, "Complete Task Clicked", ToastLength.Short).Show ();
		}

		public void FutureTaskClick(object sender, EventArgs e)
		{
			Toast.MakeText (this, "Future Task Clicked", ToastLength.Short).Show ();
		}

//		private int CheckTask(string status, List<Task>  list_Task){
//			int count = 0;
//			foreach (var task in list_Task) {
//				if (task.StatusName == status)
//					count++;
//			}
//			return count;
//		}

	
	}
}

