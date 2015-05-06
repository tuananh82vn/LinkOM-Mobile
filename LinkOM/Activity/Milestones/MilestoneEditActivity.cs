
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
using Compass.FilePicker;
using Android.Content.PM;

namespace LinkOM
{
	[Activity (Label = "MilestoneEditActivity", Theme = "@style/Theme.Customtheme")]			
	public class MilestoneEditActivity : Activity
	{
		public string results;

		public MilestoneObject MilestoneDetail;

		public ProjectSpinnerAdapter projectList; 

		public ArrayAdapter PriorityAdapter;

		public EditText editText_Title;

		public Spinner spinner_Category ;

		public CheckBox cb_Internal ;

		public Spinner spinner_Label ;

		public Spinner spinner_Project ;

		public EditText editText_Description ;

		private FilePickerFragment filePickerFragment;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.MilestoneEdit);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.document_title_edit);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			var buttonFileDialog = FindViewById<Button>(Resource.Id.btnFileDialog);


			LoadMilestone ();

			InitControl ();

			GetProjectList ();

//			GetStatusList ();
//
//			GetPriorityList ();

//			GetStaffList ();
//
//			GetPhaseList ();

//			GetLabel ();

			DisplayMilestone (MilestoneDetail);

//			buttonFileDialog.Click += delegate
//			{
//				filePickerFragment = new FilePickerFragment();
//				filePickerFragment.FileSelected += (sender, path) =>
//				{
//					filePickerFragment.Dismiss();
//					UpdateSelectedText(path);
//				};
//				filePickerFragment.Cancel += sender => filePickerFragment.Dismiss();
//				filePickerFragment.Show(FragmentManager, "FilePicker");
//			};

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}
		}

		public void LoadMilestone(){

			results= Intent.GetStringExtra ("Milestone");

			MilestoneDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<MilestoneObject> (results);
		}

		public void InitControl(){
			
			editText_Title = FindViewById<EditText> (Resource.Id.editText_Title);

			spinner_Category = FindViewById<Spinner> (Resource.Id.spinner_Category);

			cb_Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);

			spinner_Project = FindViewById<Spinner> (Resource.Id.spinner_Project);

			spinner_Label = FindViewById<Spinner> (Resource.Id.spinner_Label);

			editText_Description = FindViewById<EditText> (Resource.Id.editText_Description);

		}

		private void UpdateSelectedText(string text)
		{
			var textSelectedDirectory = FindViewById<TextView>(Resource.Id.tv_filePath);
			if (textSelectedDirectory != null)
			{
				textSelectedDirectory.Text = text;
			}
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			switch (requestCode)
			{
			case FilePickerActivity.ResultCodeDirSelected:
				switch (resultCode)
				{
				case Result.Canceled:
					break;
				case Result.FirstUser:
					break;
				case Result.Ok:
					UpdateSelectedText(data.GetStringExtra(FilePickerActivity.ResultSelectedDir));
					break;
				default:
					throw new ArgumentOutOfRangeException("resultCode");
				}
				break;
			}
		}

		public void DisplayMilestone(MilestoneObject obj){

			editText_Title.Text = obj.Title;

//			editText_Description.Text = obj.Description;

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

//		private void GetPriorityList(){
//			//Handle priority
//
//			PriorityAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.MilestonePriority, Android.Resource.Layout.SimpleSpinnerItem);
//			PriorityAdapter.SetDropDownViewResource (Android.Resource.Layout.SelectDialogSingleChoice);
//			spinner_Priority.Adapter = PriorityAdapter;
//
//			if(MilestoneDetail.PriorityName!=""){
//				int index = PriorityAdapter.GetPosition (MilestoneDetail.PriorityName);
//				spinner_Priority.SetSelection(index); 
//			}
//
//			spinner_Priority.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Priority_ItemSelected);
//		}

//		private void GetStatusList(){
//
//			var StatusAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.MilestoneStatus, Android.Resource.Layout.SimpleSpinnerItem);
//			StatusAdapter.SetDropDownViewResource (Android.Resource.Layout.SelectDialogSingleChoice);
//			spinner_Status.Adapter = StatusAdapter;
//
//			if(MilestoneDetail.StatusName!=""){
//				int index = StatusAdapter.GetPosition (MilestoneDetail.StatusName);
//				spinner_Status.SetSelection(index); 
//			}
//
//			spinner_Status.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Status_ItemSelected);
//		}

//		private void GetPhaseList(){
//
//			var PhaseAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.Phase, Android.Resource.Layout.SimpleSpinnerItem);
//			PhaseAdapter.SetDropDownViewResource (Android.Resource.Layout.SelectDialogSingleChoice);
//			spinner_Phase.Adapter = PhaseAdapter;
//
//			if(MilestoneDetail.ProjectPhaseName!=""){
//				int index = PhaseAdapter.GetPosition (MilestoneDetail.ProjectPhaseName);
//				spinner_Phase.SetSelection(index); 
//			}
//
//			spinner_Phase.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Phase_ItemSelected);
//		}

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

			spinner_Project.SetSelection(projectList.getPositionById(MilestoneDetail.ProjectId)); 

			//spinner_Project.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (project_ItemSelected);
		}

