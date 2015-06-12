
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
using Android.Text;
using System.Text.RegularExpressions;
using Android.Webkit;
using Android.Graphics;
using Android.Support.V7.Widget;

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

			//Console.WriteLine (taskCommentListView.Height);

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) 
			{
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} 
			else 
			{
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

			Utility.SetListViewHeightBasedOnChildren (taskCommentListView);

			Utility.SetListViewHeightBasedOnChildren2 (taskCommentListView,taskCommentListAdapter.GetHeight() + 150);
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
					Intent.PutExtra ("Task", Newtonsoft.Json.JsonConvert.SerializeObject (TaskDetail));
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

			var Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);
			Internal.Checked = obj.IsInternal;

			var Status = FindViewById<TextView> (Resource.Id.tv_Status);
			Status.Text = obj.StatusName;


			var Management = FindViewById<CheckBox> (Resource.Id.cb_Management);
			Management.Checked = obj.IsManagerial;

			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = obj.ProjectName;

			var ProjectManager = FindViewById<TextView> (Resource.Id.tv_ProjectManager);
			ProjectManager.Text = obj.ProjectManager;

			var tv_AssignedTo = FindViewById<TextView> (Resource.Id.tv_AssignedTo);
			tv_AssignedTo.Text = obj.AssignedToName;

			var tv_Owner = FindViewById<TextView> (Resource.Id.tv_Owner);
			tv_Owner.Text = obj.OwnerName;


			var Phase = FindViewById<TextView> (Resource.Id.tv_Phase);
			Phase.Text = obj.ProjectPhaseName;

			var Label = FindViewById<TextView> (Resource.Id.tv_Label);
			Label.Text = obj.Label;


			var AlloHours = FindViewById<TextView> (Resource.Id.tv_AlloHours);
			if (obj.AllocatedHours.HasValue) {
				AlloHours.Text = obj.AllocatedHours.Value.ToString ();
			}
			else
				AlloHours.Text="0";

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

			var Description = FindViewById<WebView> (Resource.Id.tv_Description);
			if (obj.TaskDescription != null) {
				Description.LoadData (Html.FromHtml(obj.TaskDescription).ToString(), "text/html", "utf8");
				Description.SetBackgroundColor(Color.Argb(1, 0, 0, 0));
				WebSettings webSettings = Description.Settings;
				webSettings.DefaultFontSize = 12;
			}

			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
			DepartmentName.Text = obj.DepartmentName;

		}
	}
}

