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
				new MenuItem() { Name = "Dashboard", Img = Resource.Drawable.home , 	Type="item"},
				new MenuItem() { Name = "MENU", 	Img = Resource.Drawable.Projects , 	Type="divider"},
				new MenuItem() { Name = "Project", 	Img = Resource.Drawable.Projects , 	Type="item" },
				new MenuItem() { Name = "Task", 	Img = Resource.Drawable.Tasks, 		Type="item" },
				new MenuItem() { Name = "Ticket", 	Img = Resource.Drawable.Tickets, 	Type="item" },
				new MenuItem() { Name = "Issue", 	Img = Resource.Drawable.Issues , 	Type="item"},
				new MenuItem() { Name = "Milestones", Img = Resource.Drawable.Milestones, Type="item" },
				new MenuItem() { Name = "Document", Img = Resource.Drawable.Documents, 	Type="item" },
				new MenuItem() { Name = "Knowledgebase", Img = Resource.Drawable.book, 	Type="item" },
				new MenuItem() { Name = "SETTING", 	Img = Resource.Drawable.Milestones, Type="divider" },
				new MenuItem() { Name = "Profile", 	Img = Resource.Drawable.Tasks, 	Type="item" },
				new MenuItem() { Name = "Watch List", 	Img = Resource.Drawable.Milestones, 	Type="item" },
				new MenuItem() { Name = "Email Configuration", 	Img = Resource.Drawable.Issues, 	Type="item" },
				new MenuItem() { Name = "HELP", 	Img = Resource.Drawable.Documents, 	Type="divider" },
				new MenuItem() { Name = "Guide", 	Img = Resource.Drawable.Help, 	Type="item" },
				new MenuItem() { Name = "About", 	Img = Resource.Drawable.about, 	Type="item" },
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

			var view = (convertView ?? 	context.LayoutInflater.Inflate(Resource.Layout.NavigationMenu,parent,false)) as LinearLayout;
			


			if (item.Type.Equals("item")) {

				var menutxt = view.FindViewById (Resource.Id.tv_MenuName) as TextView;

				var menuimg = view.FindViewById (Resource.Id.imageView_Menu) as ImageView;
				
				menuimg.SetImageResource (item.Img);

				menutxt.SetText (item.Name, TextView.BufferType.Normal);
				menutxt.SetTextColor (Android.Graphics.Color.WhiteSmoke);
				menutxt.Gravity = GravityFlags.CenterVertical;

			} 
			else if (item.Type.Equals("divider")) 
			{
				var menutxt = view.FindViewById (Resource.Id.tv_MenuName) as TextView;

				var menuimg = view.FindViewById (Resource.Id.imageView_Menu) as ImageView;
				menuimg.SetImageResource(Android.Resource.Color.Transparent);

				menutxt.SetText (item.Name, TextView.BufferType.Normal);
				menutxt.Gravity = GravityFlags.CenterVertical;
				menutxt.SetTextColor (Android.Graphics.Color.Aquamarine);
			}

			return view;
		}
	}

	public class MenuItem
	{
		public MenuItem()
		{

		}

		public MenuItem(string name, int img, string type)
		{
			this.Name = name;
			this.Img = img;
			this.Type = type;
		}

		public string Type { get; set; }

		public string Name { get; set; }

		public int Img { get; set; }
	}
}