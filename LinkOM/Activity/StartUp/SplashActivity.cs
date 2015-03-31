
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
//			await Task<bool>.Run (() => {
//
//				int smallestWidth = Settings.SmallestWidth;
//
//
//				if(smallestWidth==0) {
//
//				var metrics = Resources.DisplayMetrics;
//				var widthPixels = metrics.WidthPixels;
//				var heightPixels = metrics.HeightPixels;
//
//				float scaleFactor = metrics.Density;
//
//				float widthDp = widthPixels / scaleFactor;
//				float heightDp = heightPixels / scaleFactor;
//
//					Settings.SmallestWidth = int.Parse(Math.Min(widthDp, heightDp).ToString());
//
//				}
//
//				//Thread.Sleep (1000);
//				return true;
//			}); 
			StartActivity(typeof(CheckActivity));
			this.Finish();
		}
	}
}

