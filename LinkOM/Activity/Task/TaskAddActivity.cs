
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
	[Activity (Label = "TaskAddActivity", Theme = "@style/Theme.Customtheme")]			
	public class TaskAddActivity : Activity
	{
		public string results;

		const int Start_DATE_DIALOG_ID = 0;
		const int End_DATE_DIALOG_ID = 1;
		const int Actual_Start_DATE_DIALOG_ID = 2;
		const int Actual_End_DATE_DIALOG_ID = 3;


		public TaskDetailList TaskDetail;

		public ProjectSpinnerAdapter projectList; 

		public ProjectPhaseSpinnerAdapter phaseList; 
		public ProjectLabelSpinnerAdapter labelList; 


		public StaffSpinnerAdapter AssignToStaffList; 
		public StaffSpinnerAdapter OwnerStaffList; 

		public ArrayAdapter PriorityAdapter;

		public DateTime StartDate;
		public DateTime EndDate;
		public DateTime ActualStartDate;
		public DateTime ActualEndDate;


		public int Selected_ProjectID;
		public int Selected_AssignToStaffID;
		public int Selected_OwnerStaffID;

		public int Selected_PriorityID;
		public int Selected_StatusID;
		public int Selected_PhaseID;
		public string Selected_Label;

		public EditText editText_Title;

		public Spinner spinner_Project ;

		public CheckBox cb_Internal ;

		public CheckBox cb_Management ;

		public Spinner spinner_Status ;

		public Spinner spinner_Priority ;

		public Spinner spinner_AssignedTo ;

		public Spinner spinner_Owner;

		public EditText editText_StartDate ;

		public EditText editText_EndDate ;

		public EditText editText_ActualStartDate ;

		public EditText editText_ActualEndDate ;

		public EditText editText_AllocatedHours ;

		public EditText editText_SpentHours ;

		public EditText editText_Description ;

		public Spinner spinner_Phase ;

		public Spinner spinner_Label;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.TaskEdit);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.task_title_add);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			InitControl ();

			GetProjectList ();

			GetStatusList ();

			GetPriorityList ();

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

		}


		public void InitControl(){

			editText_Title = FindViewById<EditText> (Resource.Id.editText_Title);

			spinner_Project = FindViewById<Spinner> (Resource.Id.spinner_Project);

			cb_Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);

			cb_Management = FindViewById<CheckBox> (Resource.Id.cb_Management);

			spinner_Status = FindViewById<Spinner> (Resource.Id.spinner_Status);

			spinner_Priority = FindViewById<Spinner> (Resource.Id.spinner_Priority);

			spinner_AssignedTo = FindViewById<Spinner> (Resource.Id.spinner_AssignedTo);

			spinner_Owner= FindViewById<Spinner> (Resource.Id.spinner_Owner);

			editText_StartDate = FindViewById<EditText> (Resource.Id.editText_StartDate);

			editText_EndDate = FindViewById<EditText> (Resource.Id.editText_EndDate);

			editText_ActualStartDate = FindViewById<EditText> (Resource.Id.editText_ActualStartDate);

			editText_ActualEndDate = FindViewById<EditText> (Resource.Id.editText_ActualEndDate);

			editText_AllocatedHours = FindViewById<EditText> (Resource.Id.editText_AllocatedHours);

			editText_SpentHours = FindViewById<EditText> (Resource.Id.editText_SpentHours);

			editText_Description = FindViewById<EditText> (Resource.Id.editText_Description);

			spinner_Phase = FindViewById<Spinner> (Resource.Id.spinner_Phase);

			spinner_Label= FindViewById<Spinner> (Resource.Id.spinner_Label);

			editText_StartDate.Click += delegate { ShowDialog (Start_DATE_DIALOG_ID); };
			editText_EndDate.Click += delegate { ShowDialog (End_DATE_DIALOG_ID); };
			editText_ActualStartDate.Click += delegate { ShowDialog (Actual_Start_DATE_DIALOG_ID); };
			editText_ActualEndDate.Click += delegate { ShowDialog (Actual_End_DATE_DIALOG_ID); };

			StartDate = DateTime.Today;
			EndDate = DateTime.Today;
			ActualStartDate = DateTime.Today;
			ActualEndDate = DateTime.Today;
		}



		public void DisplayTask(TaskDetailList obj){

			editText_Title.Text = obj.Title;

			if(obj.IsInternal)
				cb_Internal.Checked = obj.IsInternal;

			if(obj.IsManagerial)
				cb_Management.Checked = obj.IsManagerial;


			editText_StartDate.Text = obj.StartDateString;

			editText_EndDate.Text = obj.EndDateString;

			editText_ActualStartDate.Text = obj.ActualStartDateString;

			editText_ActualEndDate.Text = obj.ActualEndDateString;

			editText_AllocatedHours.Text = obj.AllocatedHours.Value.ToString();

			editText_Description.Text = obj.TaskDescription;

		}

		//Handle item on action bar clicked
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			base.OnOptionsItemSelected (item);

			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				OnBackPressed ();
				break;

			case Resource.Id.save:
				btSaveClick ();
				break;

			default:
				break;
			}

			return true;
		}

		//Init menu on action bar
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			inflater.Inflate (Resource.Menu.SaveMenu, menu);

			return true;
		}

		private void GetPriorityList(){
			//Handle priority

			PriorityAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.TaskPriority, Android.Resource.Layout.SimpleSpinnerItem);
			PriorityAdapter.SetDropDownViewResource (Android.Resource.Layout.SelectDialogSingleChoice);
			spinner_Priority.Adapter = PriorityAdapter;


			spinner_Priority.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Priority_ItemSelected);
		}

		private void GetStatusList(){

			var StatusAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.TaskStatus, Android.Resource.Layout.SimpleSpinnerItem);
			StatusAdapter.SetDropDownViewResource (Android.Resource.Layout.SelectDialogSingleChoice);
			spinner_Status.Adapter = StatusAdapter;


			//spinner_Status.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Status_ItemSelected);
		}

		private void GetPhaseList(int ProjectId)
		{

			phaseList = new ProjectPhaseSpinnerAdapter (this,PhaseHelper.GetProjectPhaseByProject(ProjectId));

			spinner_Phase.Adapter = phaseList;

			spinner_Phase.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Phase_ItemSelected);
		}

		private void GetProjectList(){

			projectList = new ProjectSpinnerAdapter (this,ProjectHelper.GetProjectList());

			spinner_Project.Adapter = projectList;

			spinner_Project.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (project_ItemSelected);

		}

		private void GetLabelList(int ProjectId){

			labelList = new ProjectLabelSpinnerAdapter (this,LabelHelper.GetProjectLabelByProject(ProjectId));

			spinner_Label.Adapter = labelList;

			spinner_Label.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Label_ItemSelected);

		}

		private void GetAssignToStaffList(int ProjectId){

			SearchAssignedByProject objFilter = new SearchAssignedByProject ();
			objFilter.ProjectId = ProjectId;

			AssignToStaffList = new StaffSpinnerAdapter (this,StaffHelper.GetAssignedToByProject(objFilter));

			spinner_AssignedTo.Adapter = null;

			spinner_AssignedTo.Adapter = AssignToStaffList;

			spinner_AssignedTo.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (AssignToStaff_ItemSelected);
		}

		private void GetOwnerStaffList(int ProjectId){

			SearchAssignedByProject objFilter = new SearchAssignedByProject ();
			objFilter.ProjectId = ProjectId;

			OwnerStaffList = new StaffSpinnerAdapter (this,StaffHelper.GetOwnerByProject(objFilter));
			spinner_Owner.Adapter = null;

			spinner_Owner.Adapter = OwnerStaffList;

			spinner_Owner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (OwnerStaff_ItemSelected);
		}

