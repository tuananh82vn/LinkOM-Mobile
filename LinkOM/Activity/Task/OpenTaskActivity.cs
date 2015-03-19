
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
	[Activity(Label = "BasicTable", Icon = "@drawable/icon")]
	public class OpenTaskActivity : Activity {

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView (Resource.Layout.OpenTaskView);

			ImageButton button = FindViewById<ImageButton>(Resource.Id.bt_Back);
			button.Click += btBackClick;

			string text = Intent.GetStringExtra ("Json") ?? "Data not available";

			List<Task> items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Task>> (text);

			var taskList = new TaskListAdapter (this,items);

			var contactsListView = FindViewById<ListView> (Resource.Id.TaskListView);

			contactsListView.Adapter = taskList;

		}
		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}

	}


}

