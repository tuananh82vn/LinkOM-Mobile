using System;
using System.Threading;

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
	public class StreamingDataSource : Activity, INChartSeriesDataSource
	{
		Random random = new Random ();

		public StreamingDataSource ()
		{
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

