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

namespace LinkOM
{
	[Activity (Label = "DashboardActivity", MainLauncher = true, Icon = "@drawable/icon")]
	public class DashboardActivity : Activity, INChartSeriesDataSource
	{
		NChartView mNChartView;

		Random random = new Random ();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.DashboardLayout);

			mNChartView = FindViewById <NChartView> (Resource.Id.surface);

			LoadView ();
		}

		private void LoadView ()
		{
			// Paste your license key here.
			mNChartView.Chart.LicenseKey = "C9gNX+mbr1aYRzgfhMVjROf68nx/i4dAEE6Z4P+HQ2fawVTcplK6jwPBQvElxgyWpduQS0zXvvzFH8L+UxiG777loO1+2iiwdZP11Z0EY3lHNut77fVyU2a7c+Fm7F9AggQy9sgVd+dWXjwMk/sMVaEoKsxSvMHS/ilhBNaeIUslXp5ZZv/ujnbyMIHbHoKFRMtWGy9/K2+qAP3whFSVSq+0w7me9vGLIxa1x5y1TCtSR/tHlFW1X2SuA1mMACqkCNt2lVsB590WM59p3B683tyqaT4LsWZPSTmjr43e7TdP2pG4IEOVd7DWXV9LPYICNi/JR5CKECw6pYrrmIgfzfcJxDG06yTc0CS/IbuLNHD658wMRpeO4+rrS4zS1DdJ0QUXHHpV0hNW/P5QWLcyWjjsr6v8kr/vFRvUX7PSWtWjNA+qFg71wrqx5NAYRkhN/Jl10Qhp97XQZTwGsxFcYu8rsH8gvmpZcSIOpzlYBWFFaDg55NKbTgbgcnGpjpzR6i1S7OYhox159CoD6BBb/cEoORf1Pz3C5UdHl938B+3y+z4FvH+ol1FOoJp7oXEfCM1rlm96C51pl7W92a2qfBShRhZqOQLUfr1jya+g8AHUt9MfLEinhn0WikyPJGfdROFbhlZHzI2DdyyJfZ0+lGlvR2EeMBKyBcC9T4dKUUg=";

			// Margin to ensure some free space for the iOS status bar.
			mNChartView.Chart.CartesianSystem.Margin = new NChartMargin (10.0f, 10.0f, 10.0f, 20.0f);

			// Create column series with colors from the array and add them to the chart.
			NChartColumnSeries series = new NChartColumnSeries ();
			series.Brush = new NChartSolidColorBrush (Color.Argb (255, 97, 205, 232));
			series.DataSource = this;
			mNChartView.Chart.AddSeries (series);

			// Update data in the chart.
			mNChartView.Chart.UpdateData ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			mNChartView.OnResume ();
		}

		protected override void OnPause ()
		{
			base.OnPause ();
			mNChartView.OnPause ();
		}

		public NChartPoint[] Points (NChartSeries series)
		{
			NChartPoint[] result = new NChartPoint[11];
			for (int i = 0; i <= 10; ++i)
				result [i] = new NChartPoint (NChartPointState.PointStateAlignedToXWithXY (i, random.Next (30) + 1), series);
			return result;
		}

		public string Name (NChartSeries series)
		{
			return "My series";
		}

		public Bitmap Image (NChartSeries series)
		{
			return null;
		}
	}
}


