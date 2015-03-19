using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;

namespace LinkOM
{
	public class TaskListAdapter : BaseAdapter
	{
		List<Task> _TaskList;

		Activity _activity;

		public TaskListAdapter (Activity activity, List<Task> data)
		{
			_activity = activity;
			_TaskList = data;
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

		public override long GetItemId (int position) {
			return long.Parse(_TaskList [position].Id.Value.ToString());
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.TaskList, parent, false);

			var TaskTitle = view.FindViewById<TextView> (Resource.Id.TaskTitle);
			TaskTitle.Text = _TaskList [position].Id +":"+ _TaskList [position].Title;

			var ProjectName = view.FindViewById<TextView> (Resource.Id.ProjectName);
			ProjectName.Text = _TaskList [position].ProjectName;

			var TaskDetail = view.FindViewById<TextView> (Resource.Id.TaskDetail);
			TaskDetail.Text = _TaskList [position].TaskComment;

			var TaskDue = view.FindViewById<TextView> (Resource.Id.TaskDue);
			TaskDue.Text = _TaskList [position].EndDateString;

			return view;
		}
	}
}

