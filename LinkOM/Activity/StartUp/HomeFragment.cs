using System;
using System.Collections.Generic;

using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;

namespace LinkOM
{
	public class HomeFragment : Android.Support.V4.App.Fragment
	{
		public ListView milestoneListView;
		public MilestoneListAdapter milestoneList;
		public DashboardObject temp;

		public RelativeLayout bt_Task;
		public RelativeLayout bt_Project;
		public RelativeLayout bt_Ticket;
		public RelativeLayout bt_Milestone;
		public RelativeLayout bt_Issues;
		public RelativeLayout bt_Document;

		public HomeFragment(Activity context)
		{
			this.RetainInstance = true;


		}


		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			this.HasOptionsMenu = true;
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);

			var	view = inflater.Inflate (Resource.Layout.MainContentLayout2, null);

			bt_Task = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_Task);
			bt_Task.Touch += bt_TaskOnTouch;


			bt_Project = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_Project);
			bt_Project.Touch += bt_ProjectOnTouch;

			bt_Ticket = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_Ticket);
			bt_Ticket.Touch += bt_TicketOnTouch;

			bt_Milestone = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_Milestone);
			bt_Milestone.Touch += bt_MilestoneOnTouch;

			bt_Issues = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_Issues);
			bt_Issues.Touch += bt_IssuesOnTouch;

			bt_Document = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_Documents);
			bt_Document.Touch += bt_DocumentOnTouch;



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

		private void bt_DocumentOnTouch(object sender, View.TouchEventArgs touchEventArgs)
		{

			switch (touchEventArgs.Event.Action & MotionEventActions.Mask) 
			{
			case MotionEventActions.Down:

			case MotionEventActions.Move:
				bt_Document.SetBackgroundColor (Color.ParseColor("#C38741"));
				break;

			case MotionEventActions.Up:
				bt_Document.SetBackgroundColor (Color.ParseColor("#E8A455"));
				var activity = new Intent (base.Activity, typeof(DocumentActivity));
				StartActivity (activity);
				break;

			default:
				break;
			}

		}	

		private void bt_IssuesOnTouch(object sender, View.TouchEventArgs touchEventArgs)
		{

			switch (touchEventArgs.Event.Action & MotionEventActions.Mask) 
			{
			case MotionEventActions.Down:

			case MotionEventActions.Move:
				bt_Issues.SetBackgroundColor (Color.ParseColor("#1E7392"));
				break;

			case MotionEventActions.Up:
				bt_Issues.SetBackgroundColor (Color.ParseColor("#0292C5"));
				var activity = new Intent (base.Activity, typeof(IssuesActivity));
				StartActivity (activity);
				break;

			default:
				break;
			}

		}	

		private void bt_MilestoneOnTouch(object sender, View.TouchEventArgs touchEventArgs)
		{

			switch (touchEventArgs.Event.Action & MotionEventActions.Mask) 
			{
			case MotionEventActions.Down:

			case MotionEventActions.Move:
				bt_Milestone.SetBackgroundColor (Color.ParseColor("#2DA49A"));
				break;

			case MotionEventActions.Up:
				bt_Milestone.SetBackgroundColor (Color.ParseColor("#39CCC0"));
				var activity = new Intent (base.Activity, typeof(MilestoneActivity));
				StartActivity (activity);
				break;

			default:
				break;
			}

		}	

		private void bt_TicketOnTouch(object sender, View.TouchEventArgs touchEventArgs)
		{

			switch (touchEventArgs.Event.Action & MotionEventActions.Mask) 
			{
			case MotionEventActions.Down:

			case MotionEventActions.Move:
				bt_Ticket.SetBackgroundColor (Color.ParseColor("#E84044"));
				break;

			case MotionEventActions.Up:
				bt_Ticket.SetBackgroundColor (Color.ParseColor("#EE686B"));
				var activity = new Intent (base.Activity, typeof(TicketActivity));
				StartActivity (activity);
				break;

			default:
				break;
			}

		}	

		private void bt_ProjectOnTouch(object sender, View.TouchEventArgs touchEventArgs)
		{
			
			switch (touchEventArgs.Event.Action & MotionEventActions.Mask) 
			{
				case MotionEventActions.Down:

				case MotionEventActions.Move:
					bt_Project.SetBackgroundColor (Color.ParseColor("#3D984E"));
					break;

				case MotionEventActions.Up:
					bt_Project.SetBackgroundColor (Color.ParseColor("#5ECA72"));
					var activity = new Intent (base.Activity, typeof(ProjectActivity));
					StartActivity (activity);
					break;

				default:
					break;
			}

		}	

		private void bt_TaskOnTouch(object sender, View.TouchEventArgs touchEventArgs)
		{

			switch (touchEventArgs.Event.Action & MotionEventActions.Mask) 
			{
			case MotionEventActions.Down:

			case MotionEventActions.Move:
				bt_Task.SetBackgroundColor (Color.ParseColor("#6B4097"));
				break;

			case MotionEventActions.Up:
				bt_Task.SetBackgroundColor (Color.ParseColor("#936DBA"));
				var activity = new Intent (base.Activity, typeof(TaskActivity));
				StartActivity (activity);
				break;

			default:
				break;
			}

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

	}
}