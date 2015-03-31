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
using Newtonsoft.Json;
using Android.Content.PM;


namespace LinkOM
{
	[Activity(Label = "Link-OM", Icon = "@drawable/Synotive")]		
	public class HomeActivity : Activity
	{
		public string TokenNumber = "";
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetOrientaion ();

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Home);

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

			TokenNumber = Intent.GetStringExtra ("TokenNumber") ?? "";

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

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.menu_home, menu);
			return base.OnPrepareOptionsMenu(menu);
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
			Toast.MakeText (this, "Coming Soon...", ToastLength.Short).Show ();
		}

		public void bt_MilestoneClick(object sender, EventArgs e)
		{
			Toast.MakeText (this, "Coming Soon...", ToastLength.Short).Show ();
		}

		public void bt_IssuesClick(object sender, EventArgs e)
		{
			Toast.MakeText (this, "Coming Soon...", ToastLength.Short).Show ();
		}

		public void bt_DocumentClick(object sender, EventArgs e)
		{
			Toast.MakeText (this, "Coming Soon...", ToastLength.Short).Show ();
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.menu_search:
				Toast.MakeText (this, "Menu Search Clicked", ToastLength.Short).Show ();
				return true;
			case Resource.Id.menu_help:
				Toast.MakeText (this, "Menu Help Clicked", ToastLength.Short).Show ();
				return true;
			case Resource.Id.menu_server:
				var activity = new Intent (this, typeof(CheckActivity));
				activity.PutExtra ("CheckAgain", true);
				StartActivity (activity);
				this.Finish ();
				return true;
			}
			return base.OnOptionsItemSelected(item);
		}

		public override void OnBackPressed() {
			ShowAlert ();
		}

		private void finish(){
			//SaveData();     
			base.OnBackPressed();
			this.Finish ();
			Android.OS.Process.KillProcess (Android.OS.Process.MyPid ());
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
	}
}

