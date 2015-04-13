using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;

namespace LinkOM
{
	public class DocumentListAdapter : BaseAdapter
	{
		List<Documents> _DocumentList;

		Activity _activity;

		public DocumentListAdapter (Activity activity, List<Documents> data)
		{
			_activity = activity;
			_DocumentList = data;
		}

		public override int Count 
		{
			get 
				{  
					if (_DocumentList == null)
					{
						return 0;
					}
					return _DocumentList.Count; 
				}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public override long GetItemId (int position) {
			return long.Parse(_DocumentList [position].Id.ToString());
		}

		public string GetItemName (int position) {
			return _DocumentList [position].Title;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.DocumentList, parent, false);

			var DocumentName = view.FindViewById<TextView> (Resource.Id.tv_DocumentName);
			DocumentName.Text = _DocumentList [position].Title;

			var category = view.FindViewById<TextView> (Resource.Id.tv_category);
			category.Text = _DocumentList [position].DocumentCategoryName;

			var project = view.FindViewById<TextView> (Resource.Id.tv_project);
			project.Text = _DocumentList [position].ProjectName;

			var Internal = view.FindViewById<CheckBox> (Resource.Id.cb_Internal);
			Internal.Checked = _DocumentList [position].IsInternal;


			var Description = view.FindViewById<TextView> (Resource.Id.tv_Description);
			Description.Text = _DocumentList [position].Description;
//
//
//			var OpenIssues = view.FindViewById<TextView> (Resource.Id.tv_OpenIssues);
//			OpenIssues.Text = _DocumentList [position].OpenIssues.Value.ToString();
//
//			var DepartmentName = view.FindViewById<TextView> (Resource.Id.tv_DepartmentName);
//			DepartmentName.Text = _DocumentList [position].DepartmentName;
//
//			var StartDate = view.FindViewById<TextView> (Resource.Id.tv_StartDate);
//			StartDate.Text = _DocumentList [position].StartDateString;
//
//			var EndDate = view.FindViewById<TextView> (Resource.Id.tv_EndDate);
//			EndDate.Text = _DocumentList [position].EndDateString;
//
//			var AcctualHours = view.FindViewById<TextView> (Resource.Id.tv_ActualHours);
//			if(_DocumentList [position].ActualHrs!=null)
//				AcctualHours.Text = _DocumentList [position].ActualHrs.Value.ToString();
//			else
//				AcctualHours.Text = "0";
//
//			var AllocatedHours = view.FindViewById<TextView> (Resource.Id.tv_AllocatedHours);
//			if(_DocumentList [position].AllocatedHours!=null)
//				AllocatedHours.Text = _DocumentList [position].AllocatedHours.Value.ToString();
//			else
//				AllocatedHours.Text = "0";
//
//			var ProjectStatus = view.FindViewById<ImageView> (Resource.Id.image_Status);
//			if(_DocumentList [position].ProjectStatus.Equals("Open"))
//				ProjectStatus.SetImageResource(Resource.Drawable.open);
//				else
//				ProjectStatus.SetImageResource(Resource.Drawable.close);


			return view;
		}
	}
}

