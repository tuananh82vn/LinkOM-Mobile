using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;

namespace LinkOM
{
	public class TicketCommentListAdapter : BaseAdapter
	{
		List<TicketCommentObject> _TicketCommentObject;

		Activity _activity;

		public TicketCommentListAdapter (Activity activity, List<TicketCommentObject> data)
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

		public TicketCommentObject GetItemAtPosition(int position)
		{
			return _TicketCommentObject[position];
		}

		public override long GetItemId (int position) {
			return 0;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.TicketCommentList, parent, false);

			var MilestoneName = view.FindViewById<TextView> (Resource.Id.tv_MilestoneName);
			MilestoneName.Text = _TicketCommentObject [position].Comment;

			var CreatedPerson = view.FindViewById<TextView> (Resource.Id.tv_CreatedPerson);
			CreatedPerson.Text = _TicketCommentObject [position].OwnerName;

			var CommentDate = view.FindViewById<TextView> (Resource.Id.tv_CommentDate);
			CommentDate.Text = _TicketCommentObject [position].CreatedDate.Value.ToShortDateString();


			return view;
		}
	}
}

