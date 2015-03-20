
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

		public Task task1;
		public Task task2;
		public Task task3;
		public Task task4;
		public Task task5;
		public Task task6;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.TaskListView);

			ImageButton buttonBack = FindViewById<ImageButton>(Resource.Id.bt_Back);
			buttonBack.Click += btBackClick;

			//Init Data
			_TaskList = new List<Task>();

			task1 = new Task();
			task1.Id = 1;
			task1.Title="Design layout for Mobile app";
			task1.ProjectName = "Link-OM Product Development";
			task1.StartDate="13/03/15";
			task1.EndDate="16/03/15";
			task1.AssignedTo="Andy Pham";
			task1.Code="TSK-7054";
			task1.ActHours="3";
			task1.AllocatedHours="4";
			task1.StatusName="Open";

			_TaskList.Add (task1);

			task2 = new Task();
			task2.Id = 2;
			task2.Title="Database issues";
			task2.ProjectName = "Web Development";
			task2.StartDate="16/03/15";
			task2.EndDate="18/03/15";
			task2.AssignedTo="Khanh Le";
			task2.Code="TSK-7057";
			task2.ActHours="1";
			task2.AllocatedHours="2";
			task2.StatusName="Open";

			_TaskList.Add (task2);

			task3 = new Task();
			task3.Id = 3;
			task3.Title="Develop Login Page";
			task3.ProjectName = "VHI Caravan";
			task3.StartDate="11/03/14";
			task3.EndDate="18/03/15";
			task3.AssignedTo="Bhauvik";
			task3.Code="TSK-7157";
			task3.ActHours="33";
			task3.AllocatedHours="333";
			task3.StatusName="Close";

			_TaskList.Add (task3);

			task4 = new Task();
			task4.Id = 4;
			task4.Title="Fixing Admin page";
			task4.ProjectName = "Synotive Website";
			task4.StartDate="11/01/14";
			task4.EndDate="18/01/15";
			task4.AssignedTo="Swetha";
			task4.Code="TSK-71a7";
			task4.ActHours="31";
			task4.AllocatedHours="244";
			task4.StatusName="Open";
			_TaskList.Add (task4);

			task5 = new Task();
			task5.Id = 5;
			task5.Title="Generate Data Template";
			task5.ProjectName = "FIT system";
			task5.StartDate="13/01/14";
			task5.EndDate="12/02/15";
			task5.AssignedTo="David";
			task5.Code="TSK-7127";
			task5.ActHours="1";
			task5.AllocatedHours="12";
			task5.StatusName="Open";
			_TaskList.Add (task5);




			taskList = new TaskListAdapter (this,_TaskList);

			var taskListView = FindViewById<ListView> (Resource.Id.TaskListView);

			taskListView.Adapter = taskList;

			taskListView.ItemClick += listView_ItemClick;
		}

		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			//Get our item from the list adapter
			var TaskId = this.taskList.GetItemId(e.Position);
			var TaskName = this.taskList.GetItemName(e.Position);

			Toast.MakeText(this, string.Format("Selected task {0} with id {1}", TaskName, TaskId), ToastLength.Short).Show();
			//var activity = new Intent (this, typeof(TaskDetailActivity));
			//StartActivity (activity);


		}
	}
}

