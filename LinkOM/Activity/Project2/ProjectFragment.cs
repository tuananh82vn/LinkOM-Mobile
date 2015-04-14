using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;

namespace LinkOM
{
	public class ProjectFragment : ListFragment
	{
		private int _projectId;

		public bool loading;

		public string TokenNumber;
		public ProjectListAdapter projectList; 
		public ListView projectListView;

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			base.OnActivityCreated(savedInstanceState);

			InitData ();

//			if (savedInstanceState != null)
//			{
//				_projectId = savedInstanceState.GetInt("projectId", 0);
//			}

		}

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

			projectList = new ProjectListAdapter (Activity,ProjectList.Items);

			projectListView = Activity.FindViewById<ListView>(Android.Resource.Id.List);

			projectListView.Adapter = projectList;

//			projectListView.ItemClick += listView_ItemClick;
//
//			RegisterForContextMenu(projectListView);

			loading = false;

			Console.WriteLine ("End load data");

		}
		public override void OnSaveInstanceState(Bundle outState)
		{
			base.OnSaveInstanceState(outState);
			outState.PutInt("projectId", _projectId);
		}

//		public override void OnListItemClick(ListView l, View v, int position, long id)
//		{
//			ShowDetails(position);
//		}
//
//		private void ShowDetails(int playId)
//		{
//				_projectId = playId;
//				// Otherwise we need to launch a new activity to display
//				// the dialog fragment with selected text.
//				var intent = new Intent();
//
////				intent.SetClass(Activity, typeof (ProjectDetailsActivity));
////
////				intent.PutExtra("projectId", _projectId);
////
////				StartActivity(intent);
//		}
	}
}