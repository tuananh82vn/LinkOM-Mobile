
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
using Android.Views.InputMethods;
using Android.Content.PM;

namespace LinkOM
{
	[Activity (Label = "ProjectEditActivity", Theme = "@style/Theme.Customtheme")]		
	public class ProjectEditActivity : Activity, TextView.IOnEditorActionListener
	{
		const int Start_DATE_DIALOG_ID = 0;
		const int End_DATE_DIALOG_ID = 1;
		const int Actual_Start_DATE_DIALOG_ID = 2;
		const int Actual_End_DATE_DIALOG_ID = 3;

		private DateTime StartDate;
		private DateTime EndDate;

		private DateTime ActualStartDate;
		private DateTime ActualEndDate;

		private EditText StartDatePicker;
		private EditText EndDatePicker;

		private EditText ActualStartDatePicker;
		private EditText ActualEndDatePicker;

		public long ProjectId;
		public ProjectDetailList ProjectDetail;
		public InputMethodManager inputMethodManager;

		public EditText AllocatedHours;

		public Spinner spinner_Phase ;
		public Spinner spinner_Status ;

		public ProjectSpinnerAdapter projectList; 
		public ProjectPhaseSpinnerAdapter phaseList; 
		public ProjectLabelSpinnerAdapter labelList; 
		public StatusSpinnerAdapter statusList;

