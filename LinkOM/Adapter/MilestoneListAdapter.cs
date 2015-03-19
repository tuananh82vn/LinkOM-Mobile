using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;

namespace LinkOM
{
	public class MilestoneListAdapter : BaseAdapter
	{
		List<Milestone> _Milestone;

		Activity _activity;

		public MilestoneListAdapter (Activity activity, List<Milestone> data)
		{
			_activity = activity;
			_Milestone = data;
		}

		public override int Count 
		{
			get 
				{  
					if (_Milestone == null)
					{
						return 0;
					}
					return _Milestone.Count; 
				}
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public override long GetItemId (int position) {
			return long.Parse(_Milestone [position].Id.ToString());
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (Resource.Layout.MilestoneList, parent, false);

			var MilestoneName = view.FindViewById<TextView> (Resource.Id.tv_MilestoneName);
			MilestoneName.Text = _Milestone [position].Title;

			var StartDate = view.FindViewById<TextView> (Resource.Id.tv_StartDate);
			if(_Milestone [position].StartDate!=null)
			StartDate.Text = _Milestone [position].StartDate.Value.ToShortDateString();

			var CreatedPerson = view.FindViewById<TextView> (Resource.Id.tv_CreatedPerson);
			CreatedPerson.Text = _Milestone [position].OwnerName;

			var Status = view.FindViewById<ImageView> (Resource.Id.image_Status);
			if(_Milestone [position].Status.Equals("Open"))
				Status.SetImageResource(Resource.Drawable.open);
			else
				Status.SetImageResource(Resource.Drawable.close);

			return view;
		}
	}
}

