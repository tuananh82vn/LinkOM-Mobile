using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;

namespace LinkOM
{
	public class ProjectPhaseSpinnerAdapter : BaseAdapter, ISpinnerAdapter
	{
		private readonly Activity _context;
		private List<ProjectPhaseList> _ProjectPhaseList;

		private readonly IList<View> _views = new List<View>();

		public ProjectPhaseSpinnerAdapter(Activity context, List<ProjectPhaseList> data)
		{
			_context = context;
			_ProjectPhaseList = data;
		}

		public ProjectPhaseList GetItemAtPosition(int position)
		{
			return _ProjectPhaseList.ElementAt(position);
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
			for (int i = 0; i < _ProjectPhaseList.Count (); i++) {
				if (_ProjectPhaseList.ElementAt (i).Id == ProjectId) {
					return i;
				}

			}
			return -1;
		}

		public override int Count
		{
			get
			{
				return _ProjectPhaseList == null ? 0 : _ProjectPhaseList.Count();
			}
		}


		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			ProjectPhaseList item = _ProjectPhaseList.ElementAt(position);

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

