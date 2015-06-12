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


				var height = listItem.MeasuredHeight;

				totalHeight += listItem.MeasuredHeight;
			}

			var layoutParams = listView.LayoutParameters;

			layoutParams.Height = totalHeight;

			listView.LayoutParameters = layoutParams;
			listView.RequestLayout();
		}

		public static void SetListViewHeightBasedOnChildren2(ListView listView, int Height)
		{

			Console.WriteLine ("--Set List View Height Based On Children 2--");

			var adapter = listView.Adapter;
			if (adapter == null)
				return;

			var layoutParams = listView.LayoutParameters;

			layoutParams.Height = Height;

			listView.LayoutParameters = layoutParams;

			listView.RequestLayout();
		}

		public static int CalcHeight(string temp){
			int Height = 0;
			int Length = 1;
			double du = 0;

			if (temp.Length > 50) {
				Length = temp.Length / 50;
				du = temp.Length % 50;
				if (du > 0) {
					Length = Length + 1;
				}
			}

			Height += (Length * 70) + 50;
			return Height;

		}




	}
}