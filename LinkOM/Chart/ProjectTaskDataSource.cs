
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
	public class ProjectTaskDataSource : Activity, INChartSeriesDataSource
	{
		Random random = new Random ();

		public NChartPoint[] Points (NChartSeries series)
		{
			// Create points with some data for the series.
			NChartPoint[] result = new NChartPoint[1];
			result [0] = new NChartPoint (NChartPointState.PointStateWithCircleValue (0, random.Next (30) + 1), series);
			return result;
		}

		public Bitmap Image (NChartSeries series)
		{
			return null;
		}

		public string Name (NChartSeries series)
		{
			if (series.Tag == 0) {
				return string.Format ("Open");
			} else if (series.Tag == 1) {
				return string.Format ("Close");
			} else if (series.Tag == 2) {
				return string.Format ("Waiting");
			} else
				return "";
		}

	}
}