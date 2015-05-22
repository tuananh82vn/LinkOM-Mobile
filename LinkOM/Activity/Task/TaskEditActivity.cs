
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
	[Activity (Label = "TaskEditActivity", Theme = "@style/Theme.Customtheme")]			
	public class TaskEditActivity : Activity
	{
		public string results;

		const int Start_DATE_DIALOG_ID = 0;
		const int End_DATE_DIALOG_ID = 1;
		const int Actual_Start_DATE_DIALOG_ID = 2;
		const int Actual_End_DATE_DIALOG_ID = 3;


		public TaskDetailList TaskDetail;

		public ProjectSpinnerAdapter projectList;
		public ProjectLabelSpinnerAdapter labelList;
		public ProjectPhaseSpinnerAdapter phaseList;
		public StatusSpinnerAdapter statusList;
		public StaffSpinnerAdapter OwnerStaffList; 
		public StaffSpinnerAdapter AssignToStaffList;


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

		public EditText editText_Description ;

		public Spinner spinner_Phase ;

		public Spinner spinner_Label;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.TaskEdit);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.task_title_edit);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			LoadTask ();

			InitControl ();

			GetProjectList ();

			GetStatusList ();

			GetPriorityList ();

			GetOwnerStaffList (TaskDetail.ProjectId);

			GetAssignToStaffList (TaskDetail.ProjectId);

			GetPhaseList (TaskDetail.ProjectId);

			GetLabelList (TaskDetail.ProjectId);

			DisplayTask (TaskDetail);

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}
		}

		public void LoadTask(){

			results= Intent.GetStringExtra ("Task");

			TaskDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskDetailList> (results);
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

			editText_Description = FindViewById<EditText> (Resource.Id.editText_Description);

			spinner_Phase = FindViewById<Spinner> (Resource.Id.spinner_Phase);

			spinner_Label= FindViewById<Spinner> (Resource.Id.spinner_Label);

			if (TaskDetail.StartDate != null)
				StartDate = TaskDetail.StartDate;
			else
				StartDate = DateTime.Today;

			editText_StartDate.Click += delegate { ShowDialog (Start_DATE_DIALOG_ID); };
			editText_EndDate.Click += delegate { ShowDialog (End_DATE_DIALOG_ID); };
			editText_ActualStartDate.Click += delegate { ShowDialog (Actual_Start_DATE_DIALOG_ID); };
			editText_ActualEndDate.Click += delegate { ShowDialog (Actual_End_DATE_DIALOG_ID); };

		}

		private void GetProjectList(){

			projectList = new ProjectSpinnerAdapter (this,ProjectHelper.GetProjectList());

			spinner_Project.Adapter = projectList;

			spinner_Project.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (project_ItemSelected);
			if(TaskDetail.ProjectId!=null)
			spinner_Project.SetSelection(projectList.getPositionById (TaskDetail.ProjectId));

		}

		private void project_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_ProjectID = projectList.GetItemAtPosition (e.Position).Id.Value;
		}

		private void GetStatusList(){

			statusList = new StatusSpinnerAdapter (this,TaskHelper.GetTaskStatus());

			spinner_Status.Adapter = statusList;

			spinner_Status.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (status_ItemSelected);

			if(TaskDetail.StatusName!=null)
				spinner_Status.SetSelection(statusList.getPositionByName (TaskDetail.StatusName));
		}

		private void status_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_StatusID = statusList.GetItemAtPosition (e.Position).Id;
		}

		private void GetOwnerStaffList(int ProjectId){

			SearchAssignedByProject objFilter = new SearchAssignedByProject ();
			objFilter.ProjectId = ProjectId;

			OwnerStaffList = new StaffSpinnerAdapter (this,StaffHelper.GetOwnerByProject(objFilter));
			spinner_Owner.Adapter = null;

			spinner_Owner.Adapter = OwnerStaffList;

			spinner_Owner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (OwnerStaff_ItemSelected);
			if(TaskDetail.OwnerId!=null)
			spinner_Owner.SetSelection(OwnerStaffList.getPositionById (TaskDetail.OwnerId));
		}

		private void GetAssignToStaffList(int ProjectId){

			SearchAssignedByProject objFilter = new SearchAssignedByProject ();
			objFilter.ProjectId = ProjectId;

			AssignToStaffList = new StaffSpinnerAdapter (this,StaffHelper.GetAssignedToByProject(objFilter));

			spinner_AssignedTo.Adapter = null;

			spinner_AssignedTo.Adapter = AssignToStaffList;

			spinner_AssignedTo.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (AssignToStaff_ItemSelected);
			if(TaskDetail.AssignedToId!=null)
			spinner_AssignedTo.SetSelection(AssignToStaffList.getPositionById (TaskDetail.AssignedToId));
		}

		private void OwnerStaff_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_OwnerStaffID = OwnerStaffList.GetItemAtPosition (e.Position).Id;
		}

		private void AssignToStaff_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_AssignToStaffID = AssignToStaffList.GetItemAtPosition (e.Position).Id;
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

			if (obj.AllocatedHours.HasValue)
				editText_AllocatedHours.Text = obj.AllocatedHours.Value.ToString ();
			else
				editText_AllocatedHours.Text = "";

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

			PriorityAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.TaskPriority, Resource.Layout.SpinnerItemDropdown);
			//PriorityAdapter.SetDropDownViewResource (Android.Resource.Layout.SelectDialogSingleChoice);

			spinner_Priority.Adapter = PriorityAdapter;

			if(TaskDetail.PriorityName!=""){
				int index = PriorityAdapter.GetPosition (TaskDetail.PriorityName);
				spinner_Priority.SetSelection(index); 
			}

			spinner_Priority.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Priority_ItemSelected);
		}



		private void GetPhaseList(int ProjectId)
		{

			phaseList = new ProjectPhaseSpinnerAdapter (this,PhaseHelper.GetProjectPhaseByProject(ProjectId));

			spinner_Phase.Adapter = phaseList;

			spinner_Phase.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Phase_ItemSelected);

			if(TaskDetail.ProjectPhaseId.HasValue)
				spinner_Phase.SetSelection(phaseList.getPositionById(TaskDetail.ProjectPhaseId.Value));
				
		}

		private void GetLabelList(int ProjectId){

			labelList = new ProjectLabelSpinnerAdapter (this,LabelHelper.GetProjectLabelByProject(ProjectId));

			spinner_Label.Adapter = labelList;

			spinner_Label.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Label_ItemSelected);

			if(TaskDetail.Label!=null)
			spinner_Label.SetSelection(labelList.getPositionByName(TaskDetail.Label));

		}

		private void Label_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_Label = labelList.GetItemAtPosition (e.Position).Name;
		}


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

		private void Phase_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_PhaseID = phaseList.GetItemAtPosition (e.Position).Id;
		}

		public void btSaveClick()
		{
			
			TaskEdit TaskObject = new TaskEdit ();
			TaskObject.Id = TaskDetail.Id;
			TaskObject.Title = editText_Title.Text;
			TaskObject.ProjectId = Selected_ProjectID;
			TaskObject.TaskStatusId = Selected_StatusID;
			TaskObject.PriorityId = Selected_PriorityID;
			TaskObject.ProjectPhaseId = Selected_PhaseID;
			TaskObject.Label = Selected_Label;

			if (editText_StartDate.Text != "")
				TaskObject.StartDate = DateTime.Parse(editText_StartDate.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);
			
			if (editText_EndDate.Text != "")
				TaskObject.EndDate = DateTime.Parse(editText_EndDate.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);
			
			if (editText_ActualStartDate.Text != "")
				TaskObject.ActualStartDate = DateTime.Parse(editText_ActualStartDate.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);
			
			if (editText_ActualEndDate.Text != "")
				TaskObject.ActualEndDate = DateTime.Parse(editText_ActualEndDate.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);
			
			TaskObject.IsInternal = cb_Internal.Checked;
			TaskObject.IsManagerial = cb_Management.Checked;
			TaskObject.AssignedToId = Selected_AssignToStaffID;
			TaskObject.OwnerId = Selected_OwnerStaffID;

			if (editText_AllocatedHours.Text != "")
				TaskObject.AllocatedHours = Double.Parse(editText_AllocatedHours.Text);

			TaskObject.Description = editText_Description.Text;

			ApiResultSave restult = TaskHelper.EditTask (TaskObject);
			if (restult != null) {
				if (restult.Success) {
					OnBackPressed ();
					Toast.MakeText (this, "Task Saved", ToastLength.Short).Show ();
				}
				else
					Toast.MakeText (this, restult.ErrorMessage, ToastLength.Short).Show ();
			}
			else
				Toast.MakeText (this, "Server problem...", ToastLength.Short).Show ();
		}

		// the event received when the user "sets" the date in the dialog
		void OnStartDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			editText_StartDate.Text = e.Date.ToString ("dd'/'MM'/'yyyy");
		}

		// the event received when the user "sets" the date in the dialog
		void OnEndDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			editText_EndDate.Text = e.Date.ToString ("dd'/'MM'/'yyyy");
		}

		// the event received when the user "sets" the date in the dialog
		void OnActualStartDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			editText_ActualStartDate.Text = e.Date.ToString ("dd'/'MM'/'yyyy");
		}

		// the event received when the user "sets" the date in the dialog
		void OnActualEndDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			editText_ActualEndDate.Text = e.Date.ToString ("dd'/'MM'/'yyyy");
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

