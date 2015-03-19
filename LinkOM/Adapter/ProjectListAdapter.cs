using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;

namespace LinkOM
{
	public class ProjectListAdapter : BaseAdapter
	{
		List<Project> _ProjectList;

		Activity _activity;

		public ProjectListAdapter (Activity activity, List<Project> data)
		{
			_activity = activity;
			_ProjectList = data;
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

			var OpenTickets = view.FindViewById<TextView> (Resource.Id.tv_OpenTickets);
			OpenTickets.Text = _ProjectList [position].OpenTickets.Value.ToString();

			var OpenTasks = view.FindViewById<TextView> (Resource.Id.tv_OpenTasks);
			OpenTasks.Text = _ProjectList [position].OpenTasks.Value.ToString();


			var OpenIssues = view.FindViewById<TextView> (Resource.Id.tv_OpenIssues);
			OpenIssues.Text = _ProjectList [position].OpenIssues.Value.ToString();

			var DepartmentName = view.FindViewById<TextView> (Resource.Id.tv_DepartmentName);
			DepartmentName.Text = _ProjectList [position].DepartmentName;

			var StartDate = view.FindViewById<TextView> (Resource.Id.tv_StartDate);
			StartDate.Text = _ProjectList [position].StartDateString;

			var EndDate = view.FindViewById<TextView> (Resource.Id.tv_EndDate);
			EndDate.Text = _ProjectList [position].EndDateString;

			var AcctualHours = view.FindViewById<TextView> (Resource.Id.tv_ActualHours);
			if(_ProjectList [position].ActualHrs!=null)
				AcctualHours.Text = _ProjectList [position].ActualHrs.Value.ToString();
			else
				AcctualHours.Text = "0";

			var AllocatedHours = view.FindViewById<TextView> (Resource.Id.tv_AllocatedHours);
			if(_ProjectList [position].AllocatedHours!=null)
				AllocatedHours.Text = _ProjectList [position].AllocatedHours.Value.ToString();
			else
				AllocatedHours.Text = "0";

			var ProjectStatus = view.FindViewById<ImageView> (Resource.Id.image_Status);
			if(_ProjectList [position].ProjectStatus.Equals("Open"))
				ProjectStatus.SetImageResource(Resource.Drawable.open);
				else
				ProjectStatus.SetImageResource(Resource.Drawable.close);


			return view;
		}
	}
}

