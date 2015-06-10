using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Text;

namespace LinkOM
{
	public class IssuesCommentListAdapter : BaseAdapter
	{
		List<IssuesCommentList> _IssuesCommentObject;

		Activity _activity;

		public IssuesCommentListAdapter (Activity activity, List<IssuesCommentList> data)
		{
			_activity = activity;
			_IssuesCommentObject = data;
		}

		public override int Count 
		{
			get 
				{  
					if (_IssuesCommentObject == null)
					{
						return 0;
					}
					return _IssuesCommentObject.Count; 
				}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public IssuesCommentList GetItemAtPosition(int position)
		{
			return _IssuesCommentObject[position];
		}

		public override long GetItemId (int position) {
			return 0;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.CommentList, parent, false);

			var Name = view.FindViewById<TextView> (Resource.Id.tv_Name);
			//Name.Text = Html.FromHtml(_TaskCommentObject [position].Comment.Trim()).ToString();
			var msg =_IssuesCommentObject [position].Comment.Trim();
			Name.TextFormatted = Html.FromHtml(msg);

			var CreatedPerson = view.FindViewById<TextView> (Resource.Id.tv_CreatedPerson);
			CreatedPerson.Text = _IssuesCommentObject [position].UserName.Trim();

			var CommentDate = view.FindViewById<TextView> (Resource.Id.tv_CommentDate);
			CommentDate.Text = _IssuesCommentObject [position].CreatedDate.Value.ToString("dd/MM/yyyy  HH:mm:ss");


			return view;
		}
	}
}

