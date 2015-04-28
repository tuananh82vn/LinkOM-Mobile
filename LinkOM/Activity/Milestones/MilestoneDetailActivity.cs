
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
	[Activity (Label = "MilestoneDetailActivity", Theme = "@style/Theme.Customtheme")]	
	public class MilestoneDetailActivity : Activity
	{
		private ImageButton overflowButton;
		public long MilestoneId;
		public MilestoneObject MilestoneDetail;
		public string results;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.MilestoneDetailLayout);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.milestone_title_detail);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			LoadMilestone ();

			DisplayMilestone (MilestoneDetail);

		}


		public void LoadMilestone(){
			
			results= Intent.GetStringExtra ("Milestone");

			MilestoneDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<MilestoneObject> (results);
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
					Intent.PutExtra ("Milestone", results);
					Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity(Intent);
					break;
				default:
					break;
			}

			return true;
		}

		public void DisplayMilestone(MilestoneObject obj){

			var MilestoneName = FindViewById<TextView> (Resource.Id.tv_MilestoneName);
			MilestoneName.Text = obj.Title;


			var MilestoneStatus = FindViewById<TextView> (Resource.Id.tv_Status);
			MilestoneStatus.Text = obj.Status;

			var DueDate = FindViewById<TextView> (Resource.Id.tv_DueDate);
			if(obj.EndDateString!=null)
				DueDate.Text = obj.EndDateString;

			var ExpectedEndDate = FindViewById<TextView> (Resource.Id.tv_CompletedDate);
			if(obj.ActualEndDateString!=null)
				ExpectedEndDate.Text = obj.ActualEndDateString;


//			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
//			if(obj.Description!=null)
//				Description.Text = obj.Description;

//			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
//			DepartmentName.Text = obj.DepartmentName;

		}
	}
}

