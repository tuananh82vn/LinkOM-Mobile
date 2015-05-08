
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

namespace LinkOM
{
	[Activity (Label = "TaskDetailActivity", Theme = "@style/Theme.Customtheme")]	
	public class TaskDetailActivity : Activity
	{
		private ImageButton overflowButton;
		public long ProjectId;
		public TaskDetailList TaskDetail;
		public string results;

		public ListView taskCommentListView ;
		public TaskCommentListAdapter taskCommentListAdapter;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.TaskDetailLayout);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.task_title_detail);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			LoadTask ();

			if(TaskDetail!=null)
			DisplayTask (TaskDetail);

			if(TaskDetail!=null)
			LoadTaskComment (TaskDetail.Id);

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

		}


		public void LoadTask()
		{

			var TaskId = Intent.GetIntExtra ("TaskId", 0);

			if (TaskId != 0) 
			{
				TaskDetail = LoadTaskDetail (TaskId);
			}
		}

		public TaskDetailList LoadTaskDetail(int taskid){

			if (CheckLoginHelper.CheckLogin ()) 
			{
				return TaskHelper.GetTaskDetail (taskid);
			} 
			else 
			{
				var activity = new Intent (this, typeof(LoginActivity));
				activity.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
				StartActivity (activity);
				Finish();
				return null;
			}
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			inflater.Inflate (Resource.Menu.FullMenu, menu);

			return true;
		}

		public void LoadTaskComment(int TaskId){
			
			taskCommentListAdapter = new TaskCommentListAdapter (this, TaskHelper.GetTaskCommentList(TaskId));

			taskCommentListView = FindViewById<ListView> (Resource.Id.TaskCommentListView);

			taskCommentListView.Adapter = taskCommentListAdapter;

			taskCommentListView.DividerHeight = 0;

			Utility.setListViewHeightBasedOnChildren (taskCommentListView);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			base.OnOptionsItemSelected (item);

			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					OnBackPressed ();
					break;
				case Resource.Id.edit:
					Intent Intent = new Intent (this, typeof(TaskEditActivity));
					Intent.PutExtra ("Task", results);
					Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity(Intent);
					break;

				default:
					break;
			}

			return true;
		}

		public void DisplayTask(TaskDetailList obj){

			var TaskName = FindViewById<TextView> (Resource.Id.tv_TaskName);
			TaskName.Text = obj.Title;

			var Code = FindViewById<TextView> (Resource.Id.tv_Code);
			Code.Text = obj.Code;

			var Status = FindViewById<TextView> (Resource.Id.tv_Status);
			Status.Text = obj.StatusName;

			var Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);
			Internal.Checked = obj.IsInternal;

			var Management = FindViewById<CheckBox> (Resource.Id.cb_Management);
			Management.Checked = obj.IsManagerial;

//			var Completed = FindViewById<TextView> (Resource.Id.tv_Completed);
//			Completed.Text = obj.Completed;


			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = obj.ProjectName;

//			var ProjectManager = FindViewById<TextView> (Resource.Id.tv_ProjectManager);
//			ProjectManager.Text = obj.ProjectManagerName;

			var Phase = FindViewById<TextView> (Resource.Id.tv_Phase);
			Phase.Text = obj.ProjectPhaseName;


//			var Label = FindViewById<TextView> (Resource.Id.tv_Label);
//			Label.Text = obj.Label;


			var AlloHours = FindViewById<TextView> (Resource.Id.tv_AlloHours);
			if (obj.AllocatedHours.HasValue) {
				AlloHours.Text = obj.AllocatedHours.Value.ToString ();
			}
			else
				AlloHours.Text="0";

//			var SpentHours = FindViewById<TextView> (Resource.Id.tv_SpentHours);
//			SpentHours.Text = obj.SpentHours;

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
			if(obj.Description!=null)
				Description.Text = obj.Description;

//			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
//			DepartmentName.Text = obj.DepartmentName;

		}
	}
}

