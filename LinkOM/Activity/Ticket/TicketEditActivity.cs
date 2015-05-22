
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
	[Activity (Label = "TicketEditActivity", Theme = "@style/Theme.Customtheme")]			
	public class TicketEditActivity : Activity
	{
		public string results;

		const int Start_DATE_DIALOG_ID = 0;
		const int End_DATE_DIALOG_ID = 1;
		const int Actual_Start_DATE_DIALOG_ID = 2;
		const int Actual_End_DATE_DIALOG_ID = 3;


		public TicketDetailList TicketDetail;

		public ProjectSpinnerAdapter projectList; 
		public ProjectLabelSpinnerAdapter labelList;
		public ArrayAdapter PriorityAdapter;
		public StatusSpinnerAdapter statusList;
		public StatusSpinnerAdapter typeList;
		public StatusSpinnerAdapter methodList;

		public StaffSpinnerAdapter OwnerStaffList; 
		public StaffSpinnerAdapter AssignToStaffList;

		public DateTime StartDate;
		public DateTime EndDate;
		public DateTime ActualStartDate;
		public DateTime ActualEndDate;


		public int Selected_ProjectID;
		public int Selected_PriorityID;
		public int Selected_StatusID;
		public int Selected_PhaseID;
		public int Selected_AssignToStaffID;
		public int Selected_OwnerStaffID;

		public string Selected_Label;
		public int Selected_TypeID;
		public int Selected_MethodID;


		public EditText editText_Title;

		public Spinner spinner_Project ;

		public CheckBox cb_Internal ;

		public CheckBox cb_WatchList ;

		public CheckBox cb_Management ;

		public Spinner spinner_Status ;

		public Spinner spinner_Priority ;

		public Spinner spinner_AssignedTo ;

		public Spinner spinner_Owner;

		public Spinner spinner_Type;

		public Spinner spinner_Method;

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

			SetContentView (Resource.Layout.TicketEdit);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.ticket_title_edit);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			LoadTicket ();

			InitControl ();

			GetProjectList ();

			GetStatusList ();

			GetPriorityList ();

			GetOwnerStaffList (TicketDetail.ProjectId.Value);

			GetAssignToStaffList (TicketDetail.ProjectId.Value);

			GetLabelList (TicketDetail.ProjectId.Value);

			GetTypeList();

			GetReceivedMethodList();

			DisplayTicket (TicketDetail);

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}
		}

		public void LoadTicket(){

			results= Intent.GetStringExtra ("Ticket");

			var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<TicketList> (results);

			TicketDetail = TicketHelper.GetTicketDetail (temp.Id.Value);
		}

		public void InitControl(){
			
			editText_Title = FindViewById<EditText> (Resource.Id.editText_Title);

			spinner_Project = FindViewById<Spinner> (Resource.Id.spinner_Project);

			cb_Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);

			spinner_Status = FindViewById<Spinner> (Resource.Id.spinner_Status);

			spinner_Priority = FindViewById<Spinner> (Resource.Id.spinner_Priority);

			spinner_AssignedTo = FindViewById<Spinner> (Resource.Id.spinner_AssignedTo);

			spinner_Owner= FindViewById<Spinner> (Resource.Id.spinner_Owner);

			spinner_Type= FindViewById<Spinner> (Resource.Id.spinner_Type);

			spinner_Method= FindViewById<Spinner> (Resource.Id.spinner_Method);

			editText_StartDate = FindViewById<EditText> (Resource.Id.editText_StartDate);

			editText_EndDate = FindViewById<EditText> (Resource.Id.editText_EndDate);

			editText_ActualStartDate = FindViewById<EditText> (Resource.Id.editText_ActualStartDate);

			editText_ActualEndDate = FindViewById<EditText> (Resource.Id.editText_ActualEndDate);

			editText_AllocatedHours = FindViewById<EditText> (Resource.Id.editText_AllocatedHours);

			editText_Description = FindViewById<EditText> (Resource.Id.editText_Description);

			spinner_Phase = FindViewById<Spinner> (Resource.Id.spinner_Phase);

			spinner_Label= FindViewById<Spinner> (Resource.Id.spinner_Label);

			editText_StartDate.Click += delegate { ShowDialog (Start_DATE_DIALOG_ID); };
			editText_EndDate.Click += delegate { ShowDialog (End_DATE_DIALOG_ID); };
			editText_ActualStartDate.Click += delegate { ShowDialog (Actual_Start_DATE_DIALOG_ID); };
			editText_ActualEndDate.Click += delegate { ShowDialog (Actual_End_DATE_DIALOG_ID); };

			StartDate = DateTime.Today;
			EndDate= DateTime.Today;
			ActualStartDate= DateTime.Today;
			ActualEndDate= DateTime.Today;

		}

		private void GetLabelList(int ProjectId){

			labelList = new ProjectLabelSpinnerAdapter (this,LabelHelper.GetProjectLabelByProject(ProjectId));

			spinner_Label.Adapter = labelList;

			spinner_Label.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Label_ItemSelected);

			if(TicketDetail.Label!=null)
				spinner_Label.SetSelection(labelList.getPositionByName(TicketDetail.Label));

		}

		private void Label_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_Label = labelList.GetItemAtPosition (e.Position).Name;
		}

		public void DisplayTicket(TicketDetailList obj){

			editText_Title.Text = obj.Title;

			if (obj.IsInternal.HasValue)
				cb_Internal.Checked = obj.IsInternal.Value;
			else
				cb_Internal.Checked = false;

			editText_StartDate.Text = obj.StartDateString;

			editText_EndDate.Text = obj.EndDateString;

			editText_ActualStartDate.Text = obj.ActualStartDateString;

			editText_ActualEndDate.Text = obj.ActualEndDateString;

			editText_AllocatedHours.Text = obj.AllocatedHours.ToString();

			editText_Description.Text = obj.TicketDiscription;

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

		public void btSaveClick()
		{

			TicketEdit TicketObject = new TicketEdit ();
			TicketObject.Id = TicketDetail.Id.Value;
			TicketObject.Title = editText_Title.Text;
			TicketObject.ProjectId = Selected_ProjectID;
			TicketObject.TicketStatusId = Selected_StatusID;
			TicketObject.PriorityId = Selected_PriorityID;
			TicketObject.Label = Selected_Label;
			TicketObject.TicketReceivedMethodId = Selected_MethodID;
			TicketObject.TicketTypeId= Selected_TypeID;
			if (editText_StartDate.Text != "")
				TicketObject.StartDate = DateTime.Parse(editText_StartDate.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);

			if (editText_EndDate.Text != "")
				TicketObject.EndDate = DateTime.Parse(editText_EndDate.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);

			if (editText_ActualStartDate.Text != "")
				TicketObject.ActualStartDate = DateTime.Parse(editText_ActualStartDate.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);

			if (editText_ActualEndDate.Text != "")
				TicketObject.ActualEndDate = DateTime.Parse(editText_ActualEndDate.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);

			TicketObject.IsInternal = cb_Internal.Checked;
			TicketObject.AssignedToId = Selected_AssignToStaffID;
			TicketObject.OwnerId = Selected_OwnerStaffID;
			TicketObject.UserName = Settings.Username;

			if (editText_AllocatedHours.Text != "")
				TicketObject.AllocatedHours = Int32.Parse(editText_AllocatedHours.Text);


			TicketObject.Description = editText_Description.Text;

			ApiResultSave restult = TicketHelper.EditTicket (TicketObject);
			if (restult != null) {
				if (restult.Success) {
					OnBackPressed ();
					Toast.MakeText (this, "Ticket Saved", ToastLength.Short).Show ();
				}
				else
					Toast.MakeText (this, restult.ErrorMessage, ToastLength.Short).Show ();
			}
			else
				Toast.MakeText (this, "Server problem...", ToastLength.Short).Show ();
		}

		private void GetOwnerStaffList(int ProjectId){

			SearchAssignedByProject objFilter = new SearchAssignedByProject ();
			objFilter.ProjectId = ProjectId;

			OwnerStaffList = new StaffSpinnerAdapter (this,StaffHelper.GetOwnerByProject(objFilter));
			spinner_Owner.Adapter = null;

			spinner_Owner.Adapter = OwnerStaffList;

			spinner_Owner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (OwnerStaff_ItemSelected);
			if(TicketDetail.OwnerId.HasValue)
				spinner_Owner.SetSelection(OwnerStaffList.getPositionById (TicketDetail.OwnerId.Value));
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
			if(TicketDetail.AssignedToId.HasValue)
				spinner_AssignedTo.SetSelection(AssignToStaffList.getPositionById (TicketDetail.AssignedToId.Value));
		}

		private void AssignToStaff_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_AssignToStaffID = AssignToStaffList.GetItemAtPosition (e.Position).Id;
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

			spinner_Priority.Adapter = PriorityAdapter;

			if(TicketDetail.PriorityName!=""){
				int index = PriorityAdapter.GetPosition (TicketDetail.PriorityName);
				spinner_Priority.SetSelection(index); 
			}

			spinner_Priority.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Priority_ItemSelected);
		}

		private void GetStatusList(){

			statusList = new StatusSpinnerAdapter (this,TicketHelper.GetTicketStatusList());

			spinner_Status.Adapter = statusList;

			spinner_Status.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Status_ItemSelected);

			if(TicketDetail.StatusName!=null)
				spinner_Status.SetSelection(statusList.getPositionByName (TicketDetail.StatusName));
		}

		private void GetTypeList(){

			typeList = new StatusSpinnerAdapter (this,TicketHelper.GetTicketTypeList());

			spinner_Type.Adapter = typeList;

			spinner_Type.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Type_ItemSelected);

			if(TicketDetail.TicketTypeName!=null)
				spinner_Type.SetSelection(typeList.getPositionByName (TicketDetail.TicketTypeName));
		}

		private void GetReceivedMethodList(){

			methodList = new StatusSpinnerAdapter (this,TicketHelper.GetReceivedMethodList());

			spinner_Method.Adapter = methodList;

			spinner_Method.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Method_ItemSelected);

			if(TicketDetail.TicketReceivedMethod!=null)
				spinner_Method.SetSelection(methodList.getPositionByName (TicketDetail.TicketReceivedMethod));
		}

		private void Type_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_TypeID = typeList.GetItemAtPosition (e.Position).Id;
		}

		private void Method_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_MethodID = methodList.GetItemAtPosition (e.Position).Id;
		}

		private void GetProjectList(){

			projectList = new ProjectSpinnerAdapter (this,ProjectHelper.GetProjectList());

			spinner_Project.Adapter = projectList;

			spinner_Project.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (project_ItemSelected);
			if(TicketDetail.ProjectId.HasValue)
				spinner_Project.SetSelection(projectList.getPositionById (TicketDetail.ProjectId.Value));

		}

		private void Status_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_StatusID = statusList.GetItemAtPosition (e.Position).Id;
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