//		private void Status_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
//		{
//			Spinner spinner = (Spinner)sender;
//			string StatusName = spinner.GetItemAtPosition (e.Position).ToString();
//			if(StatusName.Equals("Open")){
//				Selected_StatusID=1;
//			}
//			else if(StatusName.Equals("Closed")){
//				Selected_StatusID=2;
//			}
//			else if(StatusName.Equals("Waiting On Client")){
//				Selected_StatusID=3;
//			}
//			else if(StatusName.Equals("In Progress")){
//				Selected_StatusID=4;
//			}
//			else if(StatusName.Equals("On Hold")){
//				Selected_StatusID=5;
//			}
//		}

//		private void Priority_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
//		{
//			Spinner spinner = (Spinner)sender;
//			string PriorityName = spinner.GetItemAtPosition (e.Position).ToString();
//			if(PriorityName.Equals("Low")){
//				Selected_PriorityID=1;
//			}
//			else if(PriorityName.Equals("Medium")){
//					Selected_PriorityID=2;
//				}
//			else if(PriorityName.Equals("High")){
//						Selected_PriorityID=3;
//					}
//		}

//		private void Phase_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
//		{
//			Spinner spinner = (Spinner)sender;
//			string PhaseName = spinner.GetItemAtPosition (e.Position).ToString();
//			if(PhaseName.Equals("Low")){
//				Selected_PhaseID=1;
//			}
//		}

//		private void project_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
//		{
//			Selected_ProjectID = projectList.GetItemAtPosition (e.Position).Id;
//		}


//		public void btSaveClick(object sender, EventArgs e)
//		{
//			string TokenNumber = Settings.Token;
//			string url = Settings.InstanceURL;
//
//			url=url+"/api/EditMilestone";
//
//			var objItem = new
//			{
//				Id = MilestoneDetail.Id,
//				Guid = MilestoneDetail.Guid,
//				AssignToName= String.Empty,
//				Title= editText_Title.Text,
//				ProjectId= Selected_ProjectID,
//				StartDate= editText_StartDate.Text,
//				EndDate= editText_EndDate.Text,
//				PriorityId= Selected_PriorityID,
//				MilestoneStatusId= Selected_StatusID,
//				AllocatedHours= editText_AllocatedHours.Text,
//				IsUserWatch = cb_WatchList.Checked,
//				UpdatedBy = MilestoneDetail.CreatedBy,
//			};
//
//			var objEditMilestone = (new
//				{
//					objMilestone = new
//					{
//						UserId = Settings.UserId,
//						UserName = Settings.Username,
//						Item = objItem
//					}
//				});
//
//			string results= ConnectWebAPI.Request(url,objEditMilestone);
//
//			if (results != null) {
//				ResultsJson ResultsJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultsJson> (results);
//
//				if (ResultsJson.Success) {
//					Toast.MakeText (this, "Milestone Saved", ToastLength.Short).Show ();
//				} else
//					Toast.MakeText (this, ResultsJson.ErrorMessage, ToastLength.Short).Show ();
//			}
//			else
//				Toast.MakeText (this, "Error to connect to server", ToastLength.Short).Show ();
//
//		}

	}
}

