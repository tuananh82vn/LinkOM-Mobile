
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
	[Activity (Label = "IssuesEditActivity", Theme = "@style/Theme.Customtheme")]			
	public class IssuesEditActivity : Activity
	{
		public string results;

		const int Start_DATE_DIALOG_ID = 0;
		const int End_DATE_DIALOG_ID = 1;
		const int Res_DATE_DIALOG_ID = 2;


		public IssuesDetailList IssuesDetail;

		public ProjectSpinnerAdapter projectList; 
		public ArrayAdapter PriorityAdapter;

		public DateTime StartDate;
		public DateTime EndDate;
		public DateTime ResDate;

		public StaffSpinnerAdapter OwnerStaffList; 
		public StaffSpinnerAdapter AssignToStaffList;

		public int Selected_ProjectID;
		public int Selected_PriorityID;
		public int Selected_StatusID;
		public int Selected_PhaseID;
		public int Selected_OwnerStaffID;
		public int Selected_AssignToStaffID;


		public EditText editText_Title;

		public Spinner spinner_Project ;

		public Spinner spinner_Status ;

		public Spinner spinner_Priority ;

		public Spinner spinner_Label;

		public Spinner spinner_AssignedTo ;

		public Spinner spinner_Owner;


		public EditText editText_OpenDate ;

		public EditText editText_CloseDate ;

		public EditText editText_ResDate ;


		public EditText editText_AllocatedHours ;

		public EditText editText_Description ;

		public EditText editText_Action ;


		public StatusSpinnerAdapter statusList;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.IssuesEdit);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.issues_title_edit);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			LoadIssues ();

			InitControl ();

			GetProjectList ();

			GetStatusList ();

			GetPriorityList ();

			GetOwnerStaffList (IssuesDetail.ProjectId.Value);

			GetAssignToStaffList (IssuesDetail.ProjectId.Value);

			DisplayIssues (IssuesDetail);

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}
		}

		private void GetOwnerStaffList(int ProjectId){

			SearchAssignedByProject objFilter = new SearchAssignedByProject ();
			objFilter.ProjectId = ProjectId;

			OwnerStaffList = new StaffSpinnerAdapter (this,StaffHelper.GetOwnerByProject(objFilter));
			spinner_Owner.Adapter = null;

			spinner_Owner.Adapter = OwnerStaffList;

			spinner_Owner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (OwnerStaff_ItemSelected);
			if(IssuesDetail.OwnerId.HasValue)
				spinner_Owner.SetSelection(OwnerStaffList.getPositionById (IssuesDetail.OwnerId.Value));
		}

		private void OwnerStaff_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_OwnerStaffID = OwnerStaffList.GetItemAtPosition (e.Position).Id;
		}

		private void GetAssignToStaffList(int ProjectId){

			SearchAssignedByProject objFilter = new SearchAssignedByProject ();
			objFilter.ProjectId = ProjectId;

			AssignToStaffList = new StaffSpinnerAdapter (this,StaffHelper.GetAssignedToByProject(objFilter));

			spinner_AssignedTo.Adapter = null;

			spinner_AssignedTo.Adapter = AssignToStaffList;

			spinner_AssignedTo.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (AssignToStaff_ItemSelected);
			if(IssuesDetail.AssignedToId.HasValue)
				spinner_AssignedTo.SetSelection(AssignToStaffList.getPositionById (IssuesDetail.AssignedToId.Value));
		}

		private void AssignToStaff_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_AssignToStaffID = AssignToStaffList.GetItemAtPosition (e.Position).Id;
		}

		private void GetStatusList(){

			statusList = new StatusSpinnerAdapter (this,IssuesHelper.GetIssuesStatusList());

			spinner_Status.Adapter = statusList;

			spinner_Status.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Status_ItemSelected);

			if(IssuesDetail.StatusName!=null)
				spinner_Status.SetSelection(statusList.getPositionByName (IssuesDetail.StatusName));
		}

		private void Status_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_StatusID = statusList.GetItemAtPosition (e.Position).Id;
		}

		private void GetProjectList(){

			projectList = new ProjectSpinnerAdapter (this,ProjectHelper.GetProjectList());

			spinner_Project.Adapter = projectList;

			spinner_Project.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (project_ItemSelected);
			if(IssuesDetail.ProjectId.HasValue)
				spinner_Project.SetSelection(projectList.getPositionById (IssuesDetail.ProjectId.Value));

		}

		public void LoadIssues(){

			results= Intent.GetStringExtra ("Issues");

			var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<IssuesList> (results);

			IssuesDetail = IssuesHelper.GetIssuesDetail (temp.Id.Value);
		}

		public void InitControl(){
			
			editText_Title = FindViewById<EditText> (Resource.Id.editText_Title);

			spinner_Project = FindViewById<Spinner> (Resource.Id.spinner_Project);

			spinner_Status = FindViewById<Spinner> (Resource.Id.spinner_Status);

			spinner_Priority = FindViewById<Spinner> (Resource.Id.spinner_Priority);

//			spinner_Label= FindViewById<Spinner> (Resource.Id.spinner_Label);



			spinner_AssignedTo = FindViewById<Spinner> (Resource.Id.spinner_AssignedTo);

			spinner_Owner= FindViewById<Spinner> (Resource.Id.spinner_Owner);


			editText_OpenDate = FindViewById<EditText> (Resource.Id.editText_OpenDate);

			editText_CloseDate = FindViewById<EditText> (Resource.Id.editText_CloseDate);

			editText_ResDate = FindViewById<EditText> (Resource.Id.editText_ResDate);

			editText_AllocatedHours = FindViewById<EditText> (Resource.Id.editText_AllocatedHours);


			editText_Description = FindViewById<EditText> (Resource.Id.editText_Description);

			editText_Action = FindViewById<EditText> (Resource.Id.editText_Action);


			editText_OpenDate.Click += delegate { ShowDialog (Start_DATE_DIALOG_ID); };
			editText_CloseDate.Click += delegate { ShowDialog (End_DATE_DIALOG_ID); };
			editText_ResDate.Click += delegate { ShowDialog (Res_DATE_DIALOG_ID); };

			 StartDate = DateTime.Today;
			 EndDate= DateTime.Today;
			 ResDate= DateTime.Today;
		}



		public void DisplayIssues(IssuesDetailList obj){

			editText_Title.Text = obj.Title;

			editText_OpenDate.Text = obj.OpenDateString;

			editText_CloseDate.Text = obj.CloseDateString;

			editText_ResDate.Text = obj.RessolutionTargetDateString;

			editText_AllocatedHours.Text = obj.AllocatedHours.ToString();

			editText_Description.Text = HtmlRemoval.RemoveHTMLTags(HtmlRemoval.StripTagsRegexCompiled(obj.IssueDescription));

			editText_Action.Text = obj.IssueAction;

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

			if(IssuesDetail.PriorityName!=""){
				int index = PriorityAdapter.GetPosition (IssuesDetail.PriorityName);
				spinner_Priority.SetSelection(index); 
			}

			spinner_Priority.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Priority_ItemSelected);
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

		private void project_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_ProjectID = projectList.GetItemAtPosition (e.Position).Id.Value;
		}


		public void btSaveClick()
		{
			IssuesEdit IssueObject = new IssuesEdit ();
			IssueObject.Id = IssuesDetail.Id.Value;
			IssueObject.Title = editText_Title.Text;
			IssueObject.ProjectId = Selected_ProjectID;
			IssueObject.StatusId = Selected_StatusID;
			IssueObject.PriorityId = Selected_PriorityID;

			if (editText_OpenDate.Text != "")
				IssueObject.OpenDate = DateTime.Parse(editText_OpenDate.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);

			if (editText_CloseDate.Text != "")
				IssueObject.CloseDate = DateTime.Parse(editText_CloseDate.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);

			if (editText_ResDate.Text != "")
				IssueObject.RessolutionTargetDate = DateTime.Parse(editText_ResDate.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);

	
			IssueObject.AssignedToId = Selected_AssignToStaffID;
			IssueObject.OwnerId = Selected_OwnerStaffID;

			if (editText_AllocatedHours.Text != "")
				IssueObject.AllocatedHours = Double.Parse(editText_AllocatedHours.Text);

			IssueObject.Description = editText_Description.Text;

			ApiResultSave restult = IssuesHelper.EditIssue (IssueObject);
			if (restult != null) {
				if (restult.Success) {
					OnBackPressed ();
					Toast.MakeText (this, "Issue Saved", ToastLength.Short).Show ();
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
			editText_OpenDate.Text = e.Date.ToString ("dd'/'MM'/'yyyy");
		}

		// the event received when the user "sets" the date in the dialog
		void OnEndDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			editText_CloseDate.Text = e.Date.ToString ("dd'/'MM'/'yyyy");
		}

		// the event received when the user "sets" the date in the dialog
		void OnResDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			editText_ResDate.Text = e.Date.ToString ("dd'/'MM'/'yyyy");
		}


		protected override Dialog OnCreateDialog (int id)
		{
			switch (id) {
			case Start_DATE_DIALOG_ID:
				return new DatePickerDialog (this, OnStartDateSet, StartDate.Year, StartDate.Month - 1, StartDate.Day); 
			case End_DATE_DIALOG_ID:
				return new DatePickerDialog (this, OnEndDateSet, EndDate.Year, EndDate.Month - 1, EndDate.Day); 

			case Res_DATE_DIALOG_ID:
				return new DatePickerDialog (this, OnResDateSet, ResDate.Year, ResDate.Month - 1, ResDate.Day); 
			}
			return null;
		}
	}
}

