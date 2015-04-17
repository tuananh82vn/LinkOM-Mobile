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

namespace LinkOM
{
	public class MenuListAdapter : BaseAdapter
	{
		Activity context;

		public List<MenuItem> items;

		public MenuListAdapter(Activity context)
			: base()
		{
			this.context = context;

			this.items = new List<MenuItem>() {

				new MenuItem() { Name = "Project", Img = Resource.Drawable.Projects },
				new MenuItem() { Name = "Task", Img = Resource.Drawable.Tasks },
				new MenuItem() { Name = "Ticket", Img = Resource.Drawable.Tickets },
				new MenuItem() { Name = "Issue", Img = Resource.Drawable.Issues },
				new MenuItem() { Name = "Milestones", Img = Resource.Drawable.Milestones },
				new MenuItem() { Name = "Document", Img = Resource.Drawable.Documents }
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

			var view = (convertView ??
				context.LayoutInflater.Inflate(
					Resource.Layout.NavigationMenu,
					parent,
					false)) as LinearLayout;

			var menuimg = view.FindViewById(Resource.Id.imageView_Menu) as ImageView;

			var menutxt = view.FindViewById(Resource.Id.tv_MenuName) as TextView;

			menuimg.SetImageResource(item.Img);

			menutxt.SetText(item.Name, TextView.BufferType.Normal);

			menutxt.Gravity = GravityFlags.Left;

			menutxt.SetTextColor(Android.Graphics.Color.WhiteSmoke);

			return view;
		}
	}

	public class MenuItem
	{
		public MenuItem()
		{

		}

		public MenuItem(string name, int img)
		{
			this.Name = name;
			this.Img = img;
		}


		public string Name { get; set; }

		public int Img { get; set; }
	}
}