
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;

using Android.Support.V4.Widget;

namespace LinkOM
{
	[Activity (Label = "Project", Theme = "@style/Theme.Customtheme")]			
	public class ProjectActivity : ListActivity
	{
		public bool loading;

		public string TokenNumber;
		public ProjectListAdapter projectList; 
		public ListView projectListView;
		//public SwipeRefreshLayout refresher;
	
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.Project);
			// Create your application here

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.project_title);
			//ActionBar.SetSubtitle(Resource.String.actionbar_sub);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			InitData ();

//			refresher = FindViewById<SwipeRefreshLayout> (Resource.Id.refresher);
//
//			refresher.SetColorScheme (Resource.Color.xam_dark_blue,Resource.Color.xam_purple,Resource.Color.xam_gray,Resource.Color.xam_green);
//
//			refresher.Refresh += HandleRefresh;

		}

//		async void HandleRefresh (object sender, EventArgs e)
//		{
//			await InitData ();
//			refresher.Refreshing = false;
//		}

		public async Task InitData(){

			Console.WriteLine ("Begin load data");

			if (loading)
				return;
			loading = true;

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

			string results=  ConnectWebAPI.Request(url,objsearch);

			ProjectListJson ProjectList = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectListJson> (results);

			projectList = new ProjectListAdapter (this,ProjectList.Items);

			projectListView = FindViewById<ListView> (Android.Resource.Id.List);

			projectListView.Adapter = projectList;

			projectListView.ItemClick += listView_ItemClick;

			RegisterForContextMenu(projectListView);

			loading = false;

			Console.WriteLine ("End load data");
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
			default:
				break;
			}

			return true;
		}

		//handle list item clicked
		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			//Get our item from the list adapter
			var ProjectId = this.projectList.GetItemId(e.Position);

			Intent addAccountIntent = new Intent (this, typeof(ProjectDetailActivity));
			addAccountIntent.SetFlags (ActivityFlags.ClearWhenTaskReset);
			addAccountIntent.PutExtra ("ProjectId", ProjectId);
			StartActivity(addAccountIntent);

		}

		//Init menu on action bar
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			inflater.Inflate (Resource.Menu.AddSearchMenu, menu);

			return true;
		}

//		public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
//		{
//			if (v.Id == Android.Resource.Id.List)
//			{
//				var info = (AdapterView.AdapterContextMenuInfo) menuInfo;
//				menu.SetHeaderTitle(projectList.GetItemName(info.Position));
//				var menuItems = Resources.GetStringArray(Resource.Array.menu);
//				for (var i = 0; i < menuItems.Length; i++)
//					menu.Add(Menu.None, i, i, menuItems[i]);
//			}
//		}

//		public override bool OnContextItemSelected(IMenuItem item)
//		{
//			var info = (AdapterView.AdapterContextMenuInfo) item.MenuInfo;
//			var menuItemIndex = item.ItemId;
//			var menuItems = Resources.GetStringArray(Resource.Array.menu);
//			var menuItemName = menuItems[menuItemIndex];
//
//			var ProjectName = projectList.GetItemName(info.Position);
//			int ProjectId = int.Parse(projectList.GetItemId(info.Position).ToString());
//
//			if (menuItemName.Equals ("Add Task")) {
//				var activity = new Intent (this, typeof(AddTaskActivity));
//				activity.PutExtra ("ProjectId", ProjectId);
//				StartActivity (activity);
//			}
//			else
//				Toast.MakeText(this, string.Format("Selected {0} for item {1}", menuItemName, ProjectName), ToastLength.Short).Show();
//
//			return true;
//		}
	}
}

