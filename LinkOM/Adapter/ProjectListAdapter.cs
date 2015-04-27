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
	public class ProjectListAdapter : BaseAdapter<ProjectObject>, IFilterable
	{

		private List<ProjectObject> _originalData;
		private List<ProjectObject> _ProjectList;

		public Filter Filter { get; private set; }

		Activity _activity;

		public ProjectListAdapter (Activity activity, List<ProjectObject> data)
		{
			_activity = activity;
			_ProjectList = data;

			Filter = new ProjectFilter(this);
		}

		public override ProjectObject this[int position]
		{
			get { return _ProjectList[position]; }
		} 

		public ProjectObject GetItemAtPosition(int position)
		{
			return _ProjectList[position];
		}

		public override int Count 
		{
			get 
				{  
					if (_ProjectList == null)
					{
						return 0;
					}
					return _ProjectList.Count; 
				}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public override long GetItemId (int position) {
			return long.Parse(_ProjectList [position].Id.ToString());
		}

		public string GetItemName (int position) {
			return _ProjectList [position].Name;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.ProjectList, parent, false);

			var ProjectName = view.FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = _ProjectList [position].Name;

			var ClientName = view.FindViewById<TextView> (Resource.Id.tv_ClientName);
			ClientName.Text = _ProjectList [position].ClientName;

//			var OpenTickets = view.FindViewById<TextView> (Resource.Id.tv_OpenTickets);
//			OpenTickets.Text = _ProjectList [position].OpenTickets.Value.ToString();
//
//			var OpenTasks = view.FindViewById<TextView> (Resource.Id.tv_OpenTasks);
//			OpenTasks.Text = _ProjectList [position].OpenTasks.Value.ToString();
//
//
//			var OpenIssues = view.FindViewById<TextView> (Resource.Id.tv_OpenIssues);
//			OpenIssues.Text = _ProjectList [position].OpenIssues.Value.ToString();
//
//			var DepartmentName = view.FindViewById<TextView> (Resource.Id.tv_DepartmentName);
//			DepartmentName.Text = _ProjectList [position].DepartmentName;
//
//			var StartDate = view.FindViewById<TextView> (Resource.Id.tv_StartDate);
//			StartDate.Text = _ProjectList [position].StartDateString;
//
//			var EndDate = view.FindViewById<TextView> (Resource.Id.tv_EndDate);
//			EndDate.Text = _ProjectList [position].EndDateString;
//
//			var AcctualHours = view.FindViewById<TextView> (Resource.Id.tv_ActualHours);
//			if(_ProjectList [position].ActualHrs!=null)
//				AcctualHours.Text = _ProjectList [position].ActualHrs.Value.ToString();
//			else
//				AcctualHours.Text = "0";
//
//			var AllocatedHours = view.FindViewById<TextView> (Resource.Id.tv_AllocatedHours);
//			if(_ProjectList [position].AllocatedHours!=null)
//				AllocatedHours.Text = _ProjectList [position].AllocatedHours.Value.ToString();
//			else
//				AllocatedHours.Text = "0";

			var ProjectStatus = view.FindViewById<TextView> (Resource.Id.tv_Status);
			ProjectStatus.Text = _ProjectList [position].ProjectStatus;

			return view;
		}

		private class ProjectFilter : Filter
		{
			private readonly ProjectListAdapter _adapter;
			public ProjectFilter(ProjectListAdapter adapter)
			{
				_adapter = adapter;
			}

			protected override FilterResults PerformFiltering(ICharSequence constraint)
			{
				var returnObj = new FilterResults();

				var results = new List<ProjectObject>();

				if (_adapter._originalData == null)
					_adapter._originalData = _adapter._ProjectList; 

				if (constraint == null) return returnObj;

				if (_adapter._originalData != null && _adapter._originalData.Any())
				{
					// Compare constraint to all names lowercased.
					// It they are contained they are added to results.


					results.AddRange(_adapter._originalData.Where(t => t.Name.ToLower().Contains(constraint.ToString().ToLower())));
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
					_adapter._ProjectList = values.ToArray<Object>()
						.Select(r => r.ToNetObject<ProjectObject>()).ToList();
				_adapter.NotifyDataSetChanged();

				// Don't do this and see GREF counts rising
				constraint.Dispose();
				results.Dispose();
			}
		} 
	}
}

