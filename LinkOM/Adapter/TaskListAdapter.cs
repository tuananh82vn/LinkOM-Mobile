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
	public class TaskListAdapter : BaseAdapter<TaskObject>, IFilterable
	{
		private List<TaskObject> _originalData;
		private List<TaskObject> _TaskList;

		Activity _activity;

		public Filter Filter { get; private set; }

		public TaskListAdapter (Activity activity, List<TaskObject> data)
		{
			_activity = activity;
			_TaskList = data;

			Filter = new TaskFilter(this);
		}

		public override TaskObject this[int position]
		{
			get { return _TaskList[position]; }
		} 

		public override int Count 
		{
			get 
				{  
					if (_TaskList == null)
					{
						return 0;
					}
					return _TaskList.Count; 
				}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public TaskObject GetItemAtPosition(int position)
		{
			return _TaskList[position];
		}

		public override void NotifyDataSetChanged()
		{
			// If you are using cool stuff like sections
			// remember to update the indices here!
			base.NotifyDataSetChanged();
		} 


		public override long GetItemId (int position) {
			return long.Parse(_TaskList [position].Id.Value.ToString());
		}

		public string GetItemName (int position) {
			return _TaskList [position].Title;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.TaskList, parent, false);

			var TaskTitle = view.FindViewById<TextView> (Resource.Id.tv_TaskName);
			TaskTitle.Text = _TaskList [position].Title;


//			var TaskCode = view.FindViewById<TextView> (Resource.Id.tv_Code);
//			TaskCode.Text = _TaskList [position].Code;

			var ProjectName = view.FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = _TaskList [position].ProjectName;

//			var StartDate = view.FindViewById<TextView> (Resource.Id.tv_StartDate);
//			StartDate.Text = _TaskList [position].StartDateString;
//
//			var EndDate = view.FindViewById<TextView> (Resource.Id.tv_EndDate);
//			EndDate.Text = _TaskList [position].EndDateString;
//
//			var ActualHours = view.FindViewById<TextView> (Resource.Id.tv_ActualHours);
//			ActualHours.Text = _TaskList [position].ActHours;
//
//			var AllocatedHours = view.FindViewById<TextView> (Resource.Id.tv_AllocatedHours);
//			AllocatedHours.Text = _TaskList [position].AllocatedHours;
//
//			var AssignTo = view.FindViewById<TextView> (Resource.Id.tv_AssignTo);
//			AssignTo.Text = _TaskList [position].AssignedTo;
//
//			var Owner = view.FindViewById<TextView> (Resource.Id.tv_Owner);
//			Owner.Text = _TaskList [position].Owner;

			var Priority = view.FindViewById<ImageView> (Resource.Id.image_Priority);
			if(_TaskList [position].PriorityName.Equals("High"))
				Priority.SetImageResource(Resource.Drawable.hight_priority);
				else if(_TaskList [position].PriorityName.Equals("Medium"))
						Priority.SetImageResource(Resource.Drawable.medium_priority);
							else if(_TaskList [position].PriorityName.Equals("Low"))
								Priority.SetImageResource(Resource.Drawable.low_priority);

			return view;
		}

		private class TaskFilter : Filter
		{
			private readonly TaskListAdapter _adapter;
			public TaskFilter(TaskListAdapter adapter)
			{
				_adapter = adapter;
			}

			protected override FilterResults PerformFiltering(ICharSequence constraint)
			{
				var returnObj = new FilterResults();

				var results = new List<TaskObject>();

				if (_adapter._originalData == null)
					_adapter._originalData = _adapter._TaskList; 

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
					_adapter._TaskList = values.ToArray<Object>()
						.Select(r => r.ToNetObject<TaskObject>()).ToList();
				_adapter.NotifyDataSetChanged();

				// Don't do this and see GREF counts rising
				constraint.Dispose();
				results.Dispose();
			}
		} 
	}
}

