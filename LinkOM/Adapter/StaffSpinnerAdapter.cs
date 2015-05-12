using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;

namespace LinkOM
{
	public class StaffSpinnerAdapter : BaseAdapter, ISpinnerAdapter
	{
		private readonly Activity _context;
		private List<StaffList> _StaffList;

		private readonly IList<View> _views = new List<View>();

		public StaffSpinnerAdapter(Activity context, List<StaffList> data)
		{
			_context = context;
			_StaffList = data;
		}

		public StaffList GetItemAtPosition(int position)
		{
			return _StaffList.ElementAt(position);
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return null;
		}

		public override long GetItemId(int id)
		{
			return id;
		}

		public int getPositionById(int StaffId){
			for (int i = 0; i < _StaffList.Count (); i++) {
				if (_StaffList.ElementAt (i).Id == StaffId) {
					return i;
				}
			
			}
			return -1;
	    }

		public override int Count
		{
			get
			{
				return _StaffList == null ? 0 : _StaffList.Count();
			}
		}


		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			StaffList item = _StaffList.ElementAt(position);

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

