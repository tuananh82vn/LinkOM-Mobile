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

		public string GetItemName (int position) {
			return _TaskList [position].Title;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.TaskList, parent, false);

			var TaskTitle = view.FindViewById<TextView> (Resource.Id.tv_TaskName);
			TaskTitle.Text = _TaskList [position].Title;

			var ProjectName = view.FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = _TaskList [position].ProjectName;

			var StartDate = view.FindViewById<TextView> (Resource.Id.tv_StartDate);
			StartDate.Text = _TaskList [position].StartDate;

			var EndDate = view.FindViewById<TextView> (Resource.Id.tv_EndDate);
			EndDate.Text = _TaskList [position].EndDate;

			var ActualHours = view.FindViewById<TextView> (Resource.Id.tv_ActualHours);
			ActualHours.Text = _TaskList [position].ActHours;

			var AllocatedHours = view.FindViewById<TextView> (Resource.Id.tv_AllocatedHours);
			AllocatedHours.Text = _TaskList [position].AllocatedHours;


			var AssignTo = view.FindViewById<TextView> (Resource.Id.tv_AssignTo);
			AssignTo.Text = _TaskList [position].AssignedTo;


			var TaskStatus = view.FindViewById<ImageView> (Resource.Id.image_Status);
			if(_TaskList [position].StatusName.Equals("Open"))
				TaskStatus.SetImageResource(Resource.Drawable.open);
			else
				TaskStatus.SetImageResource(Resource.Drawable.close);



			return view;
		}
	}
}

