
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
using Android.Content.PM;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Android.Graphics;
using System.Threading.Tasks;
using System.Threading;
using Android.Support.V4.Widget;

namespace LinkOM
{
	[Activity (Label = "Issues")]				
	public class IssuesActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Issues);
			// Create your application here

			var BackButton = FindViewById(Resource.Id.BackButton);
			BackButton.Click += btBackClick;

			var bt_Open = FindViewById<Button>(Resource.Id.bt_Open);
			bt_Open.SetBackgroundColor (Color.Blue);

			var bt_Fixed = FindViewById<Button>(Resource.Id.bt_Fixed);
			bt_Fixed.SetBackgroundColor (Color.LightYellow);

			var bt_Reproduce = FindViewById<Button>(Resource.Id.bt_Reproduce);
			bt_Reproduce.SetBackgroundColor (Color.DeepPink);

			var bt_Reopen = FindViewById<Button>(Resource.Id.bt_Reopen);
			bt_Reopen.SetBackgroundColor (Color.Violet);

			var bt_Duplicate = FindViewById<Button>(Resource.Id.bt_Duplicate);
			bt_Duplicate.SetBackgroundColor (Color.DarkRed);

			var bt_Closed = FindViewById<Button>(Resource.Id.bt_Closed);
			bt_Closed.SetBackgroundColor (Color.Green);

		}

		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}
	}
}

