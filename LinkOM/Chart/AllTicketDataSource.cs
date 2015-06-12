
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
using System.Collections.Generic;
using System.Linq;

namespace LinkOM
{
	public class AllTicketDataSource : Activity, INChartSeriesDataSource
	{
		Random random = new Random ();

		private int _Number=0;
		private string _Name;
		public AllTicketDataSource(int Number,string Name)
		{
			_Number = Number;
			_Name = Name;
		}

		public NChartPoint[] Points (NChartSeries series)
		{
			 //Create points with some data for the series.
			NChartPoint[] result = new NChartPoint[1];
			result [0] = new NChartPoint (NChartPointState.PointStateWithCircleValue (0, _Number), series);
			return result;
		}

		public Bitmap Image (NChartSeries series)
		{
			return null;
		}

		public string Name (NChartSeries series)
		{
			return _Name;
		}

	}
}