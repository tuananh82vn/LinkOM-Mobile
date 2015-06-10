
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

namespace LinkOM
{
	[Activity (Label = "IssuesDetailActivity", Theme = "@style/Theme.Customtheme")]	
	public class IssuesDetailActivity : Activity
	{
		private ImageButton overflowButton;
		public long ProjectId;
		public IssuesDetailList IssuesDetail;
		public string results;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.IssuesDetailLayout);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.issues_title_detail);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			LoadIssues ();

			DisplayIssues (IssuesDetail);

			LoadIssueComment (IssuesDetail.Id.Value);

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) 
			{
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} 
			else 
			{
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

		}


		public void LoadIssues(){
			
			results= Intent.GetStringExtra ("Issue");

			var temp  = Newtonsoft.Json.JsonConvert.DeserializeObject<IssuesList> (results);

			IssuesDetail = IssuesHelper.GetIssuesDetail (temp.Id.Value);
		}

		public void LoadIssueComment(int IssueId){


			var issueList = IssuesHelper.GetIssuesCommentList (IssueId);

			var issuesCommentListAdapter = new IssuesCommentListAdapter (this, issueList);

			var issuesCommentListView = FindViewById<ListView> (Resource.Id.IssuesCommentListView);

			issuesCommentListView.Adapter = issuesCommentListAdapter;

			issuesCommentListView.DividerHeight = 0;

			Utility.SetListViewHeightBasedOnChildren (issuesCommentListView);

			//ticketCommentListView.ItemClick += listView_ItemClick;

		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			inflater.Inflate (Resource.Menu.FullMenu, menu);

			return true;
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			base.OnOptionsItemSelected (item);

			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					OnBackPressed ();
					break;
				case Resource.Id.edit:
					Intent Intent = new Intent (this, typeof(IssuesEditActivity));
					Intent.PutExtra ("Issues", results);
					Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity(Intent);
					break;

				default:
					break;
			}

			return true;
		}

		public void DisplayIssues(IssuesDetailList obj){

			var IssuesName = FindViewById<TextView> (Resource.Id.tv_IssuesDetailName);
			IssuesName.Text = obj.Title;

			var Status = FindViewById<TextView> (Resource.Id.tv_DetailStatus);
			Status.Text = obj.StatusName;

			var tv_CodeIssues = FindViewById<TextView> (Resource.Id.tv_CodeIssues);
			tv_CodeIssues.Text = obj.IssueCode;

			var Priority = FindViewById<TextView> (Resource.Id.tv_Priority);
			Priority.Text = obj.PriorityName;


			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectDetailName);
			ProjectName.Text = obj.ProjectName;

			var tv_AssignedTo = FindViewById<TextView> (Resource.Id.tv_AssignedTo);
			tv_AssignedTo.Text = obj.AssignedToName;

			var tv_Owner = FindViewById<TextView> (Resource.Id.tv_Owner);
			tv_Owner.Text = obj.OwnerName;

			var AlloHours = FindViewById<TextView> (Resource.Id.tv_AlloHours);
			AlloHours.Text = obj.AllocatedHours.ToString();

			var OpenDate = FindViewById<TextView> (Resource.Id.tv_OpenDate);
			if(obj.OpenDateString!=null)
				OpenDate.Text = obj.OpenDateString;

			var tv_ClosedDate = FindViewById<TextView> (Resource.Id.tv_ClosedDate);
			if(obj.CloseDateString!=null)
				tv_ClosedDate.Text = obj.CloseDateString;

			var tv_ResDate = FindViewById<TextView> (Resource.Id.tv_ResDate);
			if(obj.RessolutionTargetDateString!=null)
				tv_ResDate.Text = obj.RessolutionTargetDateString;

			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
			if(obj.IssueDescription!=null)
				Description.Text = obj.IssueDescription;


			var Action = FindViewById<TextView> (Resource.Id.tv_Action);
			if(obj.IssueAction!=null)
				Action.Text = obj.IssueAction;
		}
	}
}

