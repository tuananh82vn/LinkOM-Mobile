
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
		public TaskList taskList;

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
			progress.SetMessage("Loading Task...");
			progress.SetCancelable(false);
			progress.Show();

			ThreadPool.QueueUserWorkItem (o => InitData ());


		}

		public void InitData ()
		{
			string url = Settings.InstanceURL;

			//Load data
			string url_Task= url+"/api/TaskList";

			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "T.ProjectName", Direction = "1"},
				new objSort{ColumnName = "T.EndDate", Direction = "2"}
			};


			var objTask = new
			{
				Title = string.Empty,
				AssignedToId = string.Empty,
				ClientId = string.Empty,
				TaskStatusId = string.Empty,
				PriorityId = string.Empty,
				DueBeforeDate = string.Empty,
				DepartmentId = string.Empty,
				ProjectId = string.Empty,
				AssignByMe = true,
				Filter = string.Empty,
				Label = string.Empty,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						UserId = Settings.UserId,
						TokenNumber =Settings.Token,
						PageSize = 100,
						PageNumber = 1,
						Sort = objSort,
						Item = objTask
					}
				});

			string results_Task= ConnectWebAPI.Request(url_Task,objsearch);

			if (results_Task != null && results_Task != "") {

				taskList = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskList> (results_Task);

			}


			//Init layout
			LinearLayout_Master = FindViewById<LinearLayout>(Resource.Id.linearLayout_Main);



			string url_TaskStatusList= url+"/api/TaskStatusList";

			string results_TaskList= ConnectWebAPI.Request(url_TaskStatusList,"");

			if (results_TaskList != null && results_TaskList != "") {

				JsonData data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results_TaskList);

				StatusList statusList = Newtonsoft.Json.JsonConvert.DeserializeObject<StatusList> (data.Data);

				if (statusList.Items.Count > 0) {
					
					buttonList = new List<Button> (statusList.Items.Count);


					for (int i = 0; i < statusList.Items.Count; i++) {
						//Init button
						Button button = new Button (this);
						//Add button into View
						AddRow (i+1,statusList.Items [i].Name,GetColor(statusList.Items [i].ColourName),button);
						//Get number of task
						var NumberOfTask = CheckTask (statusList.Items [i].Name, taskList.Items).ToString ();
						RunOnUiThread (() => button.Text =  NumberOfTask);
						buttonList.Add (button);
					}
				}
			}



			RunOnUiThread (() => progress.Dismiss());
		}



		private Color GetColor(string ColorName){
			try{
				return Color.ParseColor(ColorName);
				}
			catch(Exception){
				return Color.Blue;
			}
		}

		private int CheckTask(string status, List<TaskObject>  list_Task){
			int count = 0;
			foreach (var task in list_Task) {
				if (task.StatusName == status)
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
			button.SetTextColor (Color.White);
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
		}
	}
}

