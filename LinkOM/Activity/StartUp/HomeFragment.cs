using System;
using System.Collections.Generic;

using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkOM
{
	public class HomeFragment : Fragment
	{
		public ListView milestoneListView;
		public MilestoneListAdapter milestoneList;
		public DashboardObject temp;

		public HomeFragment()
		{
			this.RetainInstance = true;
		}


		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			this.HasOptionsMenu = true;
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);

			var	view = inflater.Inflate (Resource.Layout.MainContentLayout2, null);

			RelativeLayout bt_Task = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_Task);
			bt_Task.Click += btTaskClick;

			RelativeLayout bt_Project = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_Project);
			bt_Project.Click += btProjectClick;

			RelativeLayout bt_Ticket = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_Ticket);
			bt_Ticket.Click += bt_TicketClick;

			RelativeLayout bt_Milestone = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_Milestone);
			bt_Milestone.Click += bt_MilestoneClick;

			RelativeLayout bt_Issues = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_Issues);
			bt_Issues.Click += bt_IssuesClick;

			RelativeLayout bt_Document = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_Documents);
			bt_Document.Click += bt_DocumentClick;

			if (Settings.Orientation.Equals ("Landscape")) 
			{
				milestoneListView = view.FindViewById<ListView> (Resource.Id.MilestoneListView);
				InitDataMilestone ();
				InitDataDashboard (view);
			} 
			else 
			{
				InitSmallDataDashboard (view);
			}

			return view;
		}


		//Loading data
		public void InitDataMilestone(){

			var objMilestone = new MilestoneFilter ();
			//Get only Status Open
			objMilestone.StatusId = 1;
	
			milestoneList = new MilestoneListAdapter (this.Activity, MilestonesHelper.GetMilestonesList(objMilestone));

			milestoneListView.Adapter = milestoneList;

			milestoneListView.ItemClick += listView_ItemClick;

		}

		//Loading data
		public void InitSmallDataDashboard(View view){

				temp = DashboardHelper.GetDashboard ();
				//-------------------------------------------------------------------------------------------------//
				TextView tv_Project = view.FindViewById<TextView>(Resource.Id.tv_Project);
				if (temp.ProjectOpenCount.HasValue) {
					if(temp.ProjectOpenCount!=0)
						tv_Project.Text = temp.ProjectOpenCount.ToString ();
				}


				TextView tv_Milestone = view.FindViewById<TextView>(Resource.Id.tv_Milestone);
				if (temp.MilestoneOpenCount.HasValue) {
					if(temp.MilestoneOpenCount!=0)
						tv_Milestone.Text = temp.MilestoneOpenCount.ToString ();
				}

				TextView tv_Tickets = view.FindViewById<TextView>(Resource.Id.tv_Tickets);

				if (temp.TicketOpenCount.HasValue) {
					if(temp.TicketOpenCount!=0)
						tv_Tickets.Text = temp.TicketOpenCount.ToString ();
				}

				TextView tv_Tasks = view.FindViewById<TextView>(Resource.Id.tv_Tasks);
				if (temp.TaskOpenCount.HasValue) {
					if(temp.TaskOpenCount!=0)
						tv_Tasks.Text = temp.TaskOpenCount.ToString ();
				}

				TextView tv_Issues = view.FindViewById<TextView>(Resource.Id.tv_Issues);
				if (temp.IssueOpenCount.HasValue) {
					if(temp.IssueOpenCount!=0)
						tv_Issues.Text = temp.IssueOpenCount.ToString ();
				}

				//-------------------------------------------------------------------------------------------------//
		}

		//Loading data
		public void InitDataDashboard(View view){

					InitSmallDataDashboard (view);

					//-------------------------------------------------------------------------------------------------//
					TextView tv_MileStone_OverDue = view.FindViewById<TextView>(Resource.Id.tv_MileStone_OverDue);
					tv_MileStone_OverDue.Text = temp.MilestoneOverdueCount.ToString ();

					TextView tv_MileStone_Open = view.FindViewById<TextView>(Resource.Id.tv_MileStone_Open);
					tv_MileStone_Open.Text = temp.MilestoneOpenCount.ToString ();

					TextView tv_MileStone_Closed = view.FindViewById<TextView>(Resource.Id.tv_MileStone_Closed);
					tv_MileStone_Closed.Text = temp.MilestoneClosedCount.ToString ();

					TextView tv_MileStone_Other = view.FindViewById<TextView>(Resource.Id.tv_MileStone_Other);
					tv_MileStone_Other.Text = temp.MilestoneOtherCount.ToString ();

					TextView tv_MileStone_Total = view.FindViewById<TextView>(Resource.Id.tv_MileStone_Total);
					var MileStone_Total = temp.MilestoneOverdueCount.Value + temp.MilestoneOpenCount.Value + temp.MilestoneClosedCount.Value + temp.MilestoneOtherCount.Value;

					tv_MileStone_Total.Text = MileStone_Total.ToString ();
					//-------------------------------------------------------------------------------------------------//

					TextView tv_Ticket_OverDue = view.FindViewById<TextView>(Resource.Id.tv_Ticket_OverDue);
					tv_Ticket_OverDue.Text = temp.TicketOverdueCount.ToString ();

					TextView tv_Ticket_Open = view.FindViewById<TextView>(Resource.Id.tv_Ticket_Open);
					tv_Ticket_Open.Text = temp.TicketOpenCount.ToString ();

					TextView tv_Ticket_Closed = view.FindViewById<TextView>(Resource.Id.tv_Ticket_Closed);
					tv_Ticket_Closed.Text = temp.TicketClosedCount.ToString ();

					TextView tv_Ticket_Other = view.FindViewById<TextView>(Resource.Id.tv_Ticket_Other);
					tv_Ticket_Other.Text = temp.TicketOtherCount.ToString ();

					TextView tv_Ticket_Total = view.FindViewById<TextView>(Resource.Id.tv_Ticket_Total);
					var Ticket_Total = temp.TicketOverdueCount.Value + temp.TicketOpenCount.Value + temp.TicketClosedCount.Value + temp.TicketOtherCount.Value;

					tv_Ticket_Total.Text = Ticket_Total.ToString ();
					//-------------------------------------------------------------------------------------------------//

					TextView tv_Task_OverDue = view.FindViewById<TextView>(Resource.Id.tv_Task_OverDue);
					tv_Task_OverDue.Text = temp.TaskOverdueCount.ToString ();

					TextView tv_Task_Open = view.FindViewById<TextView>(Resource.Id.tv_Task_Open);
					tv_Task_Open.Text = temp.TaskOpenCount.ToString ();

					TextView tv_Task_Closed = view.FindViewById<TextView>(Resource.Id.tv_Task_Closed);
					tv_Task_Closed.Text = temp.TaskClosedCount.ToString ();

					TextView tv_Task_Other = view.FindViewById<TextView>(Resource.Id.tv_Task_Other);
					tv_Task_Other.Text = temp.TaskOtherCount.ToString ();

					TextView tv_Task_Total = view.FindViewById<TextView>(Resource.Id.tv_Task_Total);
					var Task_Total = temp.TaskOverdueCount.Value + temp.TaskOpenCount.Value + temp.TaskClosedCount.Value + temp.TaskOtherCount.Value;

					tv_Task_Total.Text = Task_Total.ToString ();
					//-------------------------------------------------------------------------------------------------//


					TextView tv_Issues_OverDue = view.FindViewById<TextView>(Resource.Id.tv_Issues_OverDue);
					tv_Issues_OverDue.Text = temp.IssueOverdueCount.ToString ();

					TextView tv_Issues_Open = view.FindViewById<TextView>(Resource.Id.tv_Issues_Open);
					tv_Issues_Open.Text = temp.IssueOpenCount.ToString ();

					TextView tv_Issues_Closed = view.FindViewById<TextView>(Resource.Id.tv_Issues_Closed);
					tv_Issues_Closed.Text = temp.IssueClosedCount.ToString ();

					TextView tv_Issues_Other = view.FindViewById<TextView>(Resource.Id.tv_Issues_Other);
					tv_Issues_Other.Text = temp.IssueOtherCount.ToString ();

					TextView tv_Issues_Total = view.FindViewById<TextView>(Resource.Id.tv_Issues_Total);
					var Issues_Total = temp.IssueOverdueCount.Value + temp.IssueOpenCount.Value + temp.IssueClosedCount.Value + temp.IssueOtherCount.Value;

					tv_Issues_Total.Text = Issues_Total.ToString ();
					//-------------------------------------------------------------------------------------------------//


					TextView tv_All_OverDue = view.FindViewById<TextView>(Resource.Id.tv_All_OverDue);
					var All_OverDue = temp.MilestoneOverdueCount.Value + temp.TicketOverdueCount.Value + temp.TaskOverdueCount.Value + temp.IssueOverdueCount.Value;

					tv_All_OverDue.Text = All_OverDue.ToString ();

					TextView tv_All_Open = view.FindViewById<TextView>(Resource.Id.tv_All_Open);
					var All_Open = temp.MilestoneOpenCount.Value + temp.TicketOpenCount.Value + temp.TaskOpenCount.Value + temp.IssueOpenCount.Value;

					tv_All_Open.Text = All_Open.ToString ();

					TextView tv_All_Closed = view.FindViewById<TextView>(Resource.Id.tv_All_Closed);
					var All_Closed = temp.MilestoneClosedCount.Value + temp.TicketClosedCount.Value + temp.TaskClosedCount.Value + temp.IssueClosedCount.Value;

					tv_All_Closed.Text = All_Closed.ToString ();

					TextView tv_All_Other = view.FindViewById<TextView>(Resource.Id.tv_All_Other);
					var All_Other = temp.MilestoneOtherCount.Value + temp.TicketOtherCount.Value + temp.TaskOtherCount.Value + temp.IssueOtherCount.Value;

					tv_All_Other.Text = All_Other.ToString ();

					TextView tv_All_Total = view.FindViewById<TextView>(Resource.Id.tv_All_Total);
					var All_Total = MileStone_Total + Ticket_Total + Task_Total + Issues_Total;

					tv_All_Total.Text = All_Total.ToString ();
		}

		//handle list item clicked
		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			//Get our item from the list adapter
			MilestonesList Milestone = this.milestoneList.GetItemAtPosition(e.Position);

			Intent addAccountIntent = new Intent (this.Activity, typeof(MilestoneActivity));
			//			addAccountIntent.SetFlags (ActivityFlags.ClearWhenTaskReset);

			addAccountIntent.PutExtra ("Milestone", Newtonsoft.Json.JsonConvert.SerializeObject(Milestone));

			StartActivity(addAccountIntent);

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