
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
	[Activity (Label = "Ticket")]				
	public class TicketActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Ticket);
			// Create your application here

			var BackButton = FindViewById(Resource.Id.BackButton);
			BackButton.Click += btBackClick;

			var bt_Open = FindViewById<Button>(Resource.Id.bt_Open);
			bt_Open.SetBackgroundColor (Color.Blue);

			var bt_Progress = FindViewById<Button>(Resource.Id.bt_Progress);
			bt_Progress.SetBackgroundColor (Color.Orange);

			var bt_Waiting = FindViewById<Button>(Resource.Id.bt_Waiting);
			bt_Waiting.SetBackgroundColor (Color.Yellow);

			var bt_Reject = FindViewById<Button>(Resource.Id.bt_Reject);
			bt_Reject.SetBackgroundColor (Color.Violet);

			var bt_Reopen = FindViewById<Button>(Resource.Id.bt_Reopen);
			bt_Reopen.SetBackgroundColor (Color.Pink);

			var bt_Close = FindViewById<Button>(Resource.Id.bt_Close);
			bt_Close.SetBackgroundColor (Color.Green);

		}

		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}
	}
}

