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
	public class IssuesCommentListAdapter : BaseAdapter
	{
		List<IssuesCommentList> _CommentObject;

		Activity _activity;

		private int Height;

		public IssuesCommentListAdapter (Activity activity, List<IssuesCommentList> data)
		{
			_activity = activity;
			_CommentObject = data;
		}

		public override int Count 
		{
			get 
				{  
					if (_CommentObject == null)
					{
						return 0;
					}
					return _CommentObject.Count; 
				}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public IssuesCommentList GetItemAtPosition(int position)
		{
			return _CommentObject[position];
		}

		public override long GetItemId (int position) {
			return 0;
		}

		public int GetHeight () {
			return Height;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			MyViewHolder holder = null;

			if(convertView != null)
				holder = convertView.Tag as MyViewHolder;

			if (holder == null) {

				holder = new MyViewHolder();

				convertView = _activity.LayoutInflater.Inflate (Resource.Layout.CommentList, parent, false);

				holder.Name = convertView.FindViewById<WebView> (Resource.Id.tv_Name);
				holder.CreatedPerson = convertView.FindViewById<TextView> (Resource.Id.tv_CreatedPerson);
				holder.CommentDate = convertView.FindViewById<TextView> (Resource.Id.tv_CommentDate);

				convertView.Tag = holder;
			}

			var msg = _CommentObject [position].Comment.Trim ();
			holder.Name.LoadData (Html.FromHtml (msg).ToString (), "text/html", "utf8");
			holder.Name.SetBackgroundColor (Color.Argb (1, 0, 0, 0));
			WebSettings webSettings = holder.Name.Settings;
			webSettings.DefaultFontSize = 12;

			Height += Utility.CalcHeight (Html.FromHtml (msg).ToString ());

			holder.CreatedPerson.Text = _CommentObject [position].UserName.Trim ();
			holder.CommentDate.Text = _CommentObject [position].CreatedDate.Value.ToString ("dd/MM/yyyy  HH:mm:ss");

			return convertView;
		}
	}
}

