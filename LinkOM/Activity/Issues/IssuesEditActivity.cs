
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
		const int Actual_Start_DATE_DIALOG_ID = 2;
		const int Actual_End_DATE_DIALOG_ID = 3;


		public IssuesObject IssuesDetail;

		public ProjectSpinnerAdapter projectList; 
		public ArrayAdapter PriorityAdapter;

		public DateTime StartDate;
		public DateTime EndDate;
		public DateTime ActualStartDate;
		public DateTime ActualEndDate;


		public int Selected_ProjectID;
		public int Selected_PriorityID;
		public int Selected_StatusID;
		public int Selected_PhaseID;


		public EditText editText_Title;

		public Spinner spinner_Project ;

		public CheckBox cb_Internal ;

		public CheckBox cb_WatchList ;

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

//			GetStaffList ();
//
//			GetPhaseList ();

//			GetLabel ();

			DisplayIssues (IssuesDetail);

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}
		}

		public void LoadIssues(){

			results= Intent.GetStringExtra ("Issues");

			IssuesDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<IssuesObject> (results);
		}

		public void InitControl(){
			
			editText_Title = FindViewById<EditText> (Resource.Id.editText_Title);

			spinner_Project = FindViewById<Spinner> (Resource.Id.spinner_Project);

			cb_Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);

			cb_WatchList = FindViewById<CheckBox> (Resource.Id.cb_WatchList);

			cb_Management = FindViewById<CheckBox> (Resource.Id.cb_Management);

			spinner_Status = FindViewById<Spinner> (Resource.Id.spinner_Status);

			spinner_Priority = FindViewById<Spinner> (Resource.Id.spinner_Priority);

			spinner_AssignedTo = FindViewById<Spinner> (Resource.Id.spinner_AssignedTo);

			spinner_Owner= FindViewById<Spinner> (Resource.Id.spinner_Owner);

			editText_StartDate = FindViewById<EditText> (Resource.Id.editText_StartDate);

			editText_EndDate = FindViewById<EditText> (Resource.Id.editText_EndDate);

			editText_AllocatedHours = FindViewById<EditText> (Resource.Id.editText_AllocatedHours);

			editText_Description = FindViewById<EditText> (Resource.Id.editText_Description);

			spinner_Phase = FindViewById<Spinner> (Resource.Id.spinner_Phase);

			spinner_Label= FindViewById<Spinner> (Resource.Id.spinner_Label);

			editText_StartDate.Click += delegate { ShowDialog (Start_DATE_DIALOG_ID); };
			editText_EndDate.Click += delegate { ShowDialog (End_DATE_DIALOG_ID); };

		}



		public void DisplayIssues(IssuesObject obj){

			editText_Title.Text = obj.Title;


//			cb_Internal.Checked = obj.IsInternal;


//			if(obj.IsAddToMyWatch.HasValue)
//				cb_WatchList.Checked = obj.IsAddToMyWatch.Value;

//			cb_Management.Checked = obj.IsManagement;
			

//			editText_StartDate.Text = obj.StartDateString;
//
//			editText_EndDate.Text = obj.EndDateString;
//
//			editText_ActualStartDate.Text = obj.ActualStartDateString;
//
//			editText_ActualEndDate.Text = obj.ActualEndDateString;

			editText_AllocatedHours.Text = obj.AllocatedHours.ToString();

			editText_Description.Text = obj.Description;

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
					OnBackPressed ();
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

			if(IssuesDetail.PriorityName!=""){
				int index = PriorityAdapter.GetPosition (IssuesDetail.PriorityName);
				spinner_Priority.SetSelection(index); 
			}

			spinner_Priority.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Priority_ItemSelected);
		}

		private void GetStatusList(){

			var StatusAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.TaskStatus, Android.Resource.Layout.SimpleSpinnerItem);
			StatusAdapter.SetDropDownViewResource (Android.Resource.Layout.SelectDialogSingleChoice);
			spinner_Status.Adapter = StatusAdapter;

