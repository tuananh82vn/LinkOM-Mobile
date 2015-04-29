using System;
using System.Collections.Generic;

using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

//using NavDrawer.Activities;
//using NavDrawer.Adapters;
//using NavDrawer.Models;
using System.Threading.Tasks;

namespace LinkOM
{
	public class HomeFragment : Fragment
	{
		public ListView milestoneListView;
		public MilestoneListAdapter milestoneList;

		public HomeFragment()
		{
			this.RetainInstance = true;
		}

//		private List<FriendViewModel> _friends;

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			this.HasOptionsMenu = true;
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);

			var view = inflater.Inflate(Resource.Layout.MainContentLayout, null);

			ImageButton bt_Task = view.FindViewById<ImageButton>(Resource.Id.bt_Task);
			bt_Task.Click += btTaskClick;

			ImageButton bt_Project = view.FindViewById<ImageButton>(Resource.Id.bt_Project);
			bt_Project.Click += btProjectClick;

			ImageButton bt_Ticket = view.FindViewById<ImageButton>(Resource.Id.bt_Ticket);
			bt_Ticket.Click += bt_TicketClick;

			ImageButton bt_Milestone = view.FindViewById<ImageButton>(Resource.Id.bt_Milestone);
			bt_Milestone.Click += bt_MilestoneClick;

			ImageButton bt_Issues = view.FindViewById<ImageButton>(Resource.Id.bt_Issues);
			bt_Issues.Click += bt_IssuesClick;

			ImageButton bt_Document = view.FindViewById<ImageButton>(Resource.Id.bt_Document);
			bt_Document.Click += bt_DocumentClick;

			if(Settings.Orientation.Equals("Landscape")){
				milestoneListView = view.FindViewById<ListView> (Resource.Id.MilestoneListView);
				InitDataMilestone ();
				InitDataDashboard ();
			}

			return view;
		}


		//Loading data
		public void InitDataMilestone(){

			string url = Settings.InstanceURL;

			url=url+"/api/MilestoneList";


			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "T.Title", Direction = "1"},
				new objSort{ColumnName = "T.ProjectName", Direction = "2"}
			};

			var objMilestone = new
			{
				ProjectId = string.Empty,
				StatusId = string.Empty,
				DepartmentId = string.Empty,
				Title = string.Empty,
				PriorityId= string.Empty,
				Label= string.Empty,
				DueBefore= string.Empty,
				AssignTo= string.Empty,
				AssignByMe= string.Empty,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						PageSize = 100,
						PageNumber = 1,
						Sort = objSort,
						Item = objMilestone
					}
				});

			string results=  ConnectWebAPI.Request(url,objsearch);

			if (results != null) {

				MilestoneListJson MilestoneList = Newtonsoft.Json.JsonConvert.DeserializeObject<MilestoneListJson> (results);

				milestoneList = new MilestoneListAdapter (this.Activity, MilestoneList.Items);

				milestoneListView.Adapter = milestoneList;

				milestoneListView.ItemClick += listView_ItemClick;

			}

		}

		//Loading data
		public void InitDataDashboard(){

			string url = Settings.InstanceURL;

			url=url+"/api/DashboardList";


			var objsearch = (new
			{
				objApiSearch = new
				{
					TokenNumber = Settings.Token,
					Item = string.Empty
				}
			});

			string results=  ConnectWebAPI.Request(url,objsearch);

			if (results != null) {

				MilestoneListJson MilestoneList = Newtonsoft.Json.JsonConvert.DeserializeObject<MilestoneListJson> (results);

			}

		}

		//handle list item clicked
		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
//			//Get our item from the list adapter
//			MilestoneObject Milestone = this.milestoneList.GetItemAtPosition(e.Position);
//
//			Intent addAccountIntent = new Intent (this, typeof(MilestoneDetailActivity));
//			//			addAccountIntent.SetFlags (ActivityFlags.ClearWhenTaskReset);
//
//			addAccountIntent.PutExtra ("Milestone", Newtonsoft.Json.JsonConvert.SerializeObject(Milestone));
//
//			StartActivity(addAccountIntent);

		}

		public void btTaskClick(object sender, EventArgs e)
		{
			var activity = new Intent (base.Activity, typeof(TaskActivity));
			StartActivity (activity);
		}

		public void btProjectClick(object sender, EventArgs e)
		{
			var activity = new Intent (base.Activity, typeof(ProjectActivity));
			StartActivity (activity);
		}

		public void bt_TicketClick(object sender, EventArgs e)
		{
			var activity = new Intent (base.Activity, typeof(TicketActivity));
			StartActivity (activity);
		}

		public void bt_MilestoneClick(object sender, EventArgs e)
		{
			var activity = new Intent (base.Activity, typeof(MilestoneActivity));
			StartActivity (activity);
		}

		public void bt_IssuesClick(object sender, EventArgs e)
		{
			var activity = new Intent (base.Activity, typeof(IssuesActivity));
			StartActivity (activity);
		}

		public void bt_DocumentClick(object sender, EventArgs e)
		{
			var activity = new Intent (base.Activity, typeof(DocumentActivity));
			StartActivity (activity);
		}
	}
}