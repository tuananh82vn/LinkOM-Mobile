
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System.Threading;
using System.Threading.Tasks;
using Android.Support.V4.Widget;
using Android.Views.InputMethods;
using Android.Text;
using com.refractored.fab;
namespace LinkOM
{
	[Activity (Label = "Document", Theme = "@style/Theme.Customtheme")]				
	public class DocumentActivity : Activity, TextView.IOnEditorActionListener
	{
		public List<DocumentList> _DocumentList;
		public DocumentListAdapter documentList; 

		public bool loading;

		public string TokenNumber;

		public ListView documentListView;
//		public SwipeRefreshLayout refresher;

		public EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;

		public InputMethodManager inputManager;
		public FrameLayout frame_Detail;

		public DocumentDetailList documentSelected;
		public FloatingActionButton fab;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.Document);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.document_title);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);
			// Create your application here

			mSearch = FindViewById<EditText>(Resource.Id.etSearch);
			mSearch.Alpha = 0;
			mSearch.SetOnEditorActionListener (this);
			mSearch.Focusable = false;
			mSearch.FocusableInTouchMode = false;
			mSearch.TextChanged += InputSearchOnTextChanged;

			documentListView = FindViewById<ListView> (Resource.Id.DocumentListView);

			InitData ();

			inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) 
			{
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} 
			else 
			{
				frame_Detail  = FindViewById<FrameLayout> (Resource.Id.frameDetail);
				frame_Detail.Visibility = ViewStates.Invisible;
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

			var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
			fab.AttachToListView(documentListView);
			fab.Click += Fab_Click;
		}

		void Fab_Click (object sender, EventArgs e)
		{
			Intent Intent2 = new Intent (this, typeof(DocumentAddActivity));
			Intent2.SetFlags (ActivityFlags.ClearWhenTaskReset);
			StartActivity(Intent2);
			//Toast.MakeText (this, "Coming soon.", ToastLength.Short).Show ();
		}

		private void InputSearchOnTextChanged(object sender, TextChangedEventArgs args)
		{
			documentList.Filter.InvokeFilter(mSearch.Text);
		}

		void anim_AnimationEndUp(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
		{
			mIsAnimating = false;
			mSearch.ClearFocus();
		}

		void anim_AnimationEndDown(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
		{
			mIsAnimating = false;
		}

		void anim_AnimationStartDown(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
		{
			mIsAnimating = true;
			mSearch.Animate().AlphaBy(1.0f).SetDuration(1000).Start();
		}

		void anim_AnimationStartUp(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
		{
			mIsAnimating = true;
			mSearch.Animate().AlphaBy(-1.0f).SetDuration(1000).Start();
		}

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Search) {
				if (!string.IsNullOrEmpty (mSearch.Text)) {
					documentList.Filter.InvokeFilter(mSearch.Text);
				} 
				return true;
				//next action will set focus to password edit text.
			} 
			return false;
		}

		//Init menu on action bar
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			if (Settings.Orientation.Equals ("Portrait")) {
				inflater.Inflate (Resource.Menu.SearchMenu, menu);
			}
			else
				inflater.Inflate (Resource.Menu.EditSearchMenu, menu);

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
			case Resource.Id.search:
				btSearchClick ();
				break;
			case Resource.Id.add:
				Intent Intent2 = new Intent (this, typeof(DocumentAddActivity));
				Intent2.SetFlags (ActivityFlags.ClearWhenTaskReset);
				StartActivity(Intent2);
				break;
			case Resource.Id.edit:
				if (documentSelected != null) {
					Intent Intent3 = new Intent (this, typeof(DocumentEditActivity));
					Intent3.PutExtra ("Document", Newtonsoft.Json.JsonConvert.SerializeObject (documentSelected));
					Intent3.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity (Intent3);

				} 
				else 
				{
					Toast.MakeText (this, "No document selected.", ToastLength.Short).Show ();
				}
				break;
			default:
				break;
			}

			return true;
		}

		public async Task InitData()
		{

			if (loading)
				return;
			loading = true;

		
			var objectFilter = new DocumentFilter ();

			documentList = new DocumentListAdapter (this,DocumentHelper.GetDocumentList(objectFilter));

			documentListView.Adapter = documentList;

			documentListView.ItemClick += listView_ItemClick;

			loading = false;
		}

		private void finish(){
			//SaveData();     
			this.Finish ();
			Android.OS.Process.KillProcess (Android.OS.Process.MyPid ());
		}


		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}

		public void btSearchClick()
		{
			if (!mAnimatedDown)
			{
				mSearch.Focusable = true;
				mSearch.FocusableInTouchMode= true;
				mSearch.RequestFocus ();
				MyAnimation anim = new MyAnimation(documentListView, documentListView.Height - mSearch.Height);
				anim.Duration = 500;
				documentListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartDown;
				anim.AnimationEnd += anim_AnimationEndDown;
				documentListView.Animate().TranslationYBy(mSearch.Height).SetDuration(500).Start();


				inputManager.ShowSoftInput(mSearch, ShowFlags.Implicit);

			}

			else
			{
				mSearch.Focusable = false;
				mSearch.FocusableInTouchMode= false;

				MyAnimation anim = new MyAnimation(documentListView, documentListView.Height + mSearch.Height);
				anim.Duration = 500;
				documentListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartUp;
				anim.AnimationEnd += anim_AnimationEndUp;
				documentListView.Animate().TranslationYBy(-mSearch.Height).SetDuration(500).Start();

				inputManager.HideSoftInputFromWindow(this.mSearch.WindowToken, 0);


			}

			mAnimatedDown = !mAnimatedDown;
		}

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			documentSelected = DocumentHelper.GetDocumentDetail(this.documentList.GetItemAtPosition (e.Position).Id);

			if (Settings.Orientation.Equals ("Portrait")) {

				var activity = new Intent (this, typeof(DocumentDetailActivity));

				activity.PutExtra ("DocumentId", documentSelected.Id);

				StartActivity (activity);
			}
			else 
			{
				frame_Detail.Visibility = ViewStates.Visible;
				DisplayDocument (documentSelected);
			}


		}

		public void DisplayDocument(DocumentDetailList obj){

			var DocumentName = FindViewById<TextView> (Resource.Id.tv_DocumentDetailName);
			DocumentName.Text = obj.Title;

			var Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);
			Internal.Checked = obj.IsInternal;

			var Email = FindViewById<CheckBox> (Resource.Id.cb_Email);
			Email.Checked = obj.IsSendEmailToClient;


			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectDetailName);
			if(obj.ProjectName!=null)
			ProjectName.Text = obj.ProjectName;

			var Category = FindViewById<TextView> (Resource.Id.tv_CategoryDetailName);
			if(obj.DocumentCategoryName!=null)
			Category.Text = obj.DocumentCategoryName;

			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_DepartmentName);
			if(obj.DepartmentName!=null)
				DepartmentName.Text = obj.DepartmentName;

			var Label = FindViewById<TextView> (Resource.Id.tv_LabelDetail);
			if(obj.Label!=null)
			Label.Text = obj.Label;
			

			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
			if(obj.Description!=null)
				Description.Text = obj.Description;
		}


	}
}

