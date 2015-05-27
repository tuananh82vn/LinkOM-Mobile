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
	public class DocumentListAdapter : BaseAdapter<DocumentList>, IFilterable
	{
		private List<DocumentList> _originalData;
		List<DocumentList> _DocumentList;

		public Filter Filter { get; private set; }

		Activity _activity;

		public DocumentListAdapter (Activity activity, List<DocumentList> data)
		{
			_activity = activity;
			_DocumentList = data;

			Filter = new DocumentFilter(this);
		}


		public override DocumentList this[int position]
		{
			get { return _DocumentList[position]; }
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

		public DocumentList GetItemAtPosition(int position)
		{
			return _DocumentList[position];
		}

		public string GetItemName (int position) {
			return _DocumentList [position].Title;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.DocumentList, parent, false);

			var DocumentName = view.FindViewById<TextView> (Resource.Id.tv_DocumentName);
			DocumentName.Text = _DocumentList [position].Title;

			var project = view.FindViewById<TextView> (Resource.Id.tv_ProjectName);
			project.Text = _DocumentList [position].ProjectName;

			var tv_Category = view.FindViewById<TextView> (Resource.Id.tv_Category);
			tv_Category.Text = _DocumentList [position].DocumentCategoryName;

			var publishdate = view.FindViewById<TextView> (Resource.Id.tv_publishdate);
			publishdate.Text = _DocumentList [position].CreatedDateString;

			return view;
		}

		private class DocumentFilter : Filter
		{
			private readonly DocumentListAdapter _adapter;
			public DocumentFilter(DocumentListAdapter adapter)
			{
				_adapter = adapter;
			}

			protected override FilterResults PerformFiltering(ICharSequence constraint)
			{
				var returnObj = new FilterResults();

				var results = new List<DocumentList>();

				if (_adapter._originalData == null)
					_adapter._originalData = _adapter._DocumentList; 

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
					_adapter._DocumentList = values.ToArray<Object>()
						.Select(r => r.ToNetObject<DocumentList>()).ToList();
				_adapter.NotifyDataSetChanged();

				// Don't do this and see GREF counts rising
				constraint.Dispose();
				results.Dispose();
			}
		} 
	}
}

