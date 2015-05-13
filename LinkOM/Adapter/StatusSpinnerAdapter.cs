using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;

namespace LinkOM
{
	public class StatusSpinnerAdapter : BaseAdapter, ISpinnerAdapter
	{
		private readonly Activity _context;
		private List<Status> _StatusList;

		private readonly IList<View> _views = new List<View>();

		public StatusSpinnerAdapter(Activity context, List<Status> data)
		{
			_context = context;
			_StatusList = data;
		}

		public Status GetItemAtPosition(int position)
		{
			return _StatusList.ElementAt(position);
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return null;
		}

		public override long GetItemId(int id)
		{
			return id;
		}

		public int getPositionByName(string StatusName){
			for (int i = 0; i < _StatusList.Count (); i++) {
				if (_StatusList.ElementAt (i).Name == StatusName) {
					return i;
				}
			
			}
			return -1;
	    }

		public override int Count
		{
			get
			{
				return _StatusList == null ? 0 : _StatusList.Count();
			}
		}


		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			Status item = _StatusList.ElementAt(position);

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

