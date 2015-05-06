
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
	[Activity (Label = "DocumentDetailActivity", Theme = "@style/Theme.Customtheme")]	
	public class DocumentDetailActivity : Activity
	{
		private ImageButton overflowButton;
		public long ProjectId;
		public DocumentObject DocumentDetail;
		public string results;

	//	public DocumentCommentListAdapter DocumentCommentListAdapter;
		public ListView ticketCommentListView ;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.DocumentDetailLayout);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.document_title_detail);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			LoadDocument ();

			DisplayDocument (DocumentDetail);

			//LoadDocumentComment (DocumentDetail.Id);

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

		}


		public void LoadDocument(){
			
			results= Intent.GetStringExtra ("Document");

			DocumentDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<DocumentObject> (results);
		}

//		public void LoadDocumentComment(int DocumentId){
//
//			string url = Settings.InstanceURL;
//
//			//Load data
//			string url_Task= url+"/api/DocumentCommentList";
//
//
//			var objTask = new
//			{
//				DocumentId = DocumentId,
//			};
//
//			var objsearch = (new
//				{
//					objApiSearch = new
//					{
//						TokenNumber =Settings.Token,
//						Item = objTask
//					}
//				});
//
//			string results_Task= ConnectWebAPI.Request(url_Task,objsearch);
//
//			if (results_Task != null && results_Task != "") {
//
//				var ticketList = Newtonsoft.Json.JsonConvert.DeserializeObject<DocumentCommentList> (results_Task);
//
//				if (ticketList.Items != null) {
//
//					DocumentCommentListAdapter = new DocumentCommentListAdapter (this, ticketList.Items);
//
//					ticketCommentListView = FindViewById<ListView> (Resource.Id.DocumentCommentListView);
//
//					ticketCommentListView.Adapter = DocumentCommentListAdapter;
//
//					ticketCommentListView.DividerHeight = 0;
//
//					Utility.setListViewHeightBasedOnChildren (ticketCommentListView);
//
//					//ticketCommentListView.ItemClick += listView_ItemClick;
//				} 
//
//			}
//		}

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
					Intent Intent = new Intent (this, typeof(DocumentEditActivity));
					Intent.PutExtra ("Document", results);
					Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity(Intent);
					break;

				default:
					break;
			}

			return true;
		}

		public void DisplayDocument(DocumentObject obj){

			var DocumentName = FindViewById<TextView> (Resource.Id.tv_DocumentName);
			DocumentName.Text = obj.Title;



			var Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);
			Internal.Checked = obj.IsInternal;

			var Email = FindViewById<CheckBox> (Resource.Id.cb_Email);
			Email.Checked = obj.IsSendEmailToClient;


			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectName);
			ProjectName.Text = obj.ProjectName;

			var Category = FindViewById<TextView> (Resource.Id.tv_Category);
			Category.Text = obj.DocumentCategoryName;


//			var Label = FindViewById<TextView> (Resource.Id.tv_Label);
//			Label.Text = obj.Label;
//

			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
			if(obj.Description!=null)
				Description.Text = obj.Description;

//			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
//			DepartmentName.Text = obj.DepartmentName;

		}
	}
}

