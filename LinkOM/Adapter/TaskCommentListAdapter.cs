using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Text;
using Android.Webkit;
using Android.Graphics;

namespace LinkOM
{
	public class TaskCommentListAdapter : BaseAdapter
	{
		List<TaskCommentObject> _TaskCommentObject;

		Activity _activity;

		private int Height;

		public TaskCommentListAdapter (Activity activity, List<TaskCommentObject> data)
		{
			_activity = activity;
			_TaskCommentObject = data;
		}

		public override int Count 
		{
			get 
				{  
					if (_TaskCommentObject == null)
					{
						return 0;
					}
					return _TaskCommentObject.Count; 
				}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public TaskCommentObject GetItemAtPosition(int position)
		{
			return _TaskCommentObject[position];
		}

		public override long GetItemId (int position) {
			return 0;
		}

		public int GetHeight () {
			return Height;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{

			var view = convertView;

			view = _activity.LayoutInflater.Inflate (Resource.Layout.CommentList, parent, false);

			var Name = view.FindViewById<WebView> (Resource.Id.tv_Name);
			var msg =_TaskCommentObject [position].Comment.Trim();
			Name.LoadData (Html.FromHtml(msg).ToString(), "text/html", "utf8");
			Name.SetBackgroundColor(Color.Argb(1, 0, 0, 0));
			WebSettings webSettings = Name.Settings;
			webSettings.DefaultFontSize = 12;

			Height += Utility.CalcHeight (Html.FromHtml (msg).ToString ());

			var CreatedPerson = view.FindViewById<TextView> (Resource.Id.tv_CreatedPerson);
			CreatedPerson.Text = _TaskCommentObject [position].UserName.Trim();

			var CommentDate = view.FindViewById<TextView> (Resource.Id.tv_CommentDate);
			CommentDate.Text = _TaskCommentObject [position].CreatedDate.Value.ToString("dd/MM/yyyy  HH:mm:ss");

			return view;
		}

	}
}

