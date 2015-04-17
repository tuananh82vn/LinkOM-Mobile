using System;
using System.Collections.Generic;

using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

//using NavDrawer.Activities;
//using NavDrawer.Adapters;
//using NavDrawer.Models;

namespace LinkOM
{
	public class HomeFragment : Fragment
	{

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



			return view;
		}


//		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
//		{
//			inflater.Inflate(Resource.Menu.FullMenu, menu);
//		}

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
			var activity = new Intent (base.Activity, typeof(MilestonesActivity));
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