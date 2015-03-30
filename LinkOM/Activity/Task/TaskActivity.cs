
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

namespace LinkOM
{
	[Activity (Label = "Task")]				
	public class TaskActivity : Activity
	{
		public TaskList obj ;

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

//			Button bt_Complete = FindViewById<Button>(Resource.Id.bt_Complete);
//			bt_Complete.Click += CompleteTaskClick;
//
//			Button bt_Future= FindViewById<Button>(Resource.Id.bt_Future);
//			bt_Future.Click += FutureTaskClick;

			string url = Settings.InstanceURL;

			//Get all status task

			string url2= url+"/api/TaskStatusList";

			string results1= ConnectWebAPI.Request(url2,"");

			if (results1 != null && results1 != "") {

				JsonData data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results1);

				StatusList statusList = Newtonsoft.Json.JsonConvert.DeserializeObject<StatusList> (data.Data);

//				if(statusList.Items.Count>0){
//
//					TableRow [] TableRowList = new TableRow [statusList.Items.Count];
//
//					LinearLayout [] LinearLayoutList = new LinearLayout [statusList.Items.Count];
//
//					TextView [] TextViewList = new TextView [statusList.Items.Count];
//
//					Button [] ButtonList = new Button [statusList.Items.Count];
//
//					for (int i = 1; i < statusList.Items.Count; i++) {
//
//
//						TextViewList [i] = new TextView (this);
//						ViewGroup.LayoutParams layoutParams_TextView = new ViewGroup.LayoutParams (120, 35);
//						TextViewList [i].LayoutParameters = layoutParams_TextView;
//						TextViewList [i].Gravity = GravityFlags.Center;
//						TextViewList [i].SetBackgroundColor (Color.ParseColor ("#ff045FBC"));
//						TextViewList [i].SetTextColor(Color.ParseColor("#ffffffff"));
//						TextViewList [i].TextSize = 20;
//						TextViewList [i].Text = statusList.Items [i].Name;
//
//						//						p1:layout_marginLeft="100dp"
//						//						p1:textStyle="normal"
//
//						//						ButtonList [i] = new Button (this);
//
//						LinearLayoutList [i] = new LinearLayout (this);
//						LinearLayout.LayoutParams layoutParams_Linear = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,ViewGroup.LayoutParams.WrapContent);
//						LinearLayoutList [i].LayoutParameters = layoutParams_Linear;
//						LinearLayoutList [i].Orientation = Orientation.Horizontal;
//
//						LinearLayoutList [i].AddView (TextViewList [i],0);
//
//
//						TableRowList [i] = new TableRow (this);
//						TableRow.LayoutParams layoutParams_TableRow = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent,ViewGroup.LayoutParams.WrapContent);
//						layoutParams_TableRow.TopMargin = 15;
//						TableRowList [i] .LayoutParameters = layoutParams_TableRow;
//
//						TableRowList [i].AddView (LinearLayoutList [i],0);
//
//						linear.AddView (TableRowList [i]);
//
//					}
//				}

			}


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
					bt_Open.Text = CheckTask ("Open", obj.Items).ToString ();
					bt_Closed.Text = CheckTask ("Closed", obj.Items).ToString ();
					bt_Wating.Text= CheckTask ("Waiting On Client", obj.Items).ToString ();
					bt_Progress.Text= CheckTask ("In Progress", obj.Items).ToString ();
					bt_Query.Text= CheckTask ("On Hold", obj.Items).ToString ();
//					bt_Complete.Text= CheckTask ("Complete", obj.Items).ToString ();
//					bt_Future.Text= CheckTask ("Future", obj.Items).ToString ();
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

		public void QueryTaskClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(TaskListActivity));
			activity.PutExtra ("StatusId", 5);
			StartActivity (activity);
		}
//
//		public void CompleteTaskClick(object sender, EventArgs e)
//		{
//			var activity = new Intent (this, typeof(TaskListActivity));
//			activity.PutExtra ("StatusId", 5);
//			StartActivity (activity);
//		}
//
//		public void FutureTaskClick(object sender, EventArgs e)
//		{
//			var activity = new Intent (this, typeof(TaskListActivity));
//			activity.PutExtra ("StatusId", 6);
//			StartActivity (activity);
//		}

		private int CheckTask(string status, List<Task>  list_Task){
			int count = 0;
			foreach (var task in list_Task) {
				if (task.StatusName == status)
					count++;
			}
			return count;
		}

	
	}
}

