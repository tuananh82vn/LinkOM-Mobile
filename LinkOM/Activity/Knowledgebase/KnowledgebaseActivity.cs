
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
using Android.Support.V4.Widget;

namespace LinkOM
{
	[Activity (Label = "Knowledgebase", Theme = "@style/Theme.Customtheme")]				
	public class KnowledgebaseActivity : ListActivity
	{
		public bool loading;

		public string TokenNumber;
		public KnowledgebaseListAdapter knowledgebaseList; 
		public ListView knowledgebaseListView;
		public SwipeRefreshLayout refresher;
	
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.Knowledgebase);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.Knowledgebase_title);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);
			// Create your application here


			InitData ();

//			refresher = FindViewById<SwipeRefreshLayout> (Resource.Id.refresher);
//
//			refresher.SetColorScheme (Resource.Color.golden,Resource.Color.ginger_brown,Resource.Color.french_blue,Resource.Color.fern_green);
//		
//			refresher.Refresh += HandleRefresh;

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

		}

		async void HandleRefresh (object sender, EventArgs e)
		{
			await InitData ();
			refresher.Refreshing = false;
		}

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

		public async Task InitData(){

			if (loading)
				return;
			loading = true;

			KnowledgebaseFilter objFilter = new KnowledgebaseFilter ();
			var ObjectReturn = KnowledgeBaseHelper.GetKnowledgebaseList (objFilter);

			if(ObjectReturn!=null)
			{
				knowledgebaseList = new KnowledgebaseListAdapter (this,ObjectReturn);

				knowledgebaseListView = FindViewById<ListView> (Android.Resource.Id.List);

				knowledgebaseListView.Adapter = knowledgebaseList;

	//			projectListView.ItemClick += listView_ItemClick;
	//
	//			RegisterForContextMenu(projectListView);
			}

			loading = false;

		}

		private void finish(){
			//SaveData();     
			this.Finish ();
			Android.OS.Process.KillProcess (Android.OS.Process.MyPid ());
		}


//		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
//		{
//			//Get our item from the list adapter
//			var DocumentId = this.projectList.GetItemId(e.Position);
//
//			var activity = new Intent (this, typeof(DocumentDetailActivity));
//
//			activity.PutExtra ("DocumentId", DocumentId);
//
//			StartActivity (activity);
//		}

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
//			var DocumentName = projectList.GetItemName(info.Position);
//			int DocumentId = int.Parse(projectList.GetItemId(info.Position).ToString());
//
//			if (menuItemName.Equals ("Add Task")) {
//				var activity = new Intent (this, typeof(AddTaskActivity));
//				activity.PutExtra ("DocumentId", DocumentId);
//				StartActivity (activity);
//			}
//			else
//				Toast.MakeText(this, string.Format("Selected {0} for item {1}", menuItemName, DocumentName), ToastLength.Short).Show();
//
//			return true;
//		}
	}
}

