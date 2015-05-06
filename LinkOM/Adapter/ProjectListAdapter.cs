using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Java.Lang;
using Object = Java.Lang.Object; 
using System.Linq;
using Android.Content;
using Android.Graphics;

namespace LinkOM
{
	public class ProjectListAdapter : BaseAdapter<ProjectList>, IFilterable
	{

		private List<ProjectList> _originalData;
		private List<ProjectList> _ProjectList;

		public Filter Filter { get; private set; }

		private int [] mAlternatingColors;

		Activity _activity;

		public ProjectListAdapter (Activity activity, List<ProjectList> data)
		{

			_activity = activity;
			_ProjectList = data;
			Filter = new ProjectFilter(this);

			mAlternatingColors = new int[] { 0xF2F2F2, 0xC3C3C3 };
		}

		public override ProjectList this[int position]
		{
			get { return _ProjectList[position]; }
		} 

		public ProjectList GetItemAtPosition(int position)
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

//			view.SetBackgroundColor(GetColorFromInteger(mAlternatingColors[position % mAlternatingColors.Length]));

			var ProjectName = view.FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = _ProjectList [position].Name;

			var ClientName = view.FindViewById<TextView> (Resource.Id.tv_ClientName);
			ClientName.Text = _ProjectList [position].ClientName;

			var ProjectStatus = view.FindViewById<TextView> (Resource.Id.tv_Status);
			ProjectStatus.Text = _ProjectList [position].ProjectStatus;

//			if ((position % 2) == 1)
//			{
//				ProjectName.SetTextColor(Color.White);
//				ClientName.SetTextColor(Color.White);
//				ProjectStatus.SetTextColor(Color.Black);
//			}
//
//			else
//			{
//				//White background, set text black
//				ProjectName.SetTextColor(Color.Black);
//				ClientName.SetTextColor(Color.Black);
//				ProjectStatus.SetTextColor(Color.Black);
//			}



			return view;
		}

		private Color GetColorFromInteger(int color)
		{
			return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
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

				var results = new List<ProjectList>();

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
						.Select(r => r.ToNetObject<ProjectList>()).ToList();
				_adapter.NotifyDataSetChanged();

				// Don't do this and see GREF counts rising
				constraint.Dispose();
				results.Dispose();
			}
		} 
	}
}

