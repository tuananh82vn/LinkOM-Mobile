using System;
using Android.Graphics;

namespace LinkOM
{
	public static class CheckLoginHelper
	{
		public static bool CheckLogin(){
			try
			{
				var token = Settings.Token;
				if(token.Equals(""))
					return false;
				else
					return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

	}
}

