using System;
using Android.Graphics;

namespace LinkOM
{
	public static class ColorHelper
	{
		public static Color GetColor(string ColorName){
			try
			{
				if (ColorName.Equals("orange")) return Color.Orange;
				else
					if (ColorName.Equals("pink")) return Color.Pink;
					else
						if (ColorName.Equals("purple")) return Color.Purple;
							else return Color.ParseColor(ColorName);
			}
			catch(Exception)
			{
				return Color.Black;
			}
		}

	}
}

