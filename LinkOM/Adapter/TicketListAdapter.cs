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
	public class TicketListAdapter : BaseAdapter<TicketObject>, IFilterable
	{
		private List<TicketObject> _originalData;
		private List<TicketObject> _TicketList;

		Activity _activity;

		public Filter Filter { get; private set; }

		public TicketListAdapter (Activity activity, List<TicketObject> data)
		{
			_activity = activity;
			_TicketList = data;

			Filter = new TicketFilter(this);
		}

		public override TicketObject this[int position]
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

		public TicketObject GetItemAtPosition(int position)
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
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.Ticket, parent, false);

//			var TaskTitle = view.FindViewById<TextView> (Resource.Id.tv_TaskName);
//			TaskTitle.Text = _TicketList [position].Title;


//			var TaskCode = view.FindViewById<TextView> (Resource.Id.tv_Code);
//			TaskCode.Text = _TicketList [position].Code;

			var ProjectName = view.FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = _TicketList [position].ProjectName;

//			var StartDate = view.FindViewById<TextView> (Resource.Id.tv_StartDate);
//			StartDate.Text = _TicketList [position].StartDateString;
//
//			var EndDate = view.FindViewById<TextView> (Resource.Id.tv_EndDate);
//			EndDate.Text = _TicketList [position].EndDateString;
//
//			var ActualHours = view.FindViewById<TextView> (Resource.Id.tv_ActualHours);
//			ActualHours.Text = _TicketList [position].ActHours;
//
//			var AllocatedHours = view.FindViewById<TextView> (Resource.Id.tv_AllocatedHours);
//			AllocatedHours.Text = _TicketList [position].AllocatedHours.ToString();
//
//			var AssignTo = view.FindViewById<TextView> (Resource.Id.tv_AssignTo);
//			AssignTo.Text = _TicketList [position].AssignedTo;
//
//			var Owner = view.FindViewById<TextView> (Resource.Id.tv_Owner);
//			Owner.Text = _TicketList [position].Owner;
//
//			var Status = view.FindViewById<TextView> (Resource.Id.tv_Status);
//			Status.Text = _TicketList [position].TicketStatusName;

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

				var results = new List<TicketObject>();

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
						.Select(r => r.ToNetObject<TicketObject>()).ToList();
				_adapter.NotifyDataSetChanged();

				// Don't do this and see GREF counts rising
				constraint.Dispose();
				results.Dispose();
			}
		} 
	}
}

