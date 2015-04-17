using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Graphics;
using Android.Support.V4.Widget;
using Android.Util;

using RadialProgress;
using System.Timers;

namespace LinkOM
{
	[Activity (Label = "Issues", Theme = "@style/Theme.Customtheme")]				
	public class IssuesActivity : Activity
	{
		public LinearLayout LinearLayout_Master;
		//		public ProgressDialog progress;
		public List<Button> buttonList;
		public IssuesList issuesList;
		public RadialProgressView progressView;
		private System.Timers.Timer _timer;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.Issues);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.issues_title);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);



			progressView = FindViewById<RadialProgressView> (Resource.Id.tinyProgress);
			progressView.MinValue = 0;
			progressView.MaxValue = 100;


			_timer = new System.Timers.Timer(10);
			_timer.Elapsed += HandleElapsed;
			_timer.Start();

			ThreadPool.QueueUserWorkItem (o => InitData ());


		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			base.OnOptionsItemSelected (item);

			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				OnBackPressed ();
				break;
			case Resource.Id.add:
				Intent Intent2 = new Intent (this, typeof(IssuesAddActivity));
				Intent2.SetFlags (ActivityFlags.ClearWhenTaskReset);
				StartActivity(Intent2);
				break;
			default:
				break;
			}

			return true;
		}

		//Init menu on action bar
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			inflater.Inflate (Resource.Menu.AddMenu, menu);

			return true;
		}

		void HandleElapsed (object sender, ElapsedEventArgs e)
		{
			progressView.Value ++;
			if (progressView.Value >= 100) {
				progressView.Value = 0;
			}
		}

		public void InitData ()
		{
			string url = Settings.InstanceURL;

			//Load data
			string url_Issues= url+"/api/IssueList";


			var objIssues = new
			{
				Id =string.Empty,
				Code = string.Empty,
				ProjectId = string.Empty,
				Title = string.Empty,
				ClientId = string.Empty,
				MainStatusId = string.Empty,
				PriorityId = string.Empty,
				AssignedToId= string.Empty,
				OwnerId = string.Empty,
				DepartmentId = string.Empty,
				IssueStatusId = string.Empty,
				Label = string.Empty,
				StartDateFrom= string.Empty,
				StartDateTo= string.Empty,
				EndDateFrom= string.Empty,
				EndDateTo= string.Empty,
				OverdueIssueFlag= string.Empty,
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
						Item = objIssues
					}
				});

			string results_Issues= ConnectWebAPI.Request(url_Issues,objsearch);

			if (results_Issues != null && results_Issues != "") {

				issuesList = Newtonsoft.Json.JsonConvert.DeserializeObject<IssuesList> (results_Issues);

			}


			//Init layout
			LinearLayout_Master = FindViewById<LinearLayout>(Resource.Id.linearLayout_Main);



			string url_IssuesStatusList= url+"/api/IssueStatusList";

			string results_IssuesList= ConnectWebAPI.Request(url_IssuesStatusList,"");

			if (results_IssuesList != null && results_IssuesList != "") {

				JsonData data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results_IssuesList);

				StatusList statusList = Newtonsoft.Json.JsonConvert.DeserializeObject<StatusList> (data.Data);

				if (statusList.Items.Count > 0) {

					buttonList = new List<Button> (statusList.Items.Count);

					for (int i = 0; i < statusList.Items.Count; i++) {
						//Init button
						Button button = new Button (this);
						//Add button into View
						AddRow (statusList.Items [i].Id ,statusList.Items [i].Name,ColorHelper.GetColor(statusList.Items [i].ColourName),button);
						//Get number of task
						if (issuesList != null) {
							var NumberOfIssues = CheckIssues (statusList.Items [i].Name, issuesList.Items).ToString ();
							RunOnUiThread (() => button.Text = NumberOfIssues);
							buttonList.Add (button);
						}
					}
				}
			}

			RunOnUiThread (() => progressView.Visibility=ViewStates.Invisible);
		}


		private int CheckIssues(string status, List<IssuesObject>  list_Issues){
			int count = 0;
			foreach (var task in list_Issues) {
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
			button.SetTextColor (Color.Black);
			button.SetBackgroundColor (color);
			button.Tag = id;
			button.Click += HandleMyButton;


			View view = new View (this);
			TableRow.LayoutParams layoutParams_view = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, dpToPx(1));
			view.LayoutParameters = layoutParams_view;
			view.SetBackgroundColor (Color.Gray);

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
			_timer.Stop ();
			this.Finish ();
			OnBackPressed ();
		}

		private void HandleMyButton(object sender, EventArgs e)
		{
			Button myNewButton = (Button)sender;
			int whichOne = (int)myNewButton.Tag;
			// do stuff

			var activity = new Intent (this, typeof(IssuesListActivity));
			activity.PutExtra ("IssuesStatusId", whichOne);
			StartActivity (activity);
		}
	}
}

