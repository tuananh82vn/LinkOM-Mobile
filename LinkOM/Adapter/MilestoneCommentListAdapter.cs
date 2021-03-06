﻿using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Webkit;
using Android.Text;
using Android.Graphics;

namespace LinkOM
{
	public class MilestoneCommentListAdapter : BaseAdapter
	{
		List<MilestonesCommentList> _MilestoneCommentObject;

		Activity _activity;

		public MilestoneCommentListAdapter (Activity activity, List<MilestonesCommentList> data)
		{
			_activity = activity;
			_MilestoneCommentObject = data;
		}

		public override int Count 
		{
			get 
				{  
					if (_MilestoneCommentObject == null)
					{
						return 0;
					}
					return _MilestoneCommentObject.Count; 
				}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public MilestonesCommentList GetItemAtPosition(int position)
		{
			return _MilestoneCommentObject[position];
		}

		public override long GetItemId (int position) {
			return 0;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.CommentList, parent, false);

			var Name = view.FindViewById<WebView> (Resource.Id.tv_Name);
			var msg =_MilestoneCommentObject [position].Comment.Trim();
			Name.LoadData (Html.FromHtml(msg).ToString(), "text/html", "utf8");
			Name.SetBackgroundColor(Color.Argb(1, 0, 0, 0));
			WebSettings webSettings = Name.Settings;
			webSettings.DefaultFontSize = 12;

			var CreatedPerson = view.FindViewById<TextView> (Resource.Id.tv_CreatedPerson);
			CreatedPerson.Text = _MilestoneCommentObject [position].UserName.Trim();

			var CommentDate = view.FindViewById<TextView> (Resource.Id.tv_CommentDate);
			CommentDate.Text = _MilestoneCommentObject [position].CreatedDate.Value.ToString("dd/MM/yyyy  HH:mm:ss");


			return view;
		}
	}
}

