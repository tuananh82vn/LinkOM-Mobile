
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
		public DocumentDetailList DocumentDetail;
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


			var DocumentId = Intent.GetIntExtra ("DocumentId", 0);

			if (DocumentId != 0) 
			{
				DocumentDetail = LoadDocumentDetail(DocumentId);
			}
	    }



		public DocumentDetailList LoadDocumentDetail (int DocumentId){

			if (CheckLoginHelper.CheckLogin ()) 
			{
				return DocumentHelper.GetDocumentDetail (DocumentId);
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
					Intent Intent = new Intent (this, typeof(DocumentEditActivity));
					Intent.PutExtra ("Document", Newtonsoft.Json.JsonConvert.SerializeObject (DocumentDetail));
					Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity(Intent);
					break;

				default:
					break;
			}

			return true;
		}

		public void DisplayDocument(DocumentDetailList obj){

			var DocumentName = FindViewById<TextView> (Resource.Id.tv_DocumentName);
			DocumentName.Text = obj.Title;

			var Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);
			if(obj.IsInternal!=null)
			Internal.Checked = obj.IsInternal;

			var Email = FindViewById<CheckBox> (Resource.Id.cb_Email);
			if(obj.IsSendEmailToClient!=null)
			Email.Checked = obj.IsSendEmailToClient;


			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectName);
			if(obj.ProjectName!=null)
			ProjectName.Text = obj.ProjectName;

			var tv_DepartmentName = FindViewById<TextView> (Resource.Id.tv_DepartmentName);
			if(obj.DepartmentName!=null)
			tv_DepartmentName.Text = obj.DepartmentName;


			var Label = FindViewById<TextView> (Resource.Id.tv_Label);
			if(obj.Label!=null)
			Label.Text = obj.Label;

			var Category = FindViewById<TextView> (Resource.Id.tv_Category);
			if(obj.DocumentCategoryName!=null)
			Category.Text = obj.DocumentCategoryName;


			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
			if(obj.Description!=null)
				Description.Text = obj.Description;

		}
	}
}

