using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;

namespace LinkOM
{
	public class KnowledgebaseListAdapter : BaseAdapter
	{
		List<KnowledgebaseObject> _DocumentList;

		Activity _activity;

		public KnowledgebaseListAdapter (Activity activity, List<KnowledgebaseObject> data)
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

//			var category = view.FindViewById<TextView> (Resource.Id.tv_category);
//			category.Text = _DocumentList [position].DocumentCategoryName;
//
//			var project = view.FindViewById<TextView> (Resource.Id.tv_project);
//			project.Text = _DocumentList [position].ProjectName;

			var Internal = view.FindViewById<CheckBox> (Resource.Id.cb_Internal);
			Internal.Checked = _DocumentList [position].IsInternal;

			return view;
		}
	}
}

