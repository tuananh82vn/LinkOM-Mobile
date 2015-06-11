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
	public class DocumentCommentListAdapter : BaseAdapter
	{
		List<DocumentCommentObject> _DocumentCommentObject;

		Activity _activity;

		private int Height;

		public DocumentCommentListAdapter (Activity activity, List<DocumentCommentObject> data)
		{
			_activity = activity;
			_DocumentCommentObject = data;
		}

		public override int Count 
		{
			get 
				{  
					if (_DocumentCommentObject == null)
					{
						return 0;
					}
					return _DocumentCommentObject.Count; 
				}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public DocumentCommentObject GetItemAtPosition(int position)
		{
			return _DocumentCommentObject[position];
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
			var msg = _DocumentCommentObject [position].DocumentVersion;

			Name.LoadData (Html.FromHtml(msg).ToString(), "text/html", "utf8");
			Name.SetBackgroundColor(Color.Argb(1, 0, 0, 0));
			WebSettings webSettings = Name.Settings;
			webSettings.DefaultFontSize = 12;
			webSettings.SetSupportZoom (true);


			var CreatedPerson = view.FindViewById<TextView> (Resource.Id.tv_CreatedPerson);
			CreatedPerson.Text = _DocumentCommentObject [position].PublishBy.Trim();

			var CommentDate = view.FindViewById<TextView> (Resource.Id.tv_CommentDate);
			CommentDate.Text = _DocumentCommentObject [position].CreatedDate.Value.ToString("dd/MM/yyyy  HH:mm:ss");

			return view;
		}

	}
}

