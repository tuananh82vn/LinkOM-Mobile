
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
using Android.Views.Animations;
using Android.Net;
using Java.Net;


namespace LinkOM
{

	[Activity(Theme = "@style/Theme.Customtheme", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : Activity
	{
		public ImageView imageLogo;
		public Animation rotateAboutCenterAnimation;
		public System.Timers.Timer _backgroundtimer;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.SplashLayout);

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

//			//Register for push notification
//			GcmClient.CheckDevice(this);
//			GcmClient.CheckManifest(this);
//			string regId = GcmClient.GetRegistrationId(this);
//			Console.WriteLine("Registration id:"+regId);
//			if(regId.Trim().Equals("")){
//				GcmClient.Register (this, GcmBroadcastReceiver.SENDER_IDS);
//			}

			imageLogo = FindViewById<ImageView>(Resource.Id.floating_image);

			rotateAboutCenterAnimation = AnimationUtils.LoadAnimation(this, Resource.Animation.rotate_center);

			imageLogo.StartAnimation(rotateAboutCenterAnimation);

			if (NetworkHelper.DetectNetwork()) 
			{
				Init ();
			} 
			else 
			{
				Toast.MakeText (this, "No Connection ...", ToastLength.Short).Show ();
				KeepChecking ();
			}

		}

		private void Init()
		{
				// Simulate a long loading process on app startup.
			 	Task<bool>.Run (() => {
				Thread.Sleep (2000);
				StartActivity(typeof(CheckActivity));
				this.Finish();
			}); 
		}

		private void KeepChecking(){
			_backgroundtimer = new System.Timers.Timer ();
			//Trigger event every second
			_backgroundtimer.Interval = 6000;
			_backgroundtimer.Elapsed += OnTimeBackgrounddEvent;
			_backgroundtimer.Start ();
		}

		private void OnTimeBackgrounddEvent(object sender, System.Timers.ElapsedEventArgs e)
		{
			RunOnUiThread (() => imageLogo.StartAnimation(rotateAboutCenterAnimation));

			if (NetworkHelper.DetectNetwork()) 
			{
				_backgroundtimer.Stop ();
				Init ();
			} 
			else 
			{
				RunOnUiThread (() => Toast.MakeText (this, "No Connection ...", ToastLength.Long).Show ());
			}
		}
	}
}

