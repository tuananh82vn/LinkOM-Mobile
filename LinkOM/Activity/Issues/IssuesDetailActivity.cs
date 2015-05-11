
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

			Utility.setListViewHeightBasedOnChildren (issuesCommentListView);

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

			var IssuesName = FindViewById<TextView> (Resource.Id.tv_IssuesName);
			IssuesName.Text = obj.Title;

			var Status = FindViewById<TextView> (Resource.Id.tv_Status);
			Status.Text = obj.StatusName;

			var Priority = FindViewById<TextView> (Resource.Id.tv_Priority);
			Priority.Text = obj.PriorityName;

//			var Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);
//			Internal.Checked = obj.IsInternal;

//			var Management = FindViewById<CheckBox> (Resource.Id.cb_Management);
//			Management.Checked = obj.IsManagement;


			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = obj.ProjectName;

//			var ProjectManager = FindViewById<TextView> (Resource.Id.tv_ProjectManager);
//			ProjectManager.Text = obj.ProjectManager;


//			var Label = FindViewById<TextView> (Resource.Id.tv_Label);
//			Label.Text = obj.Label;
//
//			var Type = FindViewById<TextView> (Resource.Id.tv_Type);
//			Type.Text = obj.IssuesTypeName;
//
//			var Receive  = FindViewById<TextView> (Resource.Id.tv_Receive);
//			Receive.Text = obj.IssuesReceivedMethodName;


			var AlloHours = FindViewById<TextView> (Resource.Id.tv_AlloHours);
			AlloHours.Text = obj.AllocatedHours.ToString();

//			var ActualHours  = FindViewById<TextView> (Resource.Id.tv_ActualHours);
//			ActualHours.Text = obj.ActualHours.ToString();

//			var StartDate = FindViewById<TextView> (Resource.Id.tv_StartDate);
//			if(obj.StartDate!=null)
//				StartDate.Text = obj.StartDateString;
//
//			var EndDate = FindViewById<TextView> (Resource.Id.tv_EndDate);
//			if(obj.EndDate!=null)
//				EndDate.Text = obj.EndDateString;


			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
			if(obj.IssueDescription!=null)
				Description.Text = obj.IssueDescription;

//			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
//			DepartmentName.Text = obj.DepartmentName;

		}
	}
}

