
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
	[Activity (Label = "TaskDetailActivity", Theme = "@style/Theme.Customtheme")]	
	public class TaskDetailActivity : Activity
	{
		private ImageButton overflowButton;
		public long ProjectId;
		public TaskObject TaskDetail;
		public string results;

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

			DisplayTask (TaskDetail);

		}


		public void LoadTask(){
			
			results= Intent.GetStringExtra ("Task");

			TaskDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskObject> (results);
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			inflater.Inflate (Resource.Menu.FullMenu, menu);

			return true;
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

		public void DisplayTask(TaskObject obj){

			var TaskName = FindViewById<TextView> (Resource.Id.tv_TaskName);
			TaskName.Text = obj.Title;

			var Code = FindViewById<TextView> (Resource.Id.tv_Code);
			Code.Text = obj.Code;

			var Status = FindViewById<TextView> (Resource.Id.tv_Status);
			Status.Text = obj.TaskStatus;

			var Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);
			Internal.Checked = obj.IsInternal.Value;

			var Management = FindViewById<CheckBox> (Resource.Id.cb_Management);
			Management.Checked = obj.IsManagerial.Value;

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
			AlloHours.Text = obj.AllocatedHours;

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

