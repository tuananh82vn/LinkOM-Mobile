using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;

using Java.Lang;
using NChart3D_Android;
using Android.Graphics;



namespace LinkOM
{
	
	public class SlidingTabsFragment : Fragment
	{
		private SlidingTabScrollView mSlidingTabScrollView;
		private ViewPager mViewPager;



		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate(Resource.Layout.fragment_sample, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			mSlidingTabScrollView = view.FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
			mViewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);



			mViewPager.Adapter = new SamplePagerAdapter();

			mSlidingTabScrollView.ViewPager = mViewPager;
		}

		public override void OnResume ()
		{
			base.OnResume ();
			//mNChartView1.OnResume ();
			//			mNChartView2.OnResume ();
		}

		public override void OnPause ()
		{
			base.OnPause ();
			//mNChartView1.OnPause ();
			//			mNChartView2.OnPause ();
		}


	}

	public class SamplePagerAdapter : PagerAdapter , INChartDelegate
	{
		public TaskDataSource taskData;
		public ProjectTaskDataSource projectData;

		public NChartView mNChartView1;

		public NChartPoint prevSelectedPoint;

		List<string> items = new List<string>();

		bool zoomed;

		NChartBrush[] brushes;

		public SamplePagerAdapter() : base()
		{
			items.Add("Task");
			items.Add("Ticket");
			items.Add("Project-Task");
			items.Add("Project-Ticket");
			taskData = new TaskDataSource ();
			projectData = new ProjectTaskDataSource ();
		}

		public override int Count
		{
			get { return items.Count; }
		}

		public override bool IsViewFromObject(View view, Java.Lang.Object obj)
		{
			return view == obj;
		}

		public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
		{
			View view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.pager_item, container, false);
			container.AddView(view);

			mNChartView1 =   view.FindViewById<NChartView>(Resource.Id.surface);

			LoadViews (position);

			//				TextView txtTitle = view.FindViewById<TextView>(Resource.Id.item_title);
			//				int pos = position + 1;
			//				txtTitle.Text = pos.ToString();

			return view;
		}

		public string GetHeaderTitle (int position)
		{
			return items[position];
		}

		public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object obj)
		{
			container.RemoveView((View)obj);
		}

		private void LoadViews (int position)
		{
			// Paste your license key here.
			mNChartView1.Chart.LicenseKey = "C9gNX+mbr1aYRzgfhMVjROf68nx/i4dAEE6Z4P+HQ2fawVTcplK6jwPBQvElxgyWpduQS0zXvvzFH8L+UxiG777loO1+2iiwdZP11Z0EY3lHNut77fVyU2a7c+Fm7F9AggQy9sgVd+dWXjwMk/sMVaEoKsxSvMHS/ilhBNaeIUslXp5ZZv/ujnbyMIHbHoKFRMtWGy9/K2+qAP3whFSVSq+0w7me9vGLIxa1x5y1TCtSR/tHlFW1X2SuA1mMACqkCNt2lVsB590WM59p3B683tyqaT4LsWZPSTmjr43e7TdP2pG4IEOVd7DWXV9LPYICNi/JR5CKECw6pYrrmIgfzfcJxDG06yTc0CS/IbuLNHD658wMRpeO4+rrS4zS1DdJ0QUXHHpV0hNW/P5QWLcyWjjsr6v8kr/vFRvUX7PSWtWjNA+qFg71wrqx5NAYRkhN/Jl10Qhp97XQZTwGsxFcYu8rsH8gvmpZcSIOpzlYBWFFaDg55NKbTgbgcnGpjpzR6i1S7OYhox159CoD6BBb/cEoORf1Pz3C5UdHl938B+3y+z4FvH+ol1FOoJp7oXEfCM1rlm96C51pl7W92a2qfBShRhZqOQLUfr1jya+g8AHUt9MfLEinhn0WikyPJGfdROFbhlZHzI2DdyyJfZ0+lGlvR2EeMBKyBcC9T4dKUUg=";
			zoomed = false;

			if (position == 2 || position == 3) {

							// Create brushes.
							brushes = new NChartBrush[3];
							brushes [0] = new NChartSolidColorBrush (Color.Argb (255, (int)(0.38 * 255), (int)(0.8 * 255), (int)(0.91 * 255)));
							brushes [1] = new NChartSolidColorBrush (Color.Argb (255, (int)(0.8 * 255), (int)(0.86 * 255), (int)(0.22 * 255)));
							brushes [2] = new NChartSolidColorBrush (Color.Argb (255, (int)(0.9 * 255), (int)(0.29 * 255), (int)(0.51 * 255)));
				
							// Switch on antialiasing.
							mNChartView1.Chart.ShouldAntialias = true;
				mNChartView1.Chart.CartesianSystem.Margin = new NChartMargin (10.0f, 10.0f, 10.0f, 20.0f);
				mNChartView1.Chart.PolarSystem.Margin = new NChartMargin (10.0f, 10.0f, 10.0f, 20.0f);
				mNChartView1.Chart.Caption.Text = "Project Tasks";
							// Create series that will be displayed on the chart.
							NChartPieSeries series = new NChartPieSeries ();
							series.DataSource = projectData;
							series.Tag = 0;
							series.Brush = brushes [0];
				mNChartView1.Chart.AddSeries (series);
				
							NChartPieSeries series1 = new NChartPieSeries ();
							series1.DataSource = projectData;
							series1.Tag = 1;
							series1.Brush = brushes [1];
				mNChartView1.Chart.AddSeries (series1);
				
							NChartPieSeries series2 = new NChartPieSeries ();
							series2.DataSource = projectData;
							series2.Tag = 2;
							series2.Brush = brushes [2];
				mNChartView1.Chart.AddSeries (series2);
				
							NChartPieSeriesSettings settings = new NChartPieSeriesSettings ();
							settings.HoleRatio = 0.0f;
				mNChartView1.Chart.AddSeriesSettings (settings);
				
							// Set delegate to the chart.
				mNChartView1.Chart.Delegate = this;
							// Update data in the chart.
				mNChartView1.Chart.UpdateData ();
				
				mNChartView1.Chart.MinZoom = 0.85f;
							zoomed = false;
				
							// Uncomment this line to get the animated transition.
				mNChartView1.Chart.PlayTransition(1.0f, false);
				
			} else {
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
			}
		}

		public void TimeIndexChanged (NChart nChart, double v)
		{
			// Do nothing, this demo does not cover the changing of the time index.
		}

		public void DidEndAnimating (NChart nChart, Java.Lang.Object o, NChartAnimationType animationType)
		{
			// Do nothing, this demo requires no catching of animation ending.
		}

		void UpdateTooltipText (NChartPoint point)
		{
			point.Tooltip.Text = string.Format ("{0}", point.CurrentState.Value);
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
											mNChartView1.Chart.ZoomTo (1.0f, 0.25f, 0.0f);
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
						mNChartView1.Chart.ZoomTo (0.85f, 0.25f, 0.0f);
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
					mNChartView1.Chart.ZoomTo (1.0f, 0.25f, 0.0f);
				}
			}
		}
	}
}