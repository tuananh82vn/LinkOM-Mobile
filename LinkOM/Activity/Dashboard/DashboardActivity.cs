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

namespace LinkOM
{
	[Activity (Label = "Dashboard", Theme = "@style/Theme.Customtheme")]	
	public class DashboardActivity : Activity , INChartDelegate
	{
		NChartView mNChartView1;
		NChartView mNChartView2;
		NChartView mNChartView3;

		Random random = new Random ();
		TaskDataSource taskData;
		ProjectTaskDataSource projectData;
		StreamingDataSource streamData;

		NChartBrush[] brushes;
		bool zoomed;
		NChartPoint prevSelectedPoint;


		Timer timer;
		object guard = new object ();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.DashboardLayout);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.Dashboard_title);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			mNChartView1 = FindViewById<NChartView> (Resource.Id.surface1);
			mNChartView2 = FindViewById<NChartView> (Resource.Id.surface2);
			mNChartView3 = FindViewById<NChartView> (Resource.Id.surface3);

			taskData = new TaskDataSource ();
			projectData = new ProjectTaskDataSource ();
			streamData = new StreamingDataSource ();

			LoadViews ();
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

		private void LoadViews ()
		{
			// Paste your license key here.
			mNChartView1.Chart.LicenseKey = "C9gNX+mbr1aYRzgfhMVjROf68nx/i4dAEE6Z4P+HQ2fawVTcplK6jwPBQvElxgyWpduQS0zXvvzFH8L+UxiG777loO1+2iiwdZP11Z0EY3lHNut77fVyU2a7c+Fm7F9AggQy9sgVd+dWXjwMk/sMVaEoKsxSvMHS/ilhBNaeIUslXp5ZZv/ujnbyMIHbHoKFRMtWGy9/K2+qAP3whFSVSq+0w7me9vGLIxa1x5y1TCtSR/tHlFW1X2SuA1mMACqkCNt2lVsB590WM59p3B683tyqaT4LsWZPSTmjr43e7TdP2pG4IEOVd7DWXV9LPYICNi/JR5CKECw6pYrrmIgfzfcJxDG06yTc0CS/IbuLNHD658wMRpeO4+rrS4zS1DdJ0QUXHHpV0hNW/P5QWLcyWjjsr6v8kr/vFRvUX7PSWtWjNA+qFg71wrqx5NAYRkhN/Jl10Qhp97XQZTwGsxFcYu8rsH8gvmpZcSIOpzlYBWFFaDg55NKbTgbgcnGpjpzR6i1S7OYhox159CoD6BBb/cEoORf1Pz3C5UdHl938B+3y+z4FvH+ol1FOoJp7oXEfCM1rlm96C51pl7W92a2qfBShRhZqOQLUfr1jya+g8AHUt9MfLEinhn0WikyPJGfdROFbhlZHzI2DdyyJfZ0+lGlvR2EeMBKyBcC9T4dKUUg=";
			// And here.
			mNChartView2.Chart.LicenseKey = "C9gNX+mbr1aYRzgfhMVjROf68nx/i4dAEE6Z4P+HQ2fawVTcplK6jwPBQvElxgyWpduQS0zXvvzFH8L+UxiG777loO1+2iiwdZP11Z0EY3lHNut77fVyU2a7c+Fm7F9AggQy9sgVd+dWXjwMk/sMVaEoKsxSvMHS/ilhBNaeIUslXp5ZZv/ujnbyMIHbHoKFRMtWGy9/K2+qAP3whFSVSq+0w7me9vGLIxa1x5y1TCtSR/tHlFW1X2SuA1mMACqkCNt2lVsB590WM59p3B683tyqaT4LsWZPSTmjr43e7TdP2pG4IEOVd7DWXV9LPYICNi/JR5CKECw6pYrrmIgfzfcJxDG06yTc0CS/IbuLNHD658wMRpeO4+rrS4zS1DdJ0QUXHHpV0hNW/P5QWLcyWjjsr6v8kr/vFRvUX7PSWtWjNA+qFg71wrqx5NAYRkhN/Jl10Qhp97XQZTwGsxFcYu8rsH8gvmpZcSIOpzlYBWFFaDg55NKbTgbgcnGpjpzR6i1S7OYhox159CoD6BBb/cEoORf1Pz3C5UdHl938B+3y+z4FvH+ol1FOoJp7oXEfCM1rlm96C51pl7W92a2qfBShRhZqOQLUfr1jya+g8AHUt9MfLEinhn0WikyPJGfdROFbhlZHzI2DdyyJfZ0+lGlvR2EeMBKyBcC9T4dKUUg=";
			mNChartView3.Chart.LicenseKey = "C9gNX+mbr1aYRzgfhMVjROf68nx/i4dAEE6Z4P+HQ2fawVTcplK6jwPBQvElxgyWpduQS0zXvvzFH8L+UxiG777loO1+2iiwdZP11Z0EY3lHNut77fVyU2a7c+Fm7F9AggQy9sgVd+dWXjwMk/sMVaEoKsxSvMHS/ilhBNaeIUslXp5ZZv/ujnbyMIHbHoKFRMtWGy9/K2+qAP3whFSVSq+0w7me9vGLIxa1x5y1TCtSR/tHlFW1X2SuA1mMACqkCNt2lVsB590WM59p3B683tyqaT4LsWZPSTmjr43e7TdP2pG4IEOVd7DWXV9LPYICNi/JR5CKECw6pYrrmIgfzfcJxDG06yTc0CS/IbuLNHD658wMRpeO4+rrS4zS1DdJ0QUXHHpV0hNW/P5QWLcyWjjsr6v8kr/vFRvUX7PSWtWjNA+qFg71wrqx5NAYRkhN/Jl10Qhp97XQZTwGsxFcYu8rsH8gvmpZcSIOpzlYBWFFaDg55NKbTgbgcnGpjpzR6i1S7OYhox159CoD6BBb/cEoORf1Pz3C5UdHl938B+3y+z4FvH+ol1FOoJp7oXEfCM1rlm96C51pl7W92a2qfBShRhZqOQLUfr1jya+g8AHUt9MfLEinhn0WikyPJGfdROFbhlZHzI2DdyyJfZ0+lGlvR2EeMBKyBcC9T4dKUUg=";

			//------------------------------------------------------------------------------------------------------------------------------

			// Create column series for Task
			NChartBarSeries seriesBar = new NChartBarSeries ();
			seriesBar.Brush = new NChartSolidColorBrush (Color.Argb (255, 0, 192, 96));
			seriesBar.DataSource = taskData;
			seriesBar.Tag = 1;

			mNChartView1.Chart.ShouldAntialias = true;
			mNChartView1.Chart.AddSeries (seriesBar);

			mNChartView1.Chart.Legend.Visible = false;

			mNChartView1.Chart.CartesianSystem.XAxis.DataSource = taskData;
			mNChartView1.Chart.CartesianSystem.YAxis.DataSource = taskData;
			mNChartView1.Chart.Delegate = taskData;

			mNChartView1.Chart.CartesianSystem.XAxis.HasOffset = false;
			mNChartView1.Chart.CartesianSystem.YAxis.HasOffset = true;
			mNChartView1.Chart.Caption.Text = "Task";

			mNChartView1.Chart.UpdateData ();
			mNChartView1.Chart.PlayTransition (2, false);

			//------------------------------------------------------------------------------------------------------------------------------

			// Create brushes.
			brushes = new NChartBrush[3];
			brushes [0] = new NChartSolidColorBrush (Color.Argb (255, (int)(0.38 * 255), (int)(0.8 * 255), (int)(0.91 * 255)));
			brushes [1] = new NChartSolidColorBrush (Color.Argb (255, (int)(0.8 * 255), (int)(0.86 * 255), (int)(0.22 * 255)));
			brushes [2] = new NChartSolidColorBrush (Color.Argb (255, (int)(0.9 * 255), (int)(0.29 * 255), (int)(0.51 * 255)));

			// Switch on antialiasing.
			mNChartView2.Chart.ShouldAntialias = true;
			mNChartView2.Chart.CartesianSystem.Margin = new NChartMargin (10.0f, 10.0f, 10.0f, 20.0f);
			mNChartView2.Chart.PolarSystem.Margin = new NChartMargin (10.0f, 10.0f, 10.0f, 20.0f);
			mNChartView2.Chart.Caption.Text = "Project Tasks";
			// Create series that will be displayed on the chart.
			NChartPieSeries series = new NChartPieSeries ();
			series.DataSource = projectData;
			series.Tag = 0;
			series.Brush = brushes [0];
			mNChartView2.Chart.AddSeries (series);

			NChartPieSeries series1 = new NChartPieSeries ();
			series1.DataSource = projectData;
			series1.Tag = 1;
			series1.Brush = brushes [1];
			mNChartView2.Chart.AddSeries (series1);

			NChartPieSeries series2 = new NChartPieSeries ();
			series2.DataSource = projectData;
			series2.Tag = 2;
			series2.Brush = brushes [2];
			mNChartView2.Chart.AddSeries (series2);

			NChartPieSeriesSettings settings = new NChartPieSeriesSettings ();
			settings.HoleRatio = 0.0f;
			mNChartView2.Chart.AddSeriesSettings (settings);

			// Set delegate to the chart.
			mNChartView2.Chart.Delegate = this;
			// Update data in the chart.
			mNChartView2.Chart.UpdateData ();

			mNChartView2.Chart.MinZoom = 0.85f;
			zoomed = false;

			// Uncomment this line to get the animated transition.
			mNChartView2.Chart.PlayTransition(1.0f, false);

			//------------------------------------------------------------------------------------------------------------------

			// Margin to ensure some free space for the iOS status bar.
			mNChartView3.Chart.CartesianSystem.Margin = new NChartMargin (10.0f, 10.0f, 10.0f, 20.0f);

			// Create column series with colors from the array and add them to the chart.
			NChartColumnSeries seriesColumn1 = new NChartColumnSeries ();
			// Set brush that will fill that series with color.
			seriesColumn1.Brush = brushes[1];
			// Set data source for the series.
			seriesColumn1.DataSource = streamData;
			// Add series to the chart.
			mNChartView3.Chart.AddSeries (seriesColumn1);


			mNChartView3.Chart.Legend.Visible = false;
			mNChartView3.Chart.Caption.Text = "Data Real";
			mNChartView3.Chart.DrawIn3D = true;
			// Activate streaming mode.
			mNChartView3.Chart.StreamingMode = true;

			// Prevent minimum and maximum on the axes from "jumping" by activating incremental mode. So the minimum will remain
			// the minimal value ever appeared in the data, and maximum will remain the maximal one.
			mNChartView3.Chart.IncrementalMinMaxMode = true;

			// Update data in the chart.
			mNChartView3.Chart.UpdateData ();

			TimerCallback timerCallback = new TimerCallback (Stream);
			timer = new Timer (timerCallback, mNChartView3.Chart.Series () [mNChartView3.Chart.Series ().Length - 1], 100, 100);

		}

		protected override void OnResume ()
		{
			base.OnResume ();
			mNChartView1.OnResume ();
			mNChartView2.OnResume ();
		}

		protected override void OnPause ()
		{
			base.OnPause ();
			mNChartView1.OnPause ();
			mNChartView2.OnPause ();
		}

		NChartTooltip CreateTooltip ()
		{
			NChartTooltip result = new NChartTooltip ();

			result.Background = new NChartSolidColorBrush (Color.Argb (255, 255, 255, 255));
			result.Background.Opacity = 0.9;
			result.Padding = new NChartMargin (10.0f, 10.0f, 10.0f, 10.0f);
			result.BorderColor = Color.Argb (255, 128, 128, 128);
			result.BorderThickness = 1;
			result.Font = new NChartFont (16);
			result.Visible = false;

			return result;
		}
		void UpdateTooltipText (NChartPoint point)
		{
			point.Tooltip.Text = string.Format ("{0}", point.CurrentState.Value);
		}

		public void TimeIndexChanged (NChart nChart, double v)
		{
			// Do nothing, this demo does not cover the changing of the time index.
		}

		public void DidEndAnimating (NChart nChart, Java.Lang.Object o, NChartAnimationType animationType)
		{
			// Do nothing, this demo requires no catching of animation ending.
		}

		public void PointSelected (NChart nChart, NChartPoint nChartPoint)
		{
			// Disable highlight.
			if (prevSelectedPoint != null) {
				prevSelectedPoint.Highlight (NChartHighlightType.None, 0.25f, 0.0f);
				prevSelectedPoint.Tooltip.SetVisibleAnimated (false, 0.25f);
			}

			if (nChartPoint != null) 
			{
				if (nChartPoint == prevSelectedPoint) 
				{
					Console.WriteLine ("point to same point");
					//if tooltip display . tat tooltip

					prevSelectedPoint = null;

					// Return to normal zoom.
					if (zoomed) 
					{
						zoomed = false;
						mNChartView2.Chart.ZoomTo (1.0f, 0.25f, 0.0f);
					}
				} 
				else 
				{
					Console.WriteLine ("point to diff point");
					//Hien thi tool tip

					prevSelectedPoint = nChartPoint;

					if (nChartPoint.Tooltip != null) {
						UpdateTooltipText (nChartPoint);
						nChartPoint.Tooltip.SetVisibleAnimated (true, 0.25f);
					} 
					else 
					{
						nChartPoint.Tooltip = CreateTooltip ();
						UpdateTooltipText (nChartPoint);
						nChartPoint.Tooltip.SetVisibleAnimated (true, 0.25f);
					}


					if (!zoomed) {
						zoomed = true;
						mNChartView2.Chart.ZoomTo (0.85f, 0.25f, 0.0f);
					}

					// Set shift to highlight.
					nChartPoint.HighlightShift = 0.2f;

					// Set color to highlight.
					nChartPoint.HighlightColor = Color.Red;

					// Highlight point by shift and color.
					nChartPoint.Highlight (NChartHighlightType.Shift | NChartHighlightType.Color, 0.25f, 0.0f);
				}
			} 
			else 
			{
				Console.WriteLine ("point ra ngoai");
				//if tooltip display , tat tooltip

				prevSelectedPoint = null;

				// Return to normal zoom.
				if (zoomed) {
					zoomed = false;
					mNChartView2.Chart.ZoomTo (1.0f, 0.25f, 0.0f);
				}
			}
		}

		void Stream (object series)
		{
			lock (guard) {

				// Begin the data changing session from-within separated thread.
				// Ensure thread-safe changes in the chart by wrapping the updating routine with beginTransaction and
				// endTransaction calls.
				mNChartView3.Chart.BeginTransaction ();

				// Update data in the points.
				NChartPoint[] points = (series as NChartSeries).Points ();
				foreach (NChartPoint point in points) {
					double value = random.NextDouble ();
					point.CurrentState.DoubleY = 30.0 * value;
					int color = (int)(255.0 * value);
					point.CurrentState.Brush = new NChartSolidColorBrush (Color.Argb (255, color, 200, 255 - color));
				}

				// Update data in the chart.
				mNChartView3.Chart.StreamData ();

				// End the data changing session from-within separate thread.
				mNChartView3.Chart.EndTransaction ();
			}
		}

	}


}


