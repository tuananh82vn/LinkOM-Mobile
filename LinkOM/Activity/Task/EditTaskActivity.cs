
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
	[Activity (Label = "EditTaskActivity")]			
	public class EditTaskActivity : Activity
	{
		const int Start_DATE_DIALOG_ID = 0;
		const int End_DATE_DIALOG_ID = 1;

		public ProjectSpinnerAdapter projectList; 
		public Spinner sp_Project;
		public ArrayAdapter PriorityAdapter;


		private EditText editText_Title;
		public TaskObject model;

		private TextView tv_StartDate;
		private Button bt_StartDate;
		private TextView tv_EndDate;
		private Button bt_EndDate;

		private DateTime StartDate;
		private DateTime EndDate;

		private int Selected_ProjectID;

		private int Selected_PriorityID;

		private int Selected_StatusID;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.TaskEdit);

			//Init UI

			var  BackButton = FindViewById(Resource.Id.BackButton);
			BackButton.Click += btBackClick;

			Button bt_Save = FindViewById<Button>(Resource.Id.bt_Save);
			bt_Save.Click += btSaveClick;

			// get the current date
			StartDate = DateTime.Today;

			// get the current date
			EndDate = DateTime.Today;

			tv_StartDate = FindViewById<TextView> (Resource.Id.tv_StartDate);
			tv_StartDate.Text = StartDate.ToString ("d");
			tv_EndDate = FindViewById<TextView> (Resource.Id.tv_EndDate);
			tv_EndDate.Text = EndDate.ToString ("d");

			bt_StartDate = FindViewById<Button> (Resource.Id.bt_StartDate);
			bt_EndDate = FindViewById<Button> (Resource.Id.bt_EndDate);

			// add a click event handler to the button
			bt_StartDate.Click += delegate { ShowDialog (Start_DATE_DIALOG_ID); };
			// add a click event handler to the button
			bt_EndDate.Click += delegate { ShowDialog (End_DATE_DIALOG_ID); };


			//Init variable
			string jsonTask = Intent.GetStringExtra("Task");
			model = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskObject> (jsonTask);


			//Handle Title
			editText_Title = FindViewById<EditText> (Resource.Id.editText_Title);
			editText_Title.Text= model.Title;
			
			GetProjectList ();

			GetStatusList ();

			GetPriorityList ();
		}

		private void GetPriorityList(){
			//Handle priority
			Spinner st_Priority = FindViewById<Spinner> (Resource.Id.sp_Priority);
			PriorityAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.TaskPriority, Android.Resource.Layout.SimpleSpinnerItem);
			PriorityAdapter.SetDropDownViewResource (Android.Resource.Layout.SelectDialogSingleChoice);
			st_Priority.Adapter = PriorityAdapter;

			if(model.PriorityName!=""){
				int index = PriorityAdapter.GetPosition (model.PriorityName);
				st_Priority.SetSelection(index); 
			}

			st_Priority.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Priority_ItemSelected);
		}

		private void GetStatusList(){
			//Handle Status
			Spinner st_Status = FindViewById<Spinner> (Resource.Id.sp_Status);
			var StatusAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.TaskStatus, Android.Resource.Layout.SimpleSpinnerItem);
			StatusAdapter.SetDropDownViewResource (Android.Resource.Layout.SelectDialogSingleChoice);
			st_Status.Adapter = StatusAdapter;

			if(model.StatusName!=""){
				int index = StatusAdapter.GetPosition (model.StatusName);
				st_Status.SetSelection(index); 
			}

			st_Status.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (Status_ItemSelected);
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
			sp_Project = FindViewById<Spinner> (Resource.Id.sp_Project);
			sp_Project.Adapter = projectList;
			if(model.ProjectId.Value!=0)
				sp_Project.SetSelection(projectList.getPositionById(model.ProjectId.Value)); 

			sp_Project.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (project_ItemSelected);
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

		private void project_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Selected_ProjectID = projectList.GetItemAtPosition (e.Position).Id;
		}

		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}

		public void btSaveClick(object sender, EventArgs e)
		{
			string TokenNumber = Settings.Token;
			string url = Settings.InstanceURL;

			url=url+"/api/EditTask";

			var objItem = new
			{
				Id = model.Id,
				Guid = model.Guid,
				AssignedToId= model.AssignedToId,
				AssignToName= String.Empty,
				Title= editText_Title.Text,
				ProjectId= Selected_ProjectID,
				OwnerId= model.OwnerId,
				StartDate= tv_EndDate.Text,
				EndDate= String.Empty,
				PriorityId= Selected_PriorityID,
				TaskStatusId= Selected_StatusID,
				UpdatedBy = model.CreatedBy,
			};

			var objEditTask = (new
				{
					objTask = new
					{
						UserId = Settings.UserId,
						UserName = Settings.Username,
						Item = objItem
					}
				});

			string results= ConnectWebAPI.Request(url,objEditTask);

			ResultsJson ResultsJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultsJson> (results);

			if (ResultsJson.Success) {
				Toast.MakeText (this, "Task Saved", ToastLength.Short).Show ();
			}
			else
				Toast.MakeText (this, ResultsJson.ErrorMessage, ToastLength.Short).Show ();

		}

		// the event received when the user "sets" the date in the dialog
		void OnStartDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			tv_StartDate.Text = e.Date.ToString ("d");
		}

		// the event received when the user "sets" the date in the dialog
		void OnEndDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			tv_EndDate.Text = e.Date.ToString ("d");
		}

		protected override Dialog OnCreateDialog (int id)
		{
			switch (id) {
			case Start_DATE_DIALOG_ID:
				return new DatePickerDialog (this, OnStartDateSet, StartDate.Year, StartDate.Month - 1, StartDate.Day); 
			case End_DATE_DIALOG_ID:
				return new DatePickerDialog (this, OnEndDateSet, EndDate.Year, EndDate.Month - 1, EndDate.Day); 
			}
			return null;
		}
	}
}

