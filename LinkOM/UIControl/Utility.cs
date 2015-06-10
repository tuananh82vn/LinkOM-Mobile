using System;
using System.Linq;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace LinkOM
{
	public class Utility
	{


		public static void SetListViewHeightBasedOnChildren(ListView listView)
		{
			var adapter = listView.Adapter;
			if (adapter == null)
				return;

			var totalHeight = 0;

			for (var i = 0; i < adapter.Count; i++)
			{
				View listItem = adapter.GetView(i, null, listView);

				var Unspecified = View.MeasureSpec.MakeMeasureSpec (0, MeasureSpecMode.Unspecified);

				var Atmost = View.MeasureSpec.MakeMeasureSpec (0, MeasureSpecMode.AtMost);

				var Exactly = View.MeasureSpec.MakeMeasureSpec (0, MeasureSpecMode.Exactly);

				listItem.Measure(Atmost, Unspecified);

				totalHeight += listItem.MeasuredHeight;


			}

//			Console.WriteLine("Total Items height = {0}", adapter.Count);
//			Console.WriteLine("Total height = {0}", totalHeight);


			var layoutParams = listView.LayoutParameters;

			layoutParams.Height = totalHeight -400;

			listView.LayoutParameters = layoutParams;
			listView.RequestLayout();
		}




	}
}