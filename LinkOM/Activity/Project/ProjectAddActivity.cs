
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
	[Activity (Label = "ProjectAddActivity", Theme = "@style/Theme.Customtheme")]		
	public class ProjectAddActivity : Activity, TextView.IOnEditorActionListener
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

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.ProjectEdit);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.project_add_title);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);
			// Create your application here

			InitControl ();

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
					Toast.MakeText (this, "Saved", ToastLength.Short).Show ();
					OnBackPressed ();
					break;
				default:
					break;
			}

			return true;
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

