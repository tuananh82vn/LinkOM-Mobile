
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Gcm.Client;
using Android.Content.PM;


namespace LinkOM
{

	[Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);
			Init ();
		}

		private void Init()
		{
			// Simulate a long loading process on app startup.
			 	Task<bool>.Run (() => {



				var metrics = Resources.DisplayMetrics;

				var widthPixels = metrics.WidthPixels;
				var heightPixels = metrics.HeightPixels;

				float scaleFactor = metrics.Density;

				float widthDp = widthPixels / scaleFactor;
				float heightDp = heightPixels / scaleFactor;

				Settings.SmallestWidth = int.Parse(Math.Min(widthDp, heightDp).ToString());

				int minWidth= Settings.SmallestWidth;

				if (minWidth > 360) {
					RequestedOrientation = ScreenOrientation.SensorLandscape;
					Settings.Orientation ="Landscape";
				}
				else if (minWidth <= 360) {
					RequestedOrientation = ScreenOrientation.SensorPortrait;
					Settings.Orientation ="Portrait";
				}



				GcmClient.CheckDevice(this);
				GcmClient.CheckManifest(this);
				string regId = GcmClient.GetRegistrationId(this);
				Console.WriteLine("Registration id:"+regId);
				if(regId.Trim().Equals("")){
					GcmClient.Register (this, GcmBroadcastReceiver.SENDER_IDS);
				}


				Thread.Sleep (1000);
				StartActivity(typeof(LoginActivity));
				this.Finish();
			}); 


		}
	}
}

