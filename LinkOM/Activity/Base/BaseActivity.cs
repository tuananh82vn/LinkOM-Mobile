
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

namespace LinkOM
{
	[Activity(Label = "BaseActivity")]
	public class BaseActivity : Activity
	{
		protected static String m_TAG = "MainActivity";
		public static bool m_isAppWentToBg = false;
		public static bool m_isWindowFocused = false;
		public static bool m_isMenuOpened = false;
		public static bool m_isBackPressed = false;
		public static int  s_activitycounter = 0;
		public static bool s_mainactivityvisible = false;

		public System.Timers.Timer _backgroundtimer;
		public int _backgroundSeconds;

		public BaseActivity(){
			_backgroundtimer = new System.Timers.Timer ();
			//Trigger event every second
			_backgroundtimer.Interval = 1000;
			_backgroundtimer.Elapsed += OnTimeBackgrounddEvent;
			//count down 5 seconds
			_backgroundSeconds = 30;
			_backgroundtimer.Enabled = true;

		}

		private void OnTimeBackgrounddEvent(object sender, System.Timers.ElapsedEventArgs e)
		{
			_backgroundSeconds--;

			//Update visual representation here
			//Remember to do it on UI thread
		}

		public override void OnWindowFocusChanged(bool hasFocus)  ///
		{
			m_isWindowFocused = hasFocus;

			if (m_isBackPressed && !hasFocus)
			{
				m_isBackPressed = false;
				m_isWindowFocused = true;
			}

			base.OnWindowFocusChanged(hasFocus);
		}

		public void allapplicationdidenterbackground()
		{    
			//_backgroundtimer.Start ();
			m_isAppWentToBg = true;                
			//Toast.MakeText(Android.App.Application.Context, "All App is Going to Background", ToastLength.Short).Show();            
		}

		public override void OnBackPressed()
		{
			if (this is BaseActivity)
			{
			}
			else
			{
				m_isBackPressed = true;
			}

			base.OnBackPressed();
		}

		protected override void OnPause()
		{
			if (s_mainactivityvisible == false && s_activitycounter<1)
				allapplicationdidenterbackground();
			base.OnPause();
		}       
	} 
}

