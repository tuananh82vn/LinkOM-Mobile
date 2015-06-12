using System;
using Android.Webkit;
using Android.Widget;

namespace LinkOM
{
	public class MyViewHolder : Java.Lang.Object
	{
		public WebView Name { get; set; }
		public TextView CreatedPerson { get; set; }
		public TextView CommentDate { get; set; }
	}
}

