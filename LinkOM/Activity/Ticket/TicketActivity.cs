
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
using Android.Content.PM;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Android.Graphics;
using System.Threading.Tasks;
using System.Threading;
using Android.Support.V4.Widget;
using Android.Util;

namespace LinkOM
{
	[Activity (Label = "Ticket")]				
	public class TicketActivity : Activity
	{
		public LinearLayout LinearLayout_Master;
		public ProgressDialog progress;
		public List<Button> buttonList;
		public TicketList ticketList;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Ticket);
			// Create your application here

			var BackButton = FindViewById(Resource.Id.BackButton);
			BackButton.Click += btBackClick;

			progress = new ProgressDialog (this);
			progress.Indeterminate = true;
			progress.SetProgressStyle(ProgressDialogStyle.Spinner);
			progress.SetMessage("Loading Ticket...");
			progress.SetCancelable(false);
			progress.Show();

			ThreadPool.QueueUserWorkItem (o => InitData ());


		}

		public void InitData ()
		{
			string url = Settings.InstanceURL;

			//Load data
			string url_Ticket= url+"/api/TicketList";


			var objTicket = new
			{
				ProjectId = string.Empty,
				TicketStatusId = string.Empty,
				DepartmentId = string.Empty,
				Title = string.Empty,
				PriorityId = string.Empty,
				Label= string.Empty,
				DueBefore = string.Empty,
				AssignTo = string.Empty,
				AssignByMe = string.Empty,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						UserId = Settings.UserId,
						TokenNumber =Settings.Token,
						PageSize = 100,
						PageNumber = 1,
						SortMember ="",
						SortDirection = "",
						MainStatusId=1,
						Item = objTicket
					}
				});

			string results_Ticket= ConnectWebAPI.Request(url_Ticket,objsearch);

			if (results_Ticket != null && results_Ticket != "") {

				ticketList = Newtonsoft.Json.JsonConvert.DeserializeObject<TicketList> (results_Ticket);

			}


			//Init layout
			LinearLayout_Master = FindViewById<LinearLayout>(Resource.Id.linearLayout_Main);



			string url_TicketStatusList= url+"/api/TicketStatusList";

			string results_TicketList= ConnectWebAPI.Request(url_TicketStatusList,"");

			if (results_TicketList != null && results_TicketList != "") {

				JsonData data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results_TicketList);

				StatusList statusList = Newtonsoft.Json.JsonConvert.DeserializeObject<StatusList> (data.Data);

				if (statusList.Items.Count > 0) {
					
					buttonList = new List<Button> (statusList.Items.Count);


					for (int i = 0; i < statusList.Items.Count; i++) {
						//Init button
						Button button = new Button (this);
						//Add button into View
						AddRow (statusList.Items [i].Id ,statusList.Items [i].Name,GetColor(statusList.Items [i].ColourName),button);
						//Get number of task
						var NumberOfTicket = CheckTicket (statusList.Items [i].Name, ticketList.Items).ToString ();
						RunOnUiThread (() => button.Text =  NumberOfTicket);
						buttonList.Add (button);
					}
				}
			}



			RunOnUiThread (() => progress.Dismiss());
		}



		private Color GetColor(string ColorName){
			try
			{
				if (ColorName.Equals("orange")) return Color.Orange;
				else
					if (ColorName.Equals("pink")) return Color.Pink;
				else
					return Color.ParseColor(ColorName);
			}
			catch(Exception)
			{
				return Color.Red;
			}
		}

		private int CheckTicket(string status, List<TicketObject>  list_Ticket){
			int count = 0;
			foreach (var ticket in list_Ticket) {
				if (ticket.TicketStatusName == status)
					count++;
			}
			return count;
		}

		private void AddRow(int id,string Title, Color color, Button button){

			TableRow tableRow = new TableRow (this);
			TableRow.LayoutParams layoutParams_TableRow = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent,dpToPx(70));
			layoutParams_TableRow.TopMargin = dpToPx(1);
			layoutParams_TableRow.BottomMargin = dpToPx(1);
			tableRow .LayoutParameters = layoutParams_TableRow;

			LinearLayout LinearLayout_Inside = new LinearLayout (this);
			TableRow.LayoutParams layoutParams_Linear = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent,dpToPx(70));
			LinearLayout_Inside.LayoutParameters = layoutParams_Linear;
			LinearLayout_Inside.Orientation = Orientation.Horizontal;

			TextView textView = new TextView (this);
			TableRow.LayoutParams layoutParams_textView = new TableRow.LayoutParams (dpToPx(280), dpToPx(70));
			layoutParams_textView.LeftMargin = dpToPx (10);
			textView.LayoutParameters = layoutParams_textView;
			textView.Gravity = GravityFlags.CenterVertical;
			textView.TextSize = 20;
			textView.Text = Title;

			TableRow.LayoutParams layoutParams_button = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.MatchParent);
			button.LayoutParameters = layoutParams_button;
			button.Background =  Resources.GetDrawable(Resource.Drawable.RoundButton);
			button.Text="0";
			button.SetTextColor (Color.Black);
			button.SetBackgroundColor (color);
			button.Tag = id;
			button.Click += HandleMyButton;


			View view = new View (this);
			TableRow.LayoutParams layoutParams_view = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, dpToPx(1));
			view.LayoutParameters = layoutParams_view;
			view.SetBackgroundColor (Color.Black);

			RunOnUiThread (() => LinearLayout_Inside.AddView (textView));
			RunOnUiThread (() => LinearLayout_Inside.AddView (button));
			RunOnUiThread (() => tableRow.AddView (LinearLayout_Inside));
			RunOnUiThread (() => LinearLayout_Master.AddView (tableRow));
			RunOnUiThread (() => LinearLayout_Master.AddView (view));
		}

		private int dpToPx(int dp)
		{
			float density = Resources.DisplayMetrics.Density;
			return Int32.Parse(Math.Round((float)dp * density).ToString());
		}

		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}

		private void HandleMyButton(object sender, EventArgs e)
		{
			Button myNewButton = (Button)sender;
			int whichOne = (int)myNewButton.Tag;
			// do stuff

			var activity = new Intent (this, typeof(TicketListActivity));
			activity.PutExtra ("StatusId", whichOne);
			StartActivity (activity);
		}
	}
}

