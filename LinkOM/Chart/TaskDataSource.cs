
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
	public class TaskDataSource : Activity, INChartSeriesDataSource , INChartValueAxisDataSource, INChartDelegate
	{
		Random random = new Random ();

		NChartPoint prevSelectedPoint;

		public string Name (NChartSeries series)
		{
			return "Task";
		}


		public string Name (NChartValueAxis nChartValueAxis) {
			if (nChartValueAxis.Kind == NChartValueAxisKind.X)
				return "";
			else if (nChartValueAxis.Kind == NChartValueAxisKind.Y)
				return "Status";
			return null;
		}

		public string[] Ticks (NChartValueAxis nChartValueAxis)
		{
			if (nChartValueAxis.Kind == NChartValueAxisKind.Y)
				return new string[] { "Open", "Closed", "Waiting", "Hold", "Rejected" };
			return null;
		}

		public NChartPoint[] Points (NChartSeries series)
		{
			NChartPoint[] result = new NChartPoint[5];
			for (int i = 0; i <= 4; ++i)
				result [i] = new NChartPoint (NChartPointState.PointStateAlignedToYWithXY (random.Next (30) + 1, i), series);

			return result;
		}

		public Bitmap Image (NChartSeries series)
		{
			return null;
		}

		public Number Min (NChartValueAxis nChartValueAxis)
		{
			return null;
		}

		public Number Max (NChartValueAxis nChartValueAxis)
		{
			return null;
		}

		public Number Step (NChartValueAxis nChartValueAxis)
		{
			return null;
		}

		public Number Length (NChartValueAxis nChartValueAxis) {
			return null;
		}

		public string DoubleToString (double v, NChartValueAxis nChartValueAxis) {
			return null;
		}

		public void PointSelected (NChart nChart, NChartPoint nChartPoint)
		{
			if (prevSelectedPoint != null)
				prevSelectedPoint.Tooltip.SetVisibleAnimated (false, 0.25f);

			if (nChartPoint != null) {
				if (nChartPoint.Tooltip != null) {
					if (nChartPoint == prevSelectedPoint)
						prevSelectedPoint = null;
					else {
						prevSelectedPoint = nChartPoint;
						UpdateTooltipText (nChartPoint);
						nChartPoint.Tooltip.SetVisibleAnimated (true, 0.25f);
					}
				} else {
					prevSelectedPoint = nChartPoint;
					nChartPoint.Tooltip = CreateTooltip ();
					UpdateTooltipText (nChartPoint);
					nChartPoint.Tooltip.SetVisibleAnimated (true, 0.25f);
				}
			} else
				prevSelectedPoint = null;
		}

		public void TimeIndexChanged (NChart chart, double timeIndex)
		{
		}

		public void DidEndAnimating (NChart nChart, Java.Lang.Object o, NChartAnimationType animationType) { }

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

		void UpdateTooltipText(NChartPoint point)
		{
			point.Tooltip.Text = string.Format ("{0} : {1}", point.Series.Name, point.CurrentState.DoubleX);
		}

	}
}