
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
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Content.PM;


namespace LinkOM
{
	[Activity(Label = "Link-OM", Theme = "@style/MainTheme")]					
	public class MainActivity : Activity
	{

		public FlyOutContainer Menu  ;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);



			Menu = FindViewById<FlyOutContainer> (Resource.Id.MainContainer);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			//ActionBar.SetTitle(Resource.String.project_detail_title);
			//ActionBar.SetSubtitle(Resource.String.actionbar_sub);
			//ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);


			ImageButton bt_Task = FindViewById<ImageButton>(Resource.Id.bt_Task);
			bt_Task.Click += btTaskClick;

			ImageButton bt_Project = FindViewById<ImageButton>(Resource.Id.bt_Project);
			bt_Project.Click += btProjectClick;

			ImageButton bt_Ticket = FindViewById<ImageButton>(Resource.Id.bt_Ticket);
			bt_Ticket.Click += bt_TicketClick;

			ImageButton bt_Milestone = FindViewById<ImageButton>(Resource.Id.bt_Milestone);
			bt_Milestone.Click += bt_MilestoneClick;

			ImageButton bt_Issues = FindViewById<ImageButton>(Resource.Id.bt_Issues);
			bt_Issues.Click += bt_IssuesClick;

			ImageButton bt_Document = FindViewById<ImageButton>(Resource.Id.bt_Document);
			bt_Document.Click += bt_DocumentClick;

			var menu_Project = FindViewById (Resource.Id.menu_Project);
			menu_Project.Click +=	menu_Project_Click;		

			var menu_Task = FindViewById (Resource.Id.menu_Task);
			menu_Task.Click +=	menu_Task_Click;		

			var menu_Ticket = FindViewById (Resource.Id.menu_Ticket);
			menu_Ticket.Click +=	menu_Ticket_Click;		

			var menu_Issues = FindViewById (Resource.Id.menu_Issues);
			menu_Issues.Click +=	menu_Issues_Click;		


			var menu_Milestone = FindViewById (Resource.Id.menu_Milestone);
			menu_Milestone.Click +=	menu_Milestone_Click;		

			var menu_Document = FindViewById (Resource.Id.menu_Document);
			menu_Document.Click +=	menu_Document_Click;	


			var menu_ChangeServer = FindViewById (Resource.Id.menu_ChangeServer);
			menu_ChangeServer.Click +=	menu_ChangeServer_Click;


		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			base.OnOptionsItemSelected (item);

			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					Menu.AnimatedOpened = !Menu.AnimatedOpened;
					break;
				default:
					break;
			}

			return true;
		}

		private void SetOrientaion(){
			int minWidth= Settings.SmallestWidth;
			if (minWidth > 360) {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}
			else if (minWidth <= 360) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			}
		}

		void menu_ChangeServer_Click (object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(CheckActivity));
			activity.PutExtra ("CheckAgain", true);
			StartActivity (activity);
			this.Finish ();
		}

		void menu_Task_Click (object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(TaskActivity));
			StartActivity (activity);
		}

		void menu_Ticket_Click (object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(TicketActivity));
			StartActivity (activity);
		}

		void menu_Issues_Click (object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(IssuesActivity));
			StartActivity (activity);
		}

		void menu_Project_Click (object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(ProjectActivity));
			StartActivity (activity);
		}

		void menu_Milestone_Click (object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(MilestonesActivity));
			StartActivity (activity);
		}

		void menu_Document_Click (object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(DocumentActivity));
			StartActivity (activity);
		}

		public void btTaskClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(TaskActivity));
			StartActivity (activity);
		}

		public void btProjectClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(ProjectActivity));
			StartActivity (activity);
		}

		public void bt_TicketClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(TicketActivity));
			StartActivity (activity);
		}

		public void bt_MilestoneClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(MilestonesActivity));
			StartActivity (activity);
		}

		public void bt_IssuesClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(IssuesActivity));
			StartActivity (activity);
		}

		public void bt_DocumentClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(DocumentActivity));
			StartActivity (activity);
		}

		public override void OnBackPressed() {
			ShowAlert ();
		}

		public void ShowAlert()
		{
			Android.App.AlertDialog.Builder builder = new AlertDialog.Builder(this);
			AlertDialog alertDialog = builder.Create();
			alertDialog.SetTitle("Link-OM");
			alertDialog.SetIcon(Resource.Drawable.Icon);
			alertDialog.SetMessage("Do you really want to exit?");
			//YES
			alertDialog.SetButton("Yes", (s, ev) =>
				{
					finish();
				});

			//NO
			alertDialog.SetButton3("No", (s, ev) =>
				{
					alertDialog.Hide();
				});

			alertDialog.Show();
		}

		void SignOutButton_Click (object sender, EventArgs e)
		{
			ShowAlert ();
		}

		private void finish(){
			//SaveData();     
			base.OnBackPressed();
			this.Finish ();
			Android.OS.Process.KillProcess (Android.OS.Process.MyPid ());
		}


	}
}

