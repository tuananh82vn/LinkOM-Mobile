
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
	[Activity (Label = "TicketDetailActivity", Theme = "@style/Theme.Customtheme")]	
	public class TicketDetailActivity : Activity
	{
		private ImageButton overflowButton;
		public long ProjectId;
		public TicketDetailList TicketDetail;
		public string results;

		public TicketCommentListAdapter TicketCommentListAdapter;
		public ListView ticketCommentListView ;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.TicketDetailLayout);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.ticket_title_detail);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			LoadTicket ();

			DisplayTicket (TicketDetail);

			LoadTicketComment (TicketDetail.Id.Value);

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

		}


		public void LoadTicket(){
			
			results= Intent.GetStringExtra ("Ticket");

			var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<TicketList> (results);

			TicketDetail = TicketHelper.GetTicketDetail (temp.Id.Value);

		}

		public void LoadTicketComment(int TicketId){

			TicketCommentListAdapter = new TicketCommentListAdapter (this, TicketHelper.GetTicketCommentList(TicketId));

			ticketCommentListView = FindViewById<ListView> (Resource.Id.TicketCommentListView);

			ticketCommentListView.Adapter = TicketCommentListAdapter;

			ticketCommentListView.DividerHeight = 0;

			Utility.SetListViewHeightBasedOnChildren (ticketCommentListView);

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
					Intent Intent = new Intent (this, typeof(TicketEditActivity));
					Intent.PutExtra ("Ticket", results);
					Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity(Intent);
					break;

				default:
					break;
			}

			return true;
		}

		public void DisplayTicket(TicketDetailList obj){

			var TicketName = FindViewById<TextView> (Resource.Id.tv_TicketDetailName);
			TicketName.Text = obj.Title;

			var Code = FindViewById<TextView> (Resource.Id.tv_Code);
			Code.Text = obj.Code;

			var Status = FindViewById<TextView> (Resource.Id.tv_Status);
			Status.Text = obj.StatusName;

			var Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);
			if (obj.IsInternal.HasValue)
				Internal.Checked = obj.IsInternal.Value;
			else
				Internal.Checked = false;

			var Priority = FindViewById<TextView> (Resource.Id.tv_Priority);
			Priority.Text = obj.PriorityName;

			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectDetailName);
			ProjectName.Text = obj.ProjectName;

			var ProjectManager = FindViewById<TextView> (Resource.Id.tv_ProjectManager);
			ProjectManager.Text = obj.ProjectManager;

			var DeliveryManager = FindViewById<TextView> (Resource.Id.tv_DeliveryManager);
			DeliveryManager.Text = obj.DeliveryManager;

			var CoordinatorManager = FindViewById<TextView> (Resource.Id.tv_CoordinatorManager);
			CoordinatorManager.Text = obj.ProjectCoordinator;


			var tv_AssignedTo = FindViewById<TextView> (Resource.Id.tv_AssignedTo);
			tv_AssignedTo.Text = obj.AssignedToName;

			var tv_Owner = FindViewById<TextView> (Resource.Id.tv_Owner);
			tv_Owner.Text = obj.OwnerName;

			var Label = FindViewById<TextView> (Resource.Id.tv_Label);
			Label.Text = obj.Label;

			var Type = FindViewById<TextView> (Resource.Id.tv_Type);
			Type.Text = obj.TicketTypeName;

			var Receive  = FindViewById<TextView> (Resource.Id.tv_Receive);
			Receive.Text = obj.TicketReceivedMethod;


			var AlloHours = FindViewById<TextView> (Resource.Id.tv_AlloHours);
			AlloHours.Text = obj.AllocatedHours.ToString();

			var ActualHours  = FindViewById<TextView> (Resource.Id.tv_ActualHours);
			ActualHours.Text = obj.ActualHours.ToString();

			var StartDate = FindViewById<TextView> (Resource.Id.tv_StartDate);
			if(obj.StartDate!=null)
				StartDate.Text = obj.StartDateString;

			var EndDate = FindViewById<TextView> (Resource.Id.tv_EndDate);
			if(obj.EndDate!=null)
				EndDate.Text = obj.EndDateString;

			var ActStartDate = FindViewById<TextView> (Resource.Id.tv_ActStartDate);
			if(obj.ActualStartDate!=null)
				ActStartDate.Text = obj.ActualStartDateString;

			var ActEndDate = FindViewById<TextView> (Resource.Id.tv_ActEndDate);
			if(obj.ActualEndDate!=null)
				ActEndDate.Text = obj.ActualEndDateString;


			var Description = FindViewById<WebView> (Resource.Id.tv_Description);
			if (obj.TicketDiscription != null) {
				Description.LoadData (Html.FromHtml(obj.TicketDiscription).ToString(), "text/html", "utf8");
				Description.SetBackgroundColor(Color.Argb(1, 0, 0, 0));
				WebSettings webSettings = Description.Settings;
				webSettings.DefaultFontSize = 12;
			}

			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
			DepartmentName.Text = obj.DepartmentName;

			var ClientName = FindViewById<TextView> (Resource.Id.tv_Client);
			ClientName.Text = obj.ClientName;

		}
	}
}

