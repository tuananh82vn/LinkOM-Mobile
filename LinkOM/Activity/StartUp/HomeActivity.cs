
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
	[Activity(Label = "", Icon = "@drawable/Synotive",ScreenOrientation = ScreenOrientation.Portrait)]		
	public class HomeActivity : Activity
	{
		public string TokenNumber = "";
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Home);

			ImageButton bt_Task = FindViewById<ImageButton>(Resource.Id.bt_Task);
			bt_Task.Click += btTaskClick;

			ImageButton bt_Project = FindViewById<ImageButton>(Resource.Id.bt_Project);
			bt_Project.Click += btProjectClick;

			TokenNumber = Intent.GetStringExtra ("TokenNumber") ?? "";

		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.menu_home, menu);
			return base.OnPrepareOptionsMenu(menu);
		}



		public void btTaskClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(TaskActivity));
			activity.PutExtra ("TokenNumber", TokenNumber);
			StartActivity (activity);
		}

		public void btProjectClick(object sender, EventArgs e)
		{
			var activity = new Intent (this, typeof(ProjectActivity));
			activity.PutExtra ("TokenNumber", TokenNumber);
			StartActivity (activity);
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

