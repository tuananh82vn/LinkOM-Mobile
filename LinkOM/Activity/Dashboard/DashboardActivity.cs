using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Java.Lang;

using NChart3D_Android;
using System.Threading;
using System.Collections.Generic;
using Android.Content.PM;

namespace LinkOM
{
	[Activity (Label = "Dashboard", Theme = "@style/Theme.Customtheme")]	
	public class DashboardActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.SlidingTabLayout);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.Dashboard_title);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			FragmentTransaction transaction = FragmentManager.BeginTransaction();
			SlidingTabsFragment fragment = new SlidingTabsFragment();
			transaction.Replace(Resource.Id.sample_content_fragment, fragment);
			transaction.Commit();

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

		}



		//Handle item on action bar clicked
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			base.OnOptionsItemSelected (item);

			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				OnBackPressed ();
				break;
			
			default:
				break;
			}

			return true;
		}
	}
}


