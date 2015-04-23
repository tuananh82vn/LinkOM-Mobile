

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
using Android.Support.V4.Widget;
using System.Threading.Tasks;
using Android.Views.InputMethods;
using Android.Text;


namespace LinkOM
{
	[Activity (Label = "Project", Theme = "@style/Theme.Customtheme")]			
	public class ProjectActivity : Activity, TextView.IOnEditorActionListener
	{
		public List<ProjectObject> _ProjectList;
		public ProjectListAdapter projectList;

		public int ProjectId;
		public SwipeRefreshLayout refresher;
		public bool loading;


		public EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;
		public ListView projectListView;

		public InputMethodManager inputManager;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.Project);
			// Create your application here

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.project_title);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			projectListView = FindViewById<ListView> (Resource.Id.ProjectListView);

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

		//Refesh data
		async void HandleRefresh (object sender, EventArgs e)
		{
			await InitData ();
			refresher.Refreshing = false;
		}

		//Loading data
		public async Task InitData(){

			if (loading)
				return;
			loading = true;

			string url = Settings.InstanceURL;

			url=url+"/api/ProjectList";


			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "P.Name", Direction = "1"},
				new objSort{ColumnName = "C.Name", Direction = "2"}
			};

			var objProject = new
			{
				Name = string.Empty,
				ClientName = string.Empty,
				DepartmentId = string.Empty,
				ProjectStatusId = string.Empty,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						PageSize = 20,
						PageNumber = 1,
						Sort = objSort,
						Item = objProject
					}
				});

			string results=  ConnectWebAPI.Request(url,objsearch);

			ProjectListJson ProjectList = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectListJson> (results);

			projectList = new ProjectListAdapter (this,ProjectList.Items);

			projectListView.Adapter = projectList;

			projectListView.ItemClick += listView_ItemClick;

//			RegisterForContextMenu(projectListView);

			loading = false;

		}

		//Handle item on action bar clicked
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
				Intent Intent = new Intent (this, typeof(ProjectAddActivity));

				Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);

				StartActivity(Intent);

				break;
			default:
				break;
			}

			return true;
		}

		//handle list item clicked
		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			//Get our item from the list adapter
			ProjectObject Project = this.projectList.GetItemAtPosition(e.Position);

			Intent addAccountIntent = new Intent (this, typeof(ProjectDetailActivity));
//			addAccountIntent.SetFlags (ActivityFlags.ClearWhenTaskReset);
//
			addAccountIntent.PutExtra ("Project", Newtonsoft.Json.JsonConvert.SerializeObject(Project));

			StartActivity(addAccountIntent);

		}

		//Init menu on action bar
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			inflater.Inflate (Resource.Menu.AddSearchMenu, menu);

			return true;
		}

		public void btSearchClick()
		{
			if (!mAnimatedDown)
			{
				Console.WriteLine ("DOWN");
				mSearch.Focusable = true;
				mSearch.FocusableInTouchMode= true;
				mSearch.RequestFocus ();
				MyAnimation anim = new MyAnimation(projectListView, projectListView.Height - mSearch.Height);
				anim.Duration = 500;
				projectListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartDown;
				anim.AnimationEnd += anim_AnimationEndDown;
				projectListView.Animate().TranslationYBy(mSearch.Height).SetDuration(500).Start();


				inputManager.ShowSoftInput(mSearch, ShowFlags.Implicit);

			}

			else
			{
				Console.WriteLine ("UP");
				mSearch.Focusable = false;
				mSearch.FocusableInTouchMode= false;

				MyAnimation anim = new MyAnimation(projectListView, projectListView.Height + mSearch.Height);
				anim.Duration = 500;
				projectListView.StartAnimation(anim);
				anim.AnimationStart += anim_AnimationStartUp;
				anim.AnimationEnd += anim_AnimationEndUp;
				projectListView.Animate().TranslationYBy(-mSearch.Height).SetDuration(500).Start();

				inputManager.HideSoftInputFromWindow(this.mSearch.WindowToken, 0);


			}

			mAnimatedDown = !mAnimatedDown;
		}

		private void InputSearchOnTextChanged(object sender, TextChangedEventArgs args)
		{
			projectList.Filter.InvokeFilter(mSearch.Text);
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
					projectList.Filter.InvokeFilter(mSearch.Text);
				} 
				return true;
				//next action will set focus to password edit text.
			} 
			return false;
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
//			var ProjectName = projectList.GetItemName(info.Position);
//			int ProjectId = int.Parse(projectList.GetItemId(info.Position).ToString());
//
//			if (menuItemName.Equals ("Add Task")) {
//				var activity = new Intent (this, typeof(AddTaskActivity));
//				activity.PutExtra ("ProjectId", ProjectId);
//				StartActivity (activity);
//			}
//			else
//				Toast.MakeText(this, string.Format("Selected {0} for item {1}", menuItemName, ProjectName), ToastLength.Short).Show();
//
//			return true;
//		}
	}
}

