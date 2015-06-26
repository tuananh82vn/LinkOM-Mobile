
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
using Android.Webkit;
using Android.Text;
using Android.Graphics;

namespace LinkOM
{
	[Activity (Label = "MilestoneDetailActivity", Theme = "@style/Theme.Customtheme")]	
	public class MilestoneDetailActivity : Activity
	{
		public long MilestoneId;

		public MilestonesDetailList MilestoneDetail;
		public string results;

		public MilestoneCommentListAdapter milestoneCommentListAdapter;
		public ListView milestoneCommentListView ;

		public TicketCommentListAdapter TicketCommentListAdapter;
		public ListView ticketCommentListView ;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.MilestoneDetailLayout);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.milestone_title_detail);

			var upArrow = Resources.GetDrawable(Resource.Drawable.abc_ic_ab_back_mtrl_am_alpha);
			upArrow.SetColorFilter(Resources.GetColor(Resource.Color.white), PorterDuff.Mode.SrcIn);
			ActionBar.SetHomeAsUpIndicator (upArrow);


			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			LoadMilestone ();

			if(MilestoneDetail!=null)
				DisplayMilestone (MilestoneDetail);

			if(MilestoneDetail!=null)
				LoadMilestoneComment (MilestoneDetail.Id.Value);

//			ScrollView scrollView = FindViewById<ScrollView> (Resource.Id.scrollView1);
//			scrollView.Post(() => scrollView.FullScroll(FocusSearchDirection.Up));

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

		}

		public void LoadMilestoneComment(int MilestoneId){

			milestoneCommentListAdapter = new MilestoneCommentListAdapter (this, MilestonesHelper.GetMilestoneCommentList(MilestoneId));

			milestoneCommentListView = FindViewById<ListView> (Resource.Id.MilestoneCommentListView);

			milestoneCommentListView.Adapter = milestoneCommentListAdapter;

			milestoneCommentListView.DividerHeight = 0;

			Utility.SetListViewHeightBasedOnChildren (milestoneCommentListView);

		}

		public void LoadMilestone(){
			
			var MilestoneId = Intent.GetIntExtra ("MilestoneId", 0);

			if (MilestoneId != 0) 
			{
				MilestoneDetail = LoadMilestoneDetail (MilestoneId);
			}
		}

		public MilestonesDetailList LoadMilestoneDetail(int MilestoneId){

			if (CheckLoginHelper.CheckLogin ()) 
			{
				return MilestonesHelper.GetMilestonesDetail (MilestoneId);
			} 
			else 
			{
				var activity = new Intent (this, typeof(LoginActivity));
				activity.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
				StartActivity (activity);
				Finish();
				return null;
			}
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
					Intent Intent = new Intent (this, typeof(MilestoneEditActivity));
					Intent.PutExtra ("Milestone", Newtonsoft.Json.JsonConvert.SerializeObject (MilestoneDetail));
					Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity(Intent);
					break;
				default:
					break;
			}

			return true;
		}

		public void DisplayMilestone(MilestonesDetailList obj){

			var MilestoneName = FindViewById<TextView> (Resource.Id.tv_MilestoneName);
			MilestoneName.Text = obj.Title;

			var MilestoneStatus = FindViewById<TextView> (Resource.Id.tv_Status);
			if(obj.StatusName!=null)
			MilestoneStatus.Text = obj.StatusName;

			var cb_Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);
			if(obj.IsInternal!=null)
				cb_Internal.Checked = obj.IsInternal.Value;

			var tv_ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectName);
			if(obj.ProjectName!=null)
			tv_ProjectName.Text = obj.ProjectName;

			var tv_AssignedTo = FindViewById<TextView> (Resource.Id.tv_AssignedTo);
			if(obj.AssignedToName!=null)
			tv_AssignedTo.Text = obj.AssignedToName;

			var tv_Owner = FindViewById<TextView> (Resource.Id.tv_Owner);
			if(obj.OwnerName!=null)
			tv_Owner.Text = obj.OwnerName;

			var DueDate = FindViewById<TextView> (Resource.Id.tv_DueDate);
			if(obj.EndDateString!=null)
				DueDate.Text = obj.EndDateString;

			var ExpectedEndDate = FindViewById<TextView> (Resource.Id.tv_CompletedDate);
			if(obj.ActualEndDateString!=null)
				ExpectedEndDate.Text = obj.ExpectedCompletionDateString;


			var Description = FindViewById<WebView> (Resource.Id.tv_Description);
			if (obj.MilestoneDescription != null) {
				Description.LoadData (Html.FromHtml(obj.MilestoneDescription).ToString(), "text/html", "utf8");
				Description.SetBackgroundColor(Color.Argb(1, 0, 0, 0));
				WebSettings webSettings = Description.Settings;
				webSettings.DefaultFontSize = 12;
			}

			var tv_Department = FindViewById<TextView> (Resource.Id.tv_Department);
			if(obj.DepartmentName!=null)
				tv_Department.Text = obj.DepartmentName;
		}
	}
}

