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
	public class MilestoneListAdapter : BaseAdapter<MilestonesList>, IFilterable
	{
		private List<MilestonesList> _originalData;
		private List<MilestonesList> _MilestoneList;

		public Filter Filter { get; private set; }

		Activity _activity;


		public MilestoneListAdapter (Activity activity, List<MilestonesList> data)
		{
			_activity = activity;
			_MilestoneList = data;

			Filter = new MilestoneFilter(this);
		}

		public override int Count 
		{
			get 
				{  
				if (_MilestoneList == null)
					{
						return 0;
					}
				return _MilestoneList.Count; 
				}
		}

		public override MilestonesList this[int position]
		{
			get { return _MilestoneList[position]; }
		} 

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public override long GetItemId (int position) {
			return long.Parse(_MilestoneList [position].Id.ToString());
		}

		public MilestonesList GetItemAtPosition(int position)
		{
			return _MilestoneList[position];
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.MilestoneList, parent, false);

			var MilestoneName = view.FindViewById<TextView> (Resource.Id.tv_MilestoneName);
			MilestoneName.Text = _MilestoneList [position].Title;

			var ProjectName = view.FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = _MilestoneList [position].ProjectName;

			var EndDate = view.FindViewById<TextView> (Resource.Id.tv_EndDate);
			if(_MilestoneList [position].EndDateString!=null)
				EndDate.Text = _MilestoneList [position].EndDateString;

			var CreatedPerson = view.FindViewById<TextView> (Resource.Id.tv_CreatedPerson);
			CreatedPerson.Text = _MilestoneList [position].AssignedTo;


			return view;
		}

		private class MilestoneFilter : Filter
		{
			private readonly MilestoneListAdapter _adapter;
			public MilestoneFilter(MilestoneListAdapter adapter)
			{
				_adapter = adapter;
			}

			protected override FilterResults PerformFiltering(ICharSequence constraint)
			{
				var returnObj = new FilterResults();

				var results = new List<MilestonesList>();

				if (_adapter._originalData == null)
					_adapter._originalData = _adapter._MilestoneList; 

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
					_adapter._MilestoneList = values.ToArray<Object>()
						.Select(r => r.ToNetObject<MilestonesList>()).ToList();
				_adapter.NotifyDataSetChanged();

				// Don't do this and see GREF counts rising
				constraint.Dispose();
				results.Dispose();
			}
		} 
	}
}

