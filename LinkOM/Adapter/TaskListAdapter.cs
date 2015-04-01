using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;

namespace LinkOM
{
	public class TaskListAdapter : BaseAdapter
	{
		List<TaskObject> _TaskList;

		Activity _activity;

		public TaskListAdapter (Activity activity, List<TaskObject> data)
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

		public TaskObject GetItemAtPosition(int position)
		{
			return _TaskList[position];
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


			var TaskCode = view.FindViewById<TextView> (Resource.Id.tv_Code);
			TaskCode.Text = _TaskList [position].Code;

			var ProjectName = view.FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = _TaskList [position].ProjectName;

			var StartDate = view.FindViewById<TextView> (Resource.Id.tv_StartDate);
			StartDate.Text = _TaskList [position].StartDateString;

			var EndDate = view.FindViewById<TextView> (Resource.Id.tv_EndDate);
			EndDate.Text = _TaskList [position].EndDateString;

			var ActualHours = view.FindViewById<TextView> (Resource.Id.tv_ActualHours);
			ActualHours.Text = _TaskList [position].ActHours;

			var AllocatedHours = view.FindViewById<TextView> (Resource.Id.tv_AllocatedHours);
			AllocatedHours.Text = _TaskList [position].AllocatedHours;

			var AssignTo = view.FindViewById<TextView> (Resource.Id.tv_AssignTo);
			AssignTo.Text = _TaskList [position].AssignedTo;

			var Owner = view.FindViewById<TextView> (Resource.Id.tv_Owner);
			Owner.Text = _TaskList [position].Owner;

			var Status = view.FindViewById<TextView> (Resource.Id.tv_Status);
			Status.Text = _TaskList [position].StatusName;

				

			return view;
		}
	}
}

