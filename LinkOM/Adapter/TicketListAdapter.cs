using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Java.Lang;
using Object = Java.Lang.Object; 
using System.Linq;

namespace LinkOM
{
	public class TicketListAdapter : BaseAdapter<TicketList>, IFilterable
	{
		private List<TicketList> _originalData;
		private List<TicketList> _TicketList;

		Activity _activity;

		public Filter Filter { get; private set; }

		public TicketListAdapter (Activity activity, List<TicketList> data)
		{
			_activity = activity;
			_TicketList = data;

			Filter = new TicketFilter(this);
		}

		public override TicketList this[int position]
		{
			get { return _TicketList[position]; }
		} 

		public override int Count 
		{
			get 
			{  
				if (_TicketList == null)
				{
					return 0;
				}
				return _TicketList.Count; 
			}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public TicketList GetItemAtPosition(int position)
		{
			return _TicketList[position];
		}

		public override void NotifyDataSetChanged()
		{
			// If you are using cool stuff like sections
			// remember to update the indices here!
			base.NotifyDataSetChanged();
		} 


		public override long GetItemId (int position) {
			return long.Parse(_TicketList [position].Id.ToString());
		}

		public string GetItemName (int position) {
			return _TicketList [position].Title;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.TicketList, parent, false);

			var TicketName = view.FindViewById<TextView> (Resource.Id.tv_TicketName);
			TicketName.Text = _TicketList [position].Title;

			var ProjectName = view.FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = _TicketList [position].ProjectName;

			var Priority = view.FindViewById<ImageView> (Resource.Id.image_Priority);
			if(_TicketList [position].PriorityName.Equals("High"))
				Priority.SetImageResource(Resource.Drawable.hight_priority);
			else if(_TicketList [position].PriorityName.Equals("Medium"))
				Priority.SetImageResource(Resource.Drawable.medium_priority);
			else if(_TicketList [position].PriorityName.Equals("Low"))
				Priority.SetImageResource(Resource.Drawable.low_priority);

			return view;
		}

		private class TicketFilter : Filter
		{
			private readonly TicketListAdapter _adapter;
			public TicketFilter(TicketListAdapter adapter)
			{
				_adapter = adapter;
			}

			protected override FilterResults PerformFiltering(ICharSequence constraint)
			{
				var returnObj = new FilterResults();

				var results = new List<TicketList>();

				if (_adapter._originalData == null)
					_adapter._originalData = _adapter._TicketList; 

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
					_adapter._TicketList = values.ToArray<Object>()
						.Select(r => r.ToNetObject<TicketList>()).ToList();
				_adapter.NotifyDataSetChanged();

				// Don't do this and see GREF counts rising
				constraint.Dispose();
				results.Dispose();
			}
		} 
	}
}

