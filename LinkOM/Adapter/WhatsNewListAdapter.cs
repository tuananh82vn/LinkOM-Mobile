using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace LinkOM
{
	public class WhatsNewListAdapter : BaseAdapter
	{
		Activity context;

		public List<WhatsNewItem> items;

		public WhatsNewListAdapter(Activity context)
			: base()
		{
			this.context = context;

			this.items = new List<WhatsNewItem>() {
				new WhatsNewItem() { Type = "Bugfixes!" , Color = Color.Red , Content ="Fix bugs"},
				new WhatsNewItem() { Type = "New!" , Color = Color.ForestGreen , Content ="Add news function"},
				new WhatsNewItem() { Type = "Improve!" , Color = Color.ForestGreen , Content ="Speed performance improve , dummy text , dummy text, dummy text, dummy text, dummy text "},
			};
		}

		public override int Count
		{
			get { return items.Count; }
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return position;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items[position];

			var view = (convertView ?? 	context.LayoutInflater.Inflate(Resource.Layout.WhatsNewList,parent,false)) as LinearLayout;

			var tv_Type = view.FindViewById (Resource.Id.tv_Type) as TextView;
			tv_Type.SetTextColor(item.Color);
			tv_Type.Text = item.Type;


			var tv_Content = view.FindViewById (Resource.Id.tv_Content) as TextView;
			tv_Content.Text = item.Content;

			return view;
		}
	}

	public class WhatsNewItem
	{
		public WhatsNewItem()
		{
		}

		public WhatsNewItem(String type,Color color,string content)
		{
			this.Type = type;
			this.Color = color;
			this.Content = content;
		}

		public String Type { get; set; }

		public Color Color { get; set; }

		public string Content { get; set; }
	}
}