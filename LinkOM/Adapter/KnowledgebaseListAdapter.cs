using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;

namespace LinkOM
{
	public class KnowledgebaseListAdapter : BaseAdapter
	{
		List<KnowledgebaseList> _KnowledgeList;

		Activity _activity;

		public KnowledgebaseListAdapter (Activity activity, List<KnowledgebaseList> data)
		{
			_activity = activity;
			_KnowledgeList = data;
		}

		public override int Count 
		{
			get 
				{  
					if (_KnowledgeList == null)
					{
						return 0;
					}
					return _KnowledgeList.Count; 
				}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public override long GetItemId (int position) {
			return long.Parse(_KnowledgeList [position].Id.ToString());
		}

		public string GetItemName (int position) {
			return _KnowledgeList [position].Title;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.KnowledgebaseList, parent, false);

			var KnowledgebaseName = view.FindViewById<TextView> (Resource.Id.KnowledgebaseName);
			KnowledgebaseName.Text = _KnowledgeList [position].Title;

			var tv_publishdate = view.FindViewById<TextView> (Resource.Id.tv_publishdate);
			tv_publishdate.Text = _KnowledgeList [position].PublishDateString;

			var tv_category = view.FindViewById<TextView> (Resource.Id.tv_category);
			tv_category.Text = _KnowledgeList [position].KnowledgebaseCategoryName;


			return view;
		}
	}
}

