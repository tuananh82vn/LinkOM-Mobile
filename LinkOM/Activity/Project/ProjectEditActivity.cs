
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
		public ProjectObject ProjectDetail;
		public InputMethodManager inputMethodManager;

		public EditText AllocatedHours;

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

			if (StartDatePicker.Text != "")
				StartDate = DateTime.Parse (StartDatePicker.Text);
			else
				StartDate = DateTime.Today;

			if (EndDatePicker.Text != "")
				EndDate = DateTime.Parse (EndDatePicker.Text);
			else
				EndDate = DateTime.Today;

			if (ActualStartDatePicker.Text != "")
				ActualStartDate = DateTime.Parse (ActualStartDatePicker.Text);
			else
				ActualStartDate = DateTime.Today;

			if (ActualEndDatePicker.Text != "")
				ActualEndDate = DateTime.Parse (ActualEndDatePicker.Text);
			else
				ActualEndDate = DateTime.Today;

			Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
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
			StartDatePicker.Text = e.Date.ToString ("d");
		}

		// the event received when the user "sets" the date in the dialog
		void OnActualStartDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			ActualStartDatePicker.Text = e.Date.ToString ("d");
		}

		// the event received when the user "sets" the date in the dialog
		void OnEndDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			EndDatePicker.Text = e.Date.ToString ("d");
		}

		// the event received when the user "sets" the date in the dialog
		void OnActualEndDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			ActualEndDatePicker.Text = e.Date.ToString ("d");
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

			ProjectDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectObject> (results);
		}

		public void DisplayProject(ProjectObject obj){

			var ProjectName = FindViewById<EditText> (Resource.Id.editText_ProjectName);
			ProjectName.Text = obj.Name;

			var RefCode = FindViewById<EditText> (Resource.Id.editText_RefCode);
			RefCode.Text = obj.ReferenceCode;

//			var Active = FindViewById<CheckBox> (Resource.Id.checkBox_Active);
//			Active.Checked = obj.Active;

//			var ProjectPhase = FindViewById<TextView> (Resource.Id.tv_Phase);
//			ProjectPhase.Text = obj.ProjectPhase;
//
//			var ProjectStatus = FindViewById<TextView> (Resource.Id.tv_Status);
//			ProjectStatus.Text = obj.ProjectStatus;

			AllocatedHours = FindViewById<EditText> (Resource.Id.editText_AllocatedHours);
			AllocatedHours.SetOnEditorActionListener (this);
			if (obj.AllocatedHours != null)
				AllocatedHours.Text = obj.AllocatedHours.Value.ToString();

			if(obj.StartDate!=null)
				StartDatePicker.Text = obj.StartDate.Value.ToShortDateString();

			if(obj.EndDate!=null)
				EndDatePicker.Text = obj.EndDate.Value.ToShortDateString();

//			var ActualStartDate = FindViewById<TextView> (Resource.Id.tv_ActualStartDate);
//			if(obj.ActualStartDate!=null)
//				ActualStartDate.Text = obj.ActualStartDate.Value.ToShortDateString();
//
//			var ActualEndDate = FindViewById<TextView> (Resource.Id.tv_ActualEndDate);
//			if(obj.ActualEndDate!=null)
//				ActualEndDate.Text = obj.ActualEndDate.Value.ToShortDateString();

//			var ClientName = FindViewById<TextView> (Resource.Id.tv_Client);
//			ClientName.Text = obj.ClientName;
//
//			var DeliveryManager = FindViewById<TextView> (Resource.Id.tv_DeliveryManager);
//			DeliveryManager.Text = obj.DeliveryManagerName;

//			var ProjectManager = FindViewById<TextView> (Resource.Id.tv_ProjectManager);
//			ProjectManager.Text = obj.ProjectManagerName;
//
//			var ProjectCoordinator = FindViewById<TextView> (Resource.Id.tv_ProjectCoordinator);
//			ProjectCoordinator.Text = obj.ProjectCoordinatorName;

			var Notes = FindViewById<EditText> (Resource.Id.editText_Notes);
			if(obj.Notes!=null)
				Notes.Text = obj.Notes;

			var Description = FindViewById<EditText> (Resource.Id.editText_Description);
			if(obj.Description!=null)
				Description.Text = obj.Description;

//			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
//			DepartmentName.Text = obj.DepartmentName;

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