//			if(IssuesDetail.IssuesStatusName!=""){
//				int index = StatusAdapter.GetPosition (IssuesDetail.IssuesStatusName);
//				spinner_Status.SetSelection(index); 
//			}

			spinner_Status.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Status_ItemSelected);
		}


		private void GetProjectList(){
			//Handle Project Spinner
			string TokenNumber = Settings.Token;
			string url = Settings.InstanceURL;

			url=url+"/api/ProjectList";


			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "P.Name", Direction = "1"},
				new objSort{ColumnName = "C.Name", Direction = "2"}
			};

			var objProject = new
			{
				Name = string.Empty,
				ClientName = string.Empty,
				DepartmentId = string.Empty,
				ProjectStatusId = string.Empty,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = TokenNumber,
						PageSize = 20,
						PageNumber = 1,
						Sort = objSort,
						Item = objProject
					}
				});

			string results= ConnectWebAPI.Request(url,objsearch);

			ProjectListJson ProjectList = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectListJson> (results);

			projectList = new ProjectSpinnerAdapter (this,ProjectList.Items);


			spinner_Project.Adapter = projectList;

			if(IssuesDetail.ProjectId!=null)
				spinner_Project.SetSelection(projectList.getPositionById(IssuesDetail.ProjectId)); 

			spinner_Project.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (project_ItemSelected);
		}

		private void Status_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;
			string StatusName = spinner.GetItemAtPosition (e.Position).ToString();
			if(StatusName.Equals("Open")){
				Selected_StatusID=1;
			}
			else if(StatusName.Equals("Closed")){
				Selected_StatusID=2;
			}
			else if(StatusName.Equals("Waiting On Client")){
				Selected_StatusID=3;
			}
			else if(StatusName.Equals("In Progress")){
				Selected_StatusID=4;
			}
			else if(StatusName.Equals("On Hold")){
				Selected_StatusID=5;
			}
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
			Spinner spinner = (Spinner)sender;
			string PhaseName = spinner.GetItemAtPosition (e.Position).ToString();
			if(PhaseName.Equals("Low")){
				Selected_PhaseID=1;
			}
		}

		private void project_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_ProjectID = projectList.GetItemAtPosition (e.Position).Id;
		}


		public void btSaveClick(object sender, EventArgs e)
		{
			string TokenNumber = Settings.Token;
			string url = Settings.InstanceURL;

			url=url+"/api/EditIssues";

			var objItem = new
			{
				Id = IssuesDetail.Id,
				Guid = IssuesDetail.Guid,
//				AssignedToId= IssuesDetail.AssignedToId,
				AssignToName= String.Empty,
				Title= editText_Title.Text,
				ProjectId= Selected_ProjectID,
				OwnerId= IssuesDetail.OwnerId,
				StartDate= editText_StartDate.Text,
				EndDate= editText_EndDate.Text,
				PriorityId= Selected_PriorityID,
				IssuesStatusId= Selected_StatusID,
				AllocatedHours= editText_AllocatedHours.Text,
				IsUserWatch = cb_WatchList.Checked,
				UpdatedBy = IssuesDetail.CreatedBy,
			};

			var objEditIssues = (new
				{
					objIssues = new
					{
						UserId = Settings.UserId,
						UserName = Settings.Username,
						Item = objItem
					}
				});

			string results= ConnectWebAPI.Request(url,objEditIssues);

			if (results != null) {
				ResultsJson ResultsJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultsJson> (results);

				if (ResultsJson.Success) {
					Toast.MakeText (this, "Issues Saved", ToastLength.Short).Show ();
				} else
					Toast.MakeText (this, ResultsJson.ErrorMessage, ToastLength.Short).Show ();
			}
			else
				Toast.MakeText (this, "Error to connect to server", ToastLength.Short).Show ();

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

