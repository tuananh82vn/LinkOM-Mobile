using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;

namespace LinkOM
{
	public class ProjectSpinnerAdapter : BaseAdapter, ISpinnerAdapter
	{
		private readonly Activity _context;
		private List<ProjectObject> _ProjectList;

		private readonly IList<View> _views = new List<View>();

		public ProjectSpinnerAdapter(Activity context, List<ProjectObject> data)
		{
			_context = context;
			_ProjectList = data;
		}

		public ProjectObject GetItemAtPosition(int position)
		{
			return _ProjectList.ElementAt(position);
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return null;
		}

		public override long GetItemId(int id)
		{
			return id;
		}

		public int getPositionById(int ProjectId){
			for (int i = 0; i < _ProjectList.Count (); i++) {
				if (_ProjectList.ElementAt (i).Id == ProjectId) {
					return i;
				}
			
			}
			return -1;
	    }

		public override int Count
		{
			get
			{
				return _ProjectList == null ? 0 : _ProjectList.Count();
			}
		}


		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			ProjectObject item = _ProjectList.ElementAt(position);

			var view = convertView ?? _context.LayoutInflater.Inflate (Resource.Layout.SpinnerItemDropdown, parent, false);

			var text = view.FindViewById<TextView>(Resource.Id.text);

			if (text != null)
				text.Text = item.Name;

			return view;
		}

		private void ClearViews()
		{
			foreach (var view in _views)
			{
				view.Dispose();
			}
			_views.Clear();
		}

		protected override void Dispose(bool disposing)
		{
			ClearViews();
			base.Dispose(disposing);
		}
	}
}

