
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
	[Activity (Label = "ProjectDetailActivity", Theme = "@style/Theme.Customtheme")]	
	public class ProjectDetailActivity : Activity
	{
		private ImageButton overflowButton;
		public long ProjectId;
		public ProjectDetailList ProjectDetail;
		public string results;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.ProjectDetailLayout);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.project_detail_title);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			LoadProject ();

			DisplayProject (ProjectDetail);

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

		}


		public void LoadProject(){
			
			results= Intent.GetStringExtra ("Project");

			ProjectDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectDetailList> (results);
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
					Intent Intent = new Intent (this, typeof(ProjectEditActivity));
					Intent.PutExtra ("Project", results);
					Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity(Intent);
					break;
				default:
					break;
			}

			return true;
		}

		public void DisplayProject(ProjectDetailList obj){

			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = obj.ProjectName;

			var Code = FindViewById<TextView> (Resource.Id.tv_Code);
			Code.Text = obj.ProjectCode;

			var RefCode = FindViewById<TextView> (Resource.Id.tv_RefCode);
			RefCode.Text = obj.ReferenceCode;

//			var ProjectPhase = FindViewById<TextView> (Resource.Id.tv_Phase);
//			ProjectPhase.Text = obj.ProjectPhase;

			var ProjectStatus = FindViewById<TextView> (Resource.Id.tv_Status);
			ProjectStatus.Text = obj.ProjectStatus;

			var AllocatedHours = FindViewById<TextView> (Resource.Id.tv_AlloHours);
			if (obj.AllocatedHours != null)
				AllocatedHours.Text = obj.AllocatedHours.Value.ToString();
			
			var StartDate = FindViewById<TextView> (Resource.Id.tv_StartDate);
			if(obj.StartDate!=null)
				StartDate.Text = obj.StartDate.Value.ToShortDateString();

			var EndDate = FindViewById<TextView> (Resource.Id.tv_EndDate);
			if(obj.EndDate!=null)
				EndDate.Text = obj.EndDate.Value.ToShortDateString();

			var ActualStartDate = FindViewById<TextView> (Resource.Id.tv_ActualStartDate);
			if(obj.ActualStartDate!=null)
				ActualStartDate.Text = obj.ActualStartDate.Value.ToShortDateString();

			var ActualEndDate = FindViewById<TextView> (Resource.Id.tv_ActualEndDate);
			if(obj.ActualEndDate!=null)
				ActualEndDate.Text = obj.ActualEndDate.Value.ToShortDateString();

			var ClientName = FindViewById<TextView> (Resource.Id.tv_Client);
			ClientName.Text = obj.ClientName;

			var DeliveryManager = FindViewById<TextView> (Resource.Id.tv_DeliveryManager);
			DeliveryManager.Text = obj.DeliveryManagerName;

			var ProjectManager = FindViewById<TextView> (Resource.Id.tv_ProjectManager);
			ProjectManager.Text = obj.ProjectManagerName;

			var ProjectCoordinator = FindViewById<TextView> (Resource.Id.tv_ProjectCoordinator);
			ProjectCoordinator.Text = obj.ProjectCoordinatorName;

//			var Notes = FindViewById<TextView> (Resource.Id.tv_Notes);
//			if(obj.Notes!=null)
//				Notes.Text = obj.Notes;

			var Notes = FindViewById<WebView> (Resource.Id.tv_Notes);
			if (obj.Notes != null) {
				Notes.LoadData (Html.FromHtml(obj.Notes).ToString(), "text/html", "utf8");
				Notes.SetBackgroundColor(Color.Argb(1, 0, 0, 0));
				WebSettings webSettings = Notes.Settings;
				webSettings.DefaultFontSize = 12;
			}

			var Description = FindViewById<WebView> (Resource.Id.tv_Description);
			if (obj.Description != null) {
				Description.LoadData (Html.FromHtml(obj.Description).ToString(), "text/html", "utf8");
				Description.SetBackgroundColor(Color.Argb(1, 0, 0, 0));
				WebSettings webSettings = Description.Settings;
				webSettings.DefaultFontSize = 12;
			}

//			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
//			if(obj.Description!=null)
//				Description.Text = obj.Description;

			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
			DepartmentName.Text = obj.DepartmentName;

		}

	
	}
}

