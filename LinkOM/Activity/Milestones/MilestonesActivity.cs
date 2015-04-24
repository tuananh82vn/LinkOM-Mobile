
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

using MonoDroid.TimesSquare;

namespace LinkOM
{
	[Activity (Label = "MilestonesActivity", Theme = "@style/Theme.Customtheme")]					
	public class MilestonesActivity : Activity
	{
//		CustomCalendar CalendarControl;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			RequestWindowFeature (WindowFeatures.ActionBar);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Milestones);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.milestone_title);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);
		}

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

		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}

	}
}

