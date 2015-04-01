
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

using System.Threading;
using System.Threading.Tasks;

namespace LinkOM
{
	[Activity (Label = "Project")]				
	public class ProjectActivity : ListActivity
	{
		public string TokenNumber;
		public ProjectListAdapter projectList; 
		public ListView projectListView;

	
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Project);
			// Create your application here

			var BackButton = FindViewById(Resource.Id.BackButton);
			BackButton.Click += btBackClick;


			TokenNumber = Settings.Token;
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

			projectListView = FindViewById<ListView> (Android.Resource.Id.List);

			projectListView.Adapter = projectList;

			projectListView.ItemClick += listView_ItemClick;

			RegisterForContextMenu(projectListView);

		}


		private void finish(){
			//SaveData();     
			this.Finish ();
			Android.OS.Process.KillProcess (Android.OS.Process.MyPid ());
		}


		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			//Get our item from the list adapter
			var ProjectId = this.projectList.GetItemId(e.Position);

			var activity = new Intent (this, typeof(ProjectDetailActivity));

			activity.PutExtra ("ProjectId", ProjectId);

			StartActivity (activity);
		}

		public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
		{
			if (v.Id == Android.Resource.Id.List)
			{
				var info = (AdapterView.AdapterContextMenuInfo) menuInfo;
				menu.SetHeaderTitle(projectList.GetItemName(info.Position));
				var menuItems = Resources.GetStringArray(Resource.Array.menu);
				for (var i = 0; i < menuItems.Length; i++)
					menu.Add(Menu.None, i, i, menuItems[i]);
			}
		}

		public override bool OnContextItemSelected(IMenuItem item)
		{
			var info = (AdapterView.AdapterContextMenuInfo) item.MenuInfo;
			var menuItemIndex = item.ItemId;
			var menuItems = Resources.GetStringArray(Resource.Array.menu);
			var menuItemName = menuItems[menuItemIndex];

			var ProjectName = projectList.GetItemName(info.Position);
			int ProjectId = int.Parse(projectList.GetItemId(info.Position).ToString());

			if (menuItemName.Equals ("Add Task")) {
				var activity = new Intent (this, typeof(AddTaskActivity));
				activity.PutExtra ("ProjectId", ProjectId);
				StartActivity (activity);
			}
			else
				Toast.MakeText(this, string.Format("Selected {0} for item {1}", menuItemName, ProjectName), ToastLength.Short).Show();

			return true;
		}
	}
}

