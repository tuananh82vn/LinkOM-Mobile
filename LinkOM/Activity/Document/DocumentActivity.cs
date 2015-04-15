﻿
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
	[Activity (Label = "Document")]				
	public class DocumentActivity : ListActivity
	{
		public bool loading;

		public string TokenNumber;
		public DocumentListAdapter documentList; 
		public ListView projectListView;
		public SwipeRefreshLayout refresher;
	
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Document);
			// Create your application here

			var BackButton = FindViewById(Resource.Id.BackButton);
			BackButton.Click += btBackClick;

			InitData ();

			refresher = FindViewById<SwipeRefreshLayout> (Resource.Id.refresher);

			refresher.SetColorScheme (Resource.Color.golden,Resource.Color.ginger_brown,Resource.Color.french_blue,Resource.Color.fern_green);
		
			refresher.Refresh += HandleRefresh;

		}

		async void HandleRefresh (object sender, EventArgs e)
		{
			await InitData ();
			refresher.Refreshing = false;
		}

		public async Task InitData(){

			if (loading)
				return;
			loading = true;

			TokenNumber = Settings.Token;
			string url = Settings.InstanceURL;

			url=url+"/api/DocumentList";


//			List<objSort> objSort = new List<objSort>{
//				new objSort{ColumnName = "P.Name", Direction = "1"},
//				new objSort{ColumnName = "C.Name", Direction = "2"}
//			};

			var objDocument = new
			{
				Title = string.Empty,
				DocumentCategoryId = string.Empty,
				ProjectId = string.Empty,
				DepartmentId = string.Empty,
				Label = string.Empty,

			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = TokenNumber,
						PageSize = 20,
						PageNumber = 1,
						Item = objDocument
					}
				});

			string results=  ConnectWebAPI.Request(url,objsearch);

			DocumentList DocumentList = Newtonsoft.Json.JsonConvert.DeserializeObject<DocumentList> (results);

			documentList = new DocumentListAdapter (this,DocumentList.Items);

			projectListView = FindViewById<ListView> (Android.Resource.Id.List);

			projectListView.Adapter = documentList;

//			projectListView.ItemClick += listView_ItemClick;
//
//			RegisterForContextMenu(projectListView);
//
			loading = false;
//
//			Console.WriteLine ("End load data");
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

