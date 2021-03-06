﻿using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Graphics.Drawables;



namespace LinkOM
{
	[Activity (Label = "Home", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Customtheme")]	
	public class HomeActivity : FragmentActivity
	{

		private MyActionBarDrawerToggle drawerToggle;
		private string drawerTitle;
		private string title;

		private DrawerLayout drawerLayout;

		private ListView drawerListView;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.HomeLayout);

			this.title = this.drawerTitle = this.Title;

			this.drawerLayout = this.FindViewById<DrawerLayout> (Resource.Id.drawer_layout);

			this.drawerListView = this.FindViewById<ListView> (Resource.Id.left_drawer);


			//Create Adapter for drawer List
			this.drawerListView.Adapter = new MenuListAdapter(this);
			drawerListView.DividerHeight=0;


			//Set click handler when item is selected
			this.drawerListView.ItemClick += (sender, args) => ListItemClicked (args.Position);

			//Set Drawer Shadow
			this.drawerLayout.SetDrawerShadow (Resource.Drawable.drawer_shadow_dark, (int)GravityFlags.Start);



			//DrawerToggle is the animation that happens with the indicator next to the actionbar
			this.drawerToggle = new MyActionBarDrawerToggle (this, this.drawerLayout,
				Resource.Drawable.action_menu,
				Resource.String.drawer_open,
				Resource.String.drawer_close);

			//Display the current fragments title and update the options menu
			this.drawerToggle.DrawerClosed += (o, args) => {
				this.ActionBar.Title = this.title;
				this.InvalidateOptionsMenu ();
			};

			//Display the drawer title and update the options menu
			this.drawerToggle.DrawerOpened += (o, args) => {
				this.ActionBar.Title = this.drawerTitle;
				this.InvalidateOptionsMenu ();
			};

			//Set the drawer lister to be the toggle.
			this.drawerLayout.SetDrawerListener (this.drawerToggle);

			Android.Support.V4.App.Fragment fragment = new HomeFragment (this);

			SupportFragmentManager.BeginTransaction ().Replace (Resource.Id.content_frame, fragment).Commit ();
			

			this.ActionBar.SetDisplayHomeAsUpEnabled (true);
			this.ActionBar.SetHomeButtonEnabled (true);

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}
		}

		private void ListItemClicked (int position)
		{
			
			Intent activity=null;
			switch (position) {
			case 0:
				activity = new Intent (this, typeof(DashboardActivity));
				StartActivity (activity);
				break;
			case 2:
				activity = new Intent (this, typeof(ProjectActivity));
				StartActivity (activity);
				break;
			case 3:
				activity = new Intent (this, typeof(TaskActivity));
				StartActivity (activity);
				break;
			case 4:
				activity = new Intent (this, typeof(TicketActivity));
				StartActivity (activity);
				break;
			case 5:
				activity = new Intent (this, typeof(IssuesActivity));
				StartActivity (activity);
				break;
			case 6:
				activity = new Intent (this, typeof(MilestoneActivity));
				StartActivity (activity);
				break;
			case 7:
				activity = new Intent (this, typeof(DocumentActivity));
				StartActivity (activity);
				break;
			case 8:
				activity = new Intent (this, typeof(KnowledgebaseActivity));
				StartActivity (activity);
				break;

			case 10:
				Toast.MakeText (this, "Coming Soon.", ToastLength.Short).Show ();
				break;
			case 11:
				Toast.MakeText (this, "Coming Soon.", ToastLength.Short).Show ();
				break;
			case 12:
				activity = new Intent (this, typeof(AlertConfigurationActivity));
				StartActivity (activity);
				break;
			case 14:
				Toast.MakeText (this, "Coming Soon.", ToastLength.Short).Show ();
				break;
			case 15:
				Toast.MakeText (this, "Coming Soon.", ToastLength.Short).Show ();
				break;
			}

			//this.drawerListView.SetItemChecked (position, true);
			//ActionBar.Title = this.title = Sections [position];
			this.drawerLayout.CloseDrawers();
		}

		//Init menu on action bar
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			inflater.Inflate (Resource.Menu.RefeshMenu, menu);

			return true;
		}

		public override bool OnPrepareOptionsMenu (IMenu menu)
		{

			var drawerOpen = this.drawerLayout.IsDrawerOpen((int)GravityFlags.Left);
			//when open don't show anything
			for (int i = 0; i < menu.Size (); i++)
				menu.GetItem (i).SetVisible (!drawerOpen);


			return base.OnPrepareOptionsMenu (menu);
		}

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			this.drawerToggle.SyncState ();
		}

		public override void OnConfigurationChanged (Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			this.drawerToggle.OnConfigurationChanged (newConfig);
		}

		// Pass the event to ActionBarDrawerToggle, if it returns
		// true, then it has handled the app icon touch event
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (this.drawerToggle.OnOptionsItemSelected (item))
				return true;

			base.OnOptionsItemSelected (item);

			switch (item.ItemId)
			{

				case Resource.Id.refesh:
					Android.Support.V4.App.Fragment fragment = new HomeFragment (this);
					SupportFragmentManager.BeginTransaction ().Replace (Resource.Id.content_frame, fragment).Commit ();
					Toast.MakeText (this, "Reloaded", ToastLength.Short).Show ();
					break;
			
				default:
					break;
			}

			return true;
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
					Settings.Token = "";
					finish();
				});

			//NO
			alertDialog.SetButton3("No", (s, ev) =>
				{
					alertDialog.Hide();
				});

			alertDialog.Show();
		}

		private void finish(){
			//SaveData();     
			base.OnBackPressed();
			this.Finish ();
			Android.OS.Process.KillProcess (Android.OS.Process.MyPid ());
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



	}
}

