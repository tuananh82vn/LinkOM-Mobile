using System;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;
using Android.App;
using Java.Lang;
using Object = Java.Lang.Object; 
using System.Linq;

namespace LinkOM
{
	public class SuperAwesomeModelAdapter : BaseAdapter<SuperAwesomeModel>
	{
		private readonly IList<SuperAwesomeModel> _items;
		private readonly Context _context;

		public SuperAwesomeModelAdapter(Context context, IList<SuperAwesomeModel> items)
		{
			_items = items;
			_context = context;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = _items[position];
			var view = convertView;

			if (view == null)
			{
				var inflater = LayoutInflater.FromContext(_context);
				view = inflater.Inflate(Resource.Layout.SuperAwesomeRow, parent, false);
			}

			view.FindViewById<TextView>(Resource.Id.left).Text = item.Left;
			view.FindViewById<TextView>(Resource.Id.right).Text = item.Right;

			return view;
		}

		public override int Count
		{
			get { return _items.Count; }
		}

		public override SuperAwesomeModel this[int position]
		{
			get { return _items[position]; }
		}
	}

	public class SuperAwesomeModel
	{
		public string Left { get; set; }
		public string Right { get; set; }
	}
}