		public int Selected_ProjectID;
		public int Selected_AssignToStaffID;
		public int Selected_OwnerStaffID;
		public int Selected_PriorityID;
		public int Selected_StatusID;
		public int Selected_PhaseID;
		public string Selected_Label;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.ProjectEdit);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.project_edit_title);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);
			// Create your application here

			LoadProject ();

			InitControl ();

			DisplayProject (ProjectDetail);

			GetPhaseList ();

			GetStatusList ();

			Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

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

		public void InitControl(){

			StartDatePicker = FindViewById<EditText> (Resource.Id.editText_StartDate);
			StartDatePicker.InputType =0;
			StartDatePicker.Click += delegate { ShowDialog (Start_DATE_DIALOG_ID); };

			ActualStartDatePicker = FindViewById<EditText> (Resource.Id.editText_ActualStartDate);
			ActualStartDatePicker.InputType =0;
			ActualStartDatePicker.Click += delegate { ShowDialog (Actual_Start_DATE_DIALOG_ID); };

			EndDatePicker = FindViewById<EditText> (Resource.Id.editText_EndDate);
			EndDatePicker.InputType =0;
			EndDatePicker.Click += delegate { ShowDialog (End_DATE_DIALOG_ID); };

			ActualEndDatePicker = FindViewById<EditText> (Resource.Id.editText_ActualEndDate);
			ActualEndDatePicker.InputType =0;
			ActualEndDatePicker.Click += delegate { ShowDialog (Actual_End_DATE_DIALOG_ID); };

			spinner_Phase = FindViewById<Spinner> (Resource.Id.spinner_Phase);

			spinner_Status = FindViewById<Spinner> (Resource.Id.spinner_Status);
		}

		public void GetPhaseList (){
			
			phaseList = new ProjectPhaseSpinnerAdapter (this, PhaseHelper.GetProjectPhaseByProject (ProjectDetail.ProjectId.Value));
			spinner_Phase.Adapter = phaseList;

			spinner_Phase.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Phase_ItemSelected);

		}

		private void GetStatusList(){

			statusList = new StatusSpinnerAdapter (this,ProjectHelper.GetProjectStatus());

			spinner_Status.Adapter = statusList;

			spinner_Status.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (status_ItemSelected);

			if(ProjectDetail.ProjectStatus!=null)
				spinner_Status.SetSelection(statusList.getPositionByName (ProjectDetail.ProjectStatus));
		}

		private void status_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_StatusID = statusList.GetItemAtPosition (e.Position).Id;
		}

		private void Phase_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_PhaseID = phaseList.GetItemAtPosition (e.Position).Id;
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			inflater.Inflate (Resource.Menu.SaveMenu, menu);

			return true;
		}

		// the event received when the user "sets" the date in the dialog
		void OnStartDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			StartDatePicker.Text = e.Date.ToString ("dd'/'MM'/'yyyy");
		}

		// the event received when the user "sets" the date in the dialog
		void OnActualStartDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			ActualStartDatePicker.Text = e.Date.ToString ("dd'/'MM'/'yyyy");
		}

		// the event received when the user "sets" the date in the dialog
		void OnEndDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			EndDatePicker.Text = e.Date.ToString ("dd'/'MM'/'yyyy");
		}

		// the event received when the user "sets" the date in the dialog
		void OnActualEndDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			ActualEndDatePicker.Text = e.Date.ToString ("dd'/'MM'/'yyyy");
		}

		protected override Dialog OnCreateDialog (int id)
		{
			switch (id) {
			case Start_DATE_DIALOG_ID:
				{
					return new DatePickerDialog (this, OnStartDateSet, StartDate.Year, StartDate.Month - 1, StartDate.Day); 
				}
			case End_DATE_DIALOG_ID:
				{
					return new DatePickerDialog (this, OnEndDateSet, EndDate.Year, EndDate.Month - 1, EndDate.Day); 
				}
			case Actual_Start_DATE_DIALOG_ID:
				{
					return new DatePickerDialog (this, OnActualStartDateSet, ActualStartDate.Year, ActualStartDate.Month - 1, ActualStartDate.Day); 
				}
			case Actual_End_DATE_DIALOG_ID:
				{
					return new DatePickerDialog (this, OnActualEndDateSet, ActualEndDate.Year, ActualEndDate.Month - 1, ActualEndDate.Day); 
				}
			}
			return null;
		}

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

		public void LoadProject(){

			string results = Intent.GetStringExtra ("Project");

			ProjectDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectDetailList> (results);
		}

		public void DisplayProject(ProjectDetailList obj){

			var ProjectName = FindViewById<EditText> (Resource.Id.editText_ProjectName);
			ProjectName.Text = obj.ProjectName;

			var RefCode = FindViewById<EditText> (Resource.Id.editText_RefCode);
			RefCode.Text = obj.ReferenceCode;

//			var Active = FindViewById<CheckBox> (Resource.Id.checkBox_Active);
//			Active.Checked = obj.;

			AllocatedHours = FindViewById<EditText> (Resource.Id.editText_AllocatedHours);
			AllocatedHours.SetOnEditorActionListener (this);
			if (obj.AllocatedHours != null)
				AllocatedHours.Text = obj.AllocatedHours.Value.ToString();

			if(obj.StartDate!=null)
				StartDatePicker.Text = obj.StartDate.Value.ToString ("dd'/'MM'/'yyyy");

			if(obj.EndDate!=null)
				EndDatePicker.Text = obj.EndDate.Value.ToString ("dd'/'MM'/'yyyy");

			var ActualStartDate_EditText = FindViewById<EditText> (Resource.Id.editText_ActualStartDate);
			if(obj.ActualStartDate!=null)
				ActualStartDate_EditText.Text = obj.ActualStartDate.Value.ToString ("dd'/'MM'/'yyyy");

			var ActualEndDate_EditText = FindViewById<EditText> (Resource.Id.editText_ActualEndDate);
			if(obj.ActualEndDate!=null)
				ActualEndDate_EditText.Text = obj.ActualEndDate.Value.ToString ("dd'/'MM'/'yyyy");

			var Notes = FindViewById<EditText> (Resource.Id.editText_Notes);
			if(obj.Notes!=null)
				Notes.Text = obj.Notes;

			var Description = FindViewById<EditText> (Resource.Id.editText_Description);
			if(obj.Description!=null)
				Description.Text = obj.Description;


			if (StartDatePicker.Text != "")
				StartDate = DateTime.Parse (StartDatePicker.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);
			else
				StartDate = DateTime.Today;

			if (EndDatePicker.Text != "")
				EndDate = DateTime.Parse (EndDatePicker.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);
			else
				EndDate = DateTime.Today;

			if (ActualStartDatePicker.Text != "")
				ActualStartDate = DateTime.Parse (ActualStartDatePicker.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);
			else
				ActualStartDate = DateTime.Today;

			if (ActualEndDatePicker.Text != "")
				ActualEndDate = DateTime.Parse (ActualEndDatePicker.Text,System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat);
			else
				ActualEndDate = DateTime.Today;
			

		}
		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			if (actionId == ImeAction.Done) {
				AllocatedHours.ClearFocus ();
				var inputManager = (InputMethodManager)GetSystemService(InputMethodService);
				inputManager.HideSoftInputFromWindow(AllocatedHours.WindowToken, HideSoftInputFlags.None);
			} 
			return false;
		}
	}
}

