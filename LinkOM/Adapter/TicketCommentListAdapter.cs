using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;

namespace LinkOM
{
	public class TicketCommentListAdapter : BaseAdapter
	{
		List<TicketCommentList> _TicketCommentObject;

		Activity _activity;

		public TicketCommentListAdapter (Activity activity, List<TicketCommentList> data)
		{
			_activity = activity;
			_TicketCommentObject = data;
		}

		public override int Count 
		{
			get 
				{  
					if (_TicketCommentObject == null)
					{
						return 0;
					}
					return _TicketCommentObject.Count; 
				}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public TicketCommentList GetItemAtPosition(int position)
		{
			return _TicketCommentObject[position];
		}

		public override long GetItemId (int position) {
			return 0;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.CommentList, parent, false);

			var Name = view.FindViewById<TextView> (Resource.Id.tv_Name);
			Name.Text = _TicketCommentObject [position].Comment.Trim();

			var CreatedPerson = view.FindViewById<TextView> (Resource.Id.tv_CreatedPerson);
			CreatedPerson.Text = _TicketCommentObject [position].UserName.Trim();

			var CommentDate = view.FindViewById<TextView> (Resource.Id.tv_CommentDate);
			CommentDate.Text = _TicketCommentObject [position].CreatedDate.Value.ToString("dd/MM/yyyy  HH:mm:ss");




			return view;
		}
	}
}

