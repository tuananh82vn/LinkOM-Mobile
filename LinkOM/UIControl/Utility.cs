using System;
using System.Linq;
using Android.Views;
using Android.Widget;

namespace LinkOM
{
	public class Utility
	{
		public static void setListViewHeightBasedOnChildren (ListView listView)
		{
			if (listView.Adapter == null) {
				// pre-condition
				return;
			}

			int totalHeight = listView.PaddingTop + listView.PaddingBottom;

			for (int i = 0; i < listView.Count; i++) 
			{
				View listItem = listView.Adapter.GetView (i, null, listView);

				listItem.LayoutParameters = new LinearLayout.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

				listItem.Measure (0, 0);

				totalHeight += listItem.MeasuredHeight;
			}


			listView.LayoutParameters.Height = totalHeight + (listView.DividerHeight * (listView.Count - 1)) ;
		}

		public static void changeHeight (ListView listView, int height)
		{
			listView.LayoutParameters.Height = height ;
		}
	}
}