
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

		public ProjectSpinnerAdapter projectList; 
		private EditText editText_Title;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.EditTask);

			ImageButton bt_Back = FindViewById<ImageButton>(Resource.Id.bt_Back);
			bt_Back.Click += btBackClick;

			string jsonTask = Intent.GetStringExtra("Task");

			Task model = Newtonsoft.Json.JsonConvert.DeserializeObject<Task> (jsonTask);

			editText_Title = FindViewById<EditText> (Resource.Id.editText_Title);
			editText_Title.Text= model.Title;
			

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

			Spinner sp_Project = FindViewById<Spinner> (Resource.Id.sp_Project);
			//var ProjectAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.TaskPriority, Android.Resource.Layout.SimpleSpinnerItem);
			sp_Project.Adapter = projectList;

			if(model.ProjectId.Value!=0)
				sp_Project.SetSelection(projectList.getPositionById(model.ProjectId.Value)); 

			//Handle Status

			Spinner st_Status = FindViewById<Spinner> (Resource.Id.sp_Status);
			var StatusAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.TaskStatus, Android.Resource.Layout.SimpleSpinnerItem);
			StatusAdapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			st_Status.Adapter = StatusAdapter;

			if(model.StatusName!=""){
				int index = StatusAdapter.GetPosition (model.StatusName);
				st_Status.SetSelection(index); 
			}

			Spinner st_Priority = FindViewById<Spinner> (Resource.Id.sp_Priority);
			var PriorityAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.TaskPriority, Android.Resource.Layout.SimpleSpinnerItem);
			PriorityAdapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			st_Priority.Adapter = PriorityAdapter;

			if(model.PriorityName!=""){
				int index = PriorityAdapter.GetPosition (model.PriorityName);
				st_Priority.SetSelection(index); 
			}


		}

		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}
	}
}

