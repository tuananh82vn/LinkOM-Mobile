
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

namespace LinkOM
{
	[Activity (Label = "Document", Theme = "@style/Theme.Customtheme")]				
	public class DocumentActivity : Activity, TextView.IOnEditorActionListener
	{
		public List<DocumentObject> _DocumentList;
		public DocumentListAdapter documentList; 

		public bool loading;

		public string TokenNumber;

		public ListView documentListView;
		public SwipeRefreshLayout refresher;

		public EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;

		public InputMethodManager inputManager;

	
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


			InitData ();

			inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);


//			refresher = FindViewById<SwipeRefreshLayout> (Resource.Id.refresher);
//
//			refresher.SetColorScheme (Resource.Color.golden,Resource.Color.ginger_brown,Resource.Color.french_blue,Resource.Color.fern_green);
//		
//			refresher.Refresh += HandleRefresh;

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

			inflater.Inflate (Resource.Menu.AddSearchMenu, menu);

			return true;
		}

//		async void HandleRefresh (object sender, EventArgs e)
//		{
//			await InitData ();
//			refresher.Refreshing = false;
//		}

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
			default:
				break;
			}

			return true;
		}

		public async Task InitData(){

			if (loading)
				return;
			loading = true;

			TokenNumber = Settings.Token;
			string url = Settings.InstanceURL;

			url=url+"/api/DocumentList";


//			List<objSort> objSort = new List<objSort>{
//				new objSort{ColumnName = "P.Name", Direction = "1"},
//				new objSort{ColumnName = "C.Name", Direction = "2"}
//			};

			var objDocument = new
			{
				Title = string.Empty,
				DocumentCategoryId = string.Empty,
				ProjectId = string.Empty,
				DepartmentId = string.Empty,
				Label = string.Empty,

			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = TokenNumber,
						PageSize = 20,
						PageNumber = 1,
						Item = objDocument
					}
				});

			string results=  ConnectWebAPI.Request(url,objsearch);

			DocumentList DocumentList = Newtonsoft.Json.JsonConvert.DeserializeObject<DocumentList> (results);

			documentList = new DocumentListAdapter (this,DocumentList.Items);

			documentListView = FindViewById<ListView> (Resource.Id.DocumentListView);

			documentListView.Adapter = documentList;

			documentListView.ItemClick += listView_ItemClick;
//
//			RegisterForContextMenu(projectListView);
//
			loading = false;
//
//			Console.WriteLine ("End load data");
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
			DocumentObject model = this.documentList.GetItemAtPosition (e.Position);

			var activity = new Intent (this, typeof(DocumentDetailActivity));

			activity.PutExtra ("Document", Newtonsoft.Json.JsonConvert.SerializeObject(model));

			StartActivity (activity);

		}

//		public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
//		{
//			if (v.Id == Android.Resource.Id.List)
//			{
//				var info = (AdapterView.AdapterContextMenuInfo) menuInfo;
//				menu.SetHeaderTitle(projectList.GetItemName(info.Position));
//				var menuItems = Resources.GetStringArray(Resource.Array.menu);
//				for (var i = 0; i < menuItems.Length; i++)
//					menu.Add(Menu.None, i, i, menuItems[i]);
//			}
//		}

//		public override bool OnContextItemSelected(IMenuItem item)
//		{
//			var info = (AdapterView.AdapterContextMenuInfo) item.MenuInfo;
//			var menuItemIndex = item.ItemId;
//			var menuItems = Resources.GetStringArray(Resource.Array.menu);
//			var menuItemName = menuItems[menuItemIndex];
//
//			var DocumentName = projectList.GetItemName(info.Position);
//			int DocumentId = int.Parse(projectList.GetItemId(info.Position).ToString());
//
//			if (menuItemName.Equals ("Add Task")) {
//				var activity = new Intent (this, typeof(AddTaskActivity));
//				activity.PutExtra ("DocumentId", DocumentId);
//				StartActivity (activity);
//			}
//			else
//				Toast.MakeText(this, string.Format("Selected {0} for item {1}", menuItemName, DocumentName), ToastLength.Short).Show();
//
//			return true;
//		}
	}
}