//		private void Status_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
//		{
//			Selected_StatusID =  statusList.GetItemAtPosition (e.Position).Id;
//		}

		private void Priority_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;
			string PriorityName = spinner.GetItemAtPosition (e.Position).ToString();
			if(PriorityName.Equals("Low")){
				Selected_PriorityID=1;
			}
			else if(PriorityName.Equals("Medium")){
				Selected_PriorityID=2;
			}
			else if(PriorityName.Equals("High")){
				Selected_PriorityID=3;
			}
		}

		private void Label_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_Label = labelList.GetItemAtPosition (e.Position).Name;
		}

		private void Phase_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_PhaseID = phaseList.GetItemAtPosition (e.Position).Id;
		}

		private void AssignToStaff_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_AssignToStaffID = AssignToStaffList.GetItemAtPosition (e.Position).Id;
		}

		private void OwnerStaff_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_OwnerStaffID = OwnerStaffList.GetItemAtPosition (e.Position).Id;
		}

		private void project_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_ProjectID = projectList.GetItemAtPosition (e.Position).Id.Value;

			GetAssignToStaffList (Selected_ProjectID);

			GetOwnerStaffList (Selected_ProjectID);

			GetPhaseList (Selected_ProjectID);

			GetLabelList (Selected_ProjectID);
		}


		public void btSaveClick()
		{

			TaskAdd TaskObject = new TaskAdd ();
			TaskObject.Title = editText_Title.Text;
			TaskObject.ProjectId = Selected_ProjectID;
			TaskObject.TaskStatusId = 1;
			TaskObject.PriorityId = Selected_PriorityID;
			TaskObject.ProjectPhaseId = Selected_PhaseID;
			TaskObject.Label = Selected_Label;
			TaskObject.StartDate = DateTime.Parse(editText_StartDate.Text);
			TaskObject.EndDate = DateTime.Parse(editText_EndDate.Text);
			TaskObject.ActualStartDate = DateTime.Parse(editText_ActualStartDate.Text);
			TaskObject.ActualEndDate = DateTime.Parse(editText_ActualEndDate.Text);
			TaskObject.IsInternal = cb_Internal.Checked;
			TaskObject.IsManagerial = cb_Management.Checked;
			TaskObject.AssignedToId = Selected_AssignToStaffID;
			TaskObject.OwnerId = Selected_OwnerStaffID;

			if (editText_AllocatedHours.Text != "")
				TaskObject.AllocatedHours = Double.Parse(editText_AllocatedHours.Text);
			
			if (editText_SpentHours.Text != "")
				TaskObject.SpentHours = Double.Parse(editText_SpentHours.Text);
			
			TaskObject.Description = editText_Description.Text;

			ApiResultSave restult = TaskHelper.AddTask (TaskObject);

			if (restult.Success) {
				OnBackPressed ();
				Toast.MakeText (this, "Task Saved", ToastLength.Short).Show ();

			} else
				Toast.MakeText (this, restult.ErrorMessage, ToastLength.Short).Show ();
		}

		// the event received when the user "sets" the date in the dialog
		void OnStartDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			editText_StartDate.Text = e.Date.ToString ("d");
		}

		// the event received when the user "sets" the date in the dialog
		void OnEndDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			editText_EndDate.Text = e.Date.ToString ("d");
		}

		// the event received when the user "sets" the date in the dialog
		void OnActualStartDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			editText_ActualStartDate.Text = e.Date.ToString ("d");
		}

		// the event received when the user "sets" the date in the dialog
		void OnActualEndDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			editText_ActualEndDate.Text = e.Date.ToString ("d");
		}

		protected override Dialog OnCreateDialog (int id)
		{
			switch (id) {
			case Start_DATE_DIALOG_ID:
				return new DatePickerDialog (this, OnStartDateSet, StartDate.Year, StartDate.Month - 1, StartDate.Day); 
			case End_DATE_DIALOG_ID:
				return new DatePickerDialog (this, OnEndDateSet, EndDate.Year, EndDate.Month - 1, EndDate.Day); 

			case Actual_Start_DATE_DIALOG_ID:
				return new DatePickerDialog (this, OnActualStartDateSet, ActualStartDate.Year, ActualStartDate.Month - 1, ActualStartDate.Day); 
			case Actual_End_DATE_DIALOG_ID:
				return new DatePickerDialog (this, OnActualEndDateSet, ActualEndDate.Year, ActualEndDate.Month - 1, ActualEndDate.Day); 
			}
			return null;
		}
	}
}

