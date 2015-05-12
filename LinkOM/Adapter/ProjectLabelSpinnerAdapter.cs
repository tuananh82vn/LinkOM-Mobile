using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;

namespace LinkOM
{
	public class ProjectLabelSpinnerAdapter : BaseAdapter, ISpinnerAdapter
	{
		private readonly Activity _context;
		private List<ProjectLabelList> _ProjectLabelList;

		private readonly IList<View> _views = new List<View>();

		public ProjectLabelSpinnerAdapter(Activity context, List<ProjectLabelList> data)
		{
			_context = context;
			_ProjectLabelList = data;
		}

		public ProjectLabelList GetItemAtPosition(int position)
		{
			return _ProjectLabelList.ElementAt(position);
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
			for (int i = 0; i < _ProjectLabelList.Count (); i++) {
				if (_ProjectLabelList.ElementAt (i).Id == ProjectId) {
					return i;
				}

			}
			return -1;
		}

		public override int Count
		{
			get
			{
				return _ProjectLabelList == null ? 0 : _ProjectLabelList.Count();
			}
		}


		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			ProjectLabelList item = _ProjectLabelList.ElementAt(position);

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

