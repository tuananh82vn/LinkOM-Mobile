
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LinkOM
{
	[Activity (Label = "Project",ScreenOrientation = ScreenOrientation.Portrait)]				
	public class ProjectActivity : Activity
	{
		public string TokenNumber;
		public ProjectListAdapter projectList; 

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.ProjectView);
			// Create your application here

			ImageButton buttonBack = FindViewById<ImageButton>(Resource.Id.bt_Back);
			buttonBack.Click += btBackClick;


			TokenNumber = Intent.GetStringExtra ("TokenNumber") ?? "";


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

			projectList = new ProjectListAdapter (this,ProjectList.Items);

			var projectListView = FindViewById<ListView> (Resource.Id.ProjectListView);

			projectListView.Adapter = projectList;

			projectListView.ItemClick += listView_ItemClick;


		}

		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			//Get our item from the list adapter
			long ProjectId = this.projectList.GetItemId(e.Position);

			var activity = new Intent (this, typeof(ProjectDetailActivity));
			activity.PutExtra ("TokenNumber", TokenNumber);
			activity.PutExtra ("ProjectId", ProjectId);
			StartActivity (activity);
		}
	}
}

