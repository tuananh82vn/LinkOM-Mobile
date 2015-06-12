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
		}

		public override void OnPause ()
		{
			base.OnPause ();
		}


	}

	public class SamplePagerAdapter : PagerAdapter , INChartDelegate
	{
		public TaskDataSource taskData;

		public TicketDataSource ticketData;

		public AllTicketDataSource projectData;

		public NChartView mNChartView1;

		public NChartPoint prevSelectedPoint;

		List<string> items = new List<string>();

		bool zoomed;

		public List<NChartBrush> brushes;

		public List<ProjectTicketChart>  TicketDataList;

		public List<ProjectTaskChart>  TaskDataList;

		public List<NChartPieSeries> PieSeries;

		public SamplePagerAdapter() : base()
		{
			items.Add("Task");
			items.Add("Ticket");
			items.Add("Project-Task");
			items.Add("Project-Ticket");

			taskData = new TaskDataSource ();
			ticketData = new TicketDataSource ();

			TicketDataList = ChartHelper.GetProjectTicketChart ();
			TicketDataList = TicketDataList.Where (m => m.TotalTicket > 0).ToList();

			TaskDataList = ChartHelper.GetProjectTaskChart ();
			TaskDataList = TaskDataList.Where (m => m.TotalTask > 0).ToList();

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
			mNChartView1.Chart.LicenseKey = "ditUhAZF+5gJuODAS70r3S6LgTqlYsCjZJUALXaHlQ/T8JVbfGPwtJAAwLXJr2CUzT7Xb+/nz9iQUT3ePqNMh9MIoeKauM6jMUciLBMObrpLcaFeyAMQnQgSPyHhAv6pHI84ZyFLcigU/6a1ZOWVsHEUYcls7u0PqHdx3GoMjJ7ZAC0Sgtzinter7yvzwVlGr5pwnK2IZp9bYGJ7QXgh3/tyNsY6RPn9pGa5zq6OPe3JNuGvAmnYeymrpGIcAZIieLkQfS2hWGWOuovsqIgPyzvX3HW+BPYIYg9l8OP05Ff7sTbBP04y5BJB5DAiWe9xMythAOk6+GXLni1Khtxutr/OyZXF69TZQZY92ydYa7g3rJton68/8120gXIKx+0wqPpEHJuaIrbyd8qWgzvyrKJTf4hjle8GRIINMKMyxlnhyzMns5U/VPwWhTgke+7UtQXtCzeXX2hCEp5Sf6zxY7+AEPXWHGSaZWIOjqiT7uAGmh/TeKq4KxPxB0B5aeODKnWvZ72vWmxZXLUm0XGWg0s1DscoOqhogrrGNQNtbyvOaUxVjI5GGU9tX8OU8P3TDwd0LI8RdCMuBU7nKmJxGz916ryKWrqEdv12dsu9sk9ZEKxjelYTQdQRG6HQSfJZvOo0TB6z3QHNIXZ14lA+vnUilIFolSl9FHdYX26SlRk=";
			zoomed = false;

			if (position == 0 || position == 1) {
					// Create column series for Task
					NChartBarSeries seriesBar1 = new NChartBarSeries ();
					seriesBar1.Brush = new NChartSolidColorBrush (Color.Argb (255, 0, 192, 96));

					mNChartView1.Chart.RemoveAllSeries ();

					if (position == 0) {
						seriesBar1.DataSource = taskData;
						seriesBar1.Tag = 0;

						mNChartView1.Chart.CartesianSystem.XAxis.DataSource = taskData;
						mNChartView1.Chart.CartesianSystem.YAxis.DataSource = taskData;
						mNChartView1.Chart.Delegate = taskData;
					}
					else
					if (position == 1) {
						seriesBar1.DataSource = ticketData;
						seriesBar1.Tag = 1;

						mNChartView1.Chart.CartesianSystem.XAxis.DataSource = ticketData;
						mNChartView1.Chart.CartesianSystem.YAxis.DataSource = ticketData;
						mNChartView1.Chart.Delegate = ticketData;
					}
				
					mNChartView1.Chart.AddSeries (seriesBar1);

					mNChartView1.Chart.ShouldAntialias = true;
					mNChartView1.Chart.CartesianSystem.XAxis.LineVisible = false;
					mNChartView1.Chart.CartesianSystem.XAlongY.Visible = false;

					mNChartView1.Chart.CartesianSystem.BorderVisible = false;
					mNChartView1.Chart.CartesianSystem.YAlongX.Color = Color.WhiteSmoke;

					mNChartView1.Chart.Legend.Visible = false;
					mNChartView1.Chart.CartesianSystem.XAxis.MinTickSpacing = 0.0f;

					mNChartView1.Chart.CartesianSystem.XAxis.HasOffset = false;
					mNChartView1.Chart.CartesianSystem.YAxis.HasOffset = true;

					mNChartView1.Chart.UpdateData ();
					mNChartView1.Chart.PlayTransition (1, false);
			}
			else

			if (position == 2 || position == 3) {

				// Switch on antialiasing.
				mNChartView1.Chart.ShouldAntialias = true;
				mNChartView1.Chart.RemoveAllSeries ();
				mNChartView1.Chart.CartesianSystem.Margin = new NChartMargin (10.0f, 10.0f, 10.0f, 20.0f);
				mNChartView1.Chart.PolarSystem.Margin = new NChartMargin (10.0f, 10.0f, 10.0f, 20.0f);
				mNChartView1.Chart.CartesianSystem.XAlongY.Color = Color.White;
				mNChartView1.Chart.CartesianSystem.YAlongX.Color = Color.Gray;
				
				if (position == 3) {
					// Create brushes.
					brushes = new List<NChartBrush> (TicketDataList.Count);

					for (int i = 0; i < TicketDataList.Count; i++) {
						NChartBrush tempBursh = new NChartSolidColorBrush (ColorHelper.GetColor (TicketDataList [i].TicketStatusColor));

						NChartPieSeries series = new NChartPieSeries ();
						series.DataSource = new AllTicketDataSource (TicketDataList [i].TotalTicket.Value, TicketDataList [i].StatusName);
						series.Tag = i;
						series.Brush = tempBursh;
						mNChartView1.Chart.AddSeries (series);
					}
				}

				if (position == 2) {
					// Create brushes.
					brushes = new List<NChartBrush> (TaskDataList.Count);

						for (int i = 0; i < TaskDataList.Count; i++) {
							NChartBrush tempBursh = new NChartSolidColorBrush (ColorHelper.GetColor (TaskDataList [i].TaskStatusColor));

						NChartPieSeries series = new NChartPieSeries ();
							series.DataSource = new AllTicketDataSource (TaskDataList [i].TotalTask.Value, TaskDataList [i].StatusName);
						series.Tag = i;
						series.Brush = tempBursh;
						mNChartView1.Chart.AddSeries (series);
					}
				}

				
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