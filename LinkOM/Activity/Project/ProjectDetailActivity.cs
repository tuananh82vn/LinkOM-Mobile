
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
	[Activity (Label = "ProjectDetailActivity", Theme = "@style/CustomTheme")]			
	public class ProjectDetailActivity : Activity
	{
		private ImageButton overflowButton;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.ProjectDetailLayout);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.project_title);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			string TokenNumber = Settings.Token;

			long ProjectId = Intent.GetLongExtra ("ProjectId",0);

			string url = Settings.InstanceURL;

			url=url+"/api/ProjectDetailList";

			var objProject = new
			{
				Id = ProjectId,
			};

			var objsearch = (new
			{
				objApiSearch = new
				{
					TokenNumber = TokenNumber,
					Item = objProject
				}
			});

			string results= ConnectWebAPI.Request(url,objsearch);

			ProjectDetailJson ProjectDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectDetailJson> (results);

			DisplayProject (ProjectDetail.Item);

//			// Load Milestone and display 
//
//			url = Settings.InstanceURL;
//
//			url=url+"/api/MilestoneList";
//
//			List<objSort> objSort = new List<objSort>{
//				new objSort{ColumnName = "P.StartDate", Direction = "1"},
//				new objSort{ColumnName = "C.Name", Direction = "2"}
//			};
//
//			var objMilestone = new
//			{
//				Title = string.Empty,
//				ProjectId = ProjectId,
//				StatusId = string.Empty,
//				PriorityId = string.Empty,
//				DepartmentId = string.Empty,
//				ClientId = string.Empty,
//			};
//
//			var objsearch2 = (new
//			{
//				objApiSearch = new
//				{
//					TokenNumber = TokenNumber,
//					PageSize = 20,
//					PageNumber = 1,
//					Sort = objSort,
//					Item = objMilestone
//				}
//			});
//
//			string temp = ConnectWebAPI.Request(url,objsearch2);

//			if (temp != null) {
//
//				MilestoneListJson Milestone = Newtonsoft.Json.JsonConvert.DeserializeObject<MilestoneListJson> (temp);
//
//				MilestoneListAdapter MilestoneList = new MilestoneListAdapter (this, Milestone.Items);
//
//				var milestoneListView = FindViewById<ListView> (Resource.Id.list_Milestone);
//
//				milestoneListView.Adapter = MilestoneList;
//			}

		}


		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			inflater.Inflate (Resource.Menu.ProjectDetailMenu, menu);

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
				case Resource.Id.add:
					break;
				default:
					break;
			}

			return true;
		}

		public void DisplayProject(ProjectDetail obj){

			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = obj.ProjectName;

			var Code = FindViewById<TextView> (Resource.Id.tv_Code);
			Code.Text = obj.Code;

			var RefCode = FindViewById<TextView> (Resource.Id.tv_RefCode);
			RefCode.Text = obj.ReferenceCode;

			var AllocatedHours = FindViewById<TextView> (Resource.Id.tv_AlloHours);
			if (obj.AllocatedHours != null)
				AllocatedHours.Text = obj.AllocatedHours.Value.ToString();

			var ProjectStatus = FindViewById<TextView> (Resource.Id.tv_Status);
			ProjectStatus.Text = obj.ProjectStatus;

			var ProjectPhase = FindViewById<TextView> (Resource.Id.tv_Phase);
			ProjectPhase.Text = obj.ProjectPhase;

			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
			DepartmentName.Text = obj.DepartmentName;

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

			var Notes = FindViewById<TextView> (Resource.Id.tv_Notes);
			if(obj.Notes!=null)
				Notes.Text = obj.Notes;

			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
			if(obj.Description!=null)
				Description.Text = obj.Description;

		}
	}
}

