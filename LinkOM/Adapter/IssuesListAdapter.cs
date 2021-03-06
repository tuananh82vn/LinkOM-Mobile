﻿using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Java.Lang;
using Object = Java.Lang.Object; 
using System.Linq;

namespace LinkOM
{
	public class IssuesListAdapter : BaseAdapter<IssuesList>, IFilterable
	{
		private List<IssuesList> _originalData;
		private List<IssuesList> _IssuesList;

		Activity _activity;

		public Filter Filter { get; private set; }

		public IssuesListAdapter (Activity activity, List<IssuesList> data)
		{
			_activity = activity;
			_IssuesList = data;

			Filter = new IssuesFilter(this);
		}

		public override IssuesList this[int position]
		{
			get { return _IssuesList[position]; }
		} 

		public override int Count 
		{
			get 
			{  
				if (_IssuesList == null)
				{
					return 0;
				}
				return _IssuesList.Count; 
			}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public IssuesList GetItemAtPosition(int position)
		{
			return _IssuesList[position];
		}

		public override void NotifyDataSetChanged()
		{
			// If you are using cool stuff like sections
			// remember to update the indices here!
			base.NotifyDataSetChanged();
		} 


		public override long GetItemId (int position) {
			return long.Parse(_IssuesList [position].Id.ToString());
		}

		public string GetItemName (int position) {
			return _IssuesList [position].Title;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.IssuesList, parent, false);

			var Title = view.FindViewById<TextView> (Resource.Id.tv_IssuesName);
			Title.Text = _IssuesList [position].Title;
//
//
//			var TaskCode = view.FindViewById<TextView> (Resource.Id.tv_Code);
//			TaskCode.Text = _IssuesList [position].Code;

			var ProjectName = view.FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = _IssuesList [position].ProjectName;

//			var StartDate = view.FindViewById<TextView> (Resource.Id.tv_StartDate);
//			StartDate.Text = _IssuesList [position].StartDateString;

//			var EndDate = view.FindViewById<TextView> (Resource.Id.tv_EndDate);
//			EndDate.Text = _IssuesList [position].EndDateString;

//			var ActualHours = view.FindViewById<TextView> (Resource.Id.tv_ActualHours);
//			ActualHours.Text = _IssuesList [position].ActHours;

//			var AllocatedHours = view.FindViewById<TextView> (Resource.Id.tv_AllocatedHours);
//			AllocatedHours.Text = _IssuesList [position].AllocatedHours.ToString();

//			var AssignTo = view.FindViewById<TextView> (Resource.Id.tv_AssignTo);
//			AssignTo.Text = _IssuesList [position].AssignedTo;

//			var Owner = view.FindViewById<TextView> (Resource.Id.tv_Owner);
//			Owner.Text = _IssuesList [position].Owner;
//
//			var Status = view.FindViewById<TextView> (Resource.Id.tv_Status);
//			Status.Text = _IssuesList [position].TicketStatusName;

			var Priority = view.FindViewById<ImageView> (Resource.Id.image_Priority);
			if(_IssuesList [position].PriorityName.Equals("High"))
				Priority.SetImageResource(Resource.Drawable.hight_priority);
//			else if(_IssuesList [position].PriorityName.Equals("Medium"))
//				Priority.SetImageResource(Resource.Drawable.medium_priority);
			else if(_IssuesList [position].PriorityName.Equals("Low"))
				Priority.SetImageResource(Resource.Drawable.low_priority);

			return view;
		}

		private class IssuesFilter : Filter
		{
			private readonly IssuesListAdapter _adapter;
			public IssuesFilter(IssuesListAdapter adapter)
			{
				_adapter = adapter;
			}

			protected override FilterResults PerformFiltering(ICharSequence constraint)
			{
				var returnObj = new FilterResults();

				var results = new List<IssuesList>();

				if (_adapter._originalData == null)
					_adapter._originalData = _adapter._IssuesList; 

				if (constraint == null) return returnObj;

				if (_adapter._originalData != null && _adapter._originalData.Any())
				{
					// Compare constraint to all names lowercased.
					// It they are contained they are added to results.


					results.AddRange(_adapter._originalData.Where(t => t.Title.ToLower().Contains(constraint.ToString().ToLower())));
				}

				// Nasty piece of .NET to Java wrapping, be careful with this!
				returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
				returnObj.Count = results.Count;

				constraint.Dispose();

				return returnObj;
			}

			protected override void PublishResults(ICharSequence constraint, FilterResults results)
			{
				using (var values = results.Values)
					_adapter._IssuesList = values.ToArray<Object>()
						.Select(r => r.ToNetObject<IssuesList>()).ToList();
				_adapter.NotifyDataSetChanged();

				// Don't do this and see GREF counts rising
				constraint.Dispose();
				results.Dispose();
			}
		} 
	}
}

