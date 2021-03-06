﻿

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
using Android.Content.PM;
using System.Collections;
using com.refractored.fab;
using Android.Graphics;


namespace LinkOM
{
	[Activity (Label = "Project", Theme = "@style/Theme.Customtheme")]			
	public class ProjectActivity : Activity, TextView.IOnEditorActionListener
	{
		public List<ProjectList> _ProjectList;
		public ProjectListAdapter projectList;

		public int ProjectId;
//		public SwipeRefreshLayout refresher;
		public bool loading;


		public EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;
		public ListView projectListView;

		public InputMethodManager inputManager;

		public MenuInflater inflater;

		public ProjectDetailList ProjectSelected;
		public FrameLayout frame_Detail;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			RequestWindowFeature (WindowFeatures.ActionBar);

			SetContentView (Resource.Layout.Project);
			// Create your application here

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetTitle(Resource.String.project_title);
			ActionBar.SetDisplayShowTitleEnabled (true);

			var upArrow = Resources.GetDrawable(Resource.Drawable.abc_ic_ab_back_mtrl_am_alpha);
			upArrow.SetColorFilter(Resources.GetColor(Resource.Color.white), PorterDuff.Mode.SrcIn);
			ActionBar.SetHomeAsUpIndicator (upArrow);

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
			fab.HasShadow = true;
			fab.AttachToListView(projectListView);
			fab.Click += Fab_Click;
		}

		void Fab_Click (object sender, EventArgs e)
		{
			Intent Intent = new Intent (this, typeof(ProjectAddActivity));
			Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
			StartActivity(Intent);
		}



		//Loading data
		public void InitData(){


			projectList = new ProjectListAdapter (this, ProjectHelper.GetProjectList());

			projectListView.Adapter = projectList;

			projectListView.ItemClick += listView_ItemClick;

			RegisterForContextMenu(projectListView);

			loading = false;

		}

		public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
		{
			if (v.Id == Resource.Id.ProjectListView)
			{
				var info = (AdapterView.AdapterContextMenuInfo) menuInfo;
				menu.SetHeaderTitle(projectList[info.Position].Name);
				var menuItems = Resources.GetStringArray(Resource.Array.menu);
				for (var i = 0; i < menuItems.Length; i++)
					menu.Add(Menu.None, i, i, menuItems[i]);
			}
		}

		public override bool OnContextItemSelected(IMenuItem item)
		{
			var info = (AdapterView.AdapterContextMenuInfo) item.MenuInfo;
			var menuItemIndex = item.ItemId;
			var menuItems = Resources.GetStringArray(Resource.Array.menu);

			var menuItemName = menuItems[menuItemIndex];

			var ProjectId = projectList[info.Position].Id.Value;

			if(menuItemName.Equals("Add Task")){
				Intent Intent = new Intent (this, typeof(TaskAddActivity));
				Intent.PutExtra ("ProjectId", ProjectId);
				Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
				StartActivity(Intent);
			}
			else
				if(menuItemName.Equals("Add Ticket")){
					Intent Intent = new Intent (this, typeof(TicketAddActivity));
					Intent.PutExtra ("ProjectId", ProjectId);
					Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity(Intent);
				}
				else
					if(menuItemName.Equals("Add Issue")){
						Intent Intent = new Intent (this, typeof(IssuesAddActivity));
						Intent.PutExtra ("ProjectId", ProjectId);
						Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
						StartActivity(Intent);
					}
//					else if(menuItemName.Equals("Add Document")){
//						Intent Intent = new Intent (this, typeof(DocumentAddActivity));
//						Intent.PutExtra ("ProjectId", ProjectId);
//						Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
//						StartActivity(Intent);
//					}

//			Toast.MakeText(this, string.Format("Selected {0} for item {1}", menuItemName, listItemName), ToastLength.Short).Show();
			return true;
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
			case Resource.Id.edit:
				if (ProjectSelected != null) {
					Intent Intent2 = new Intent (this, typeof(ProjectEditActivity));
					Intent2.PutExtra ("Project", Newtonsoft.Json.JsonConvert.SerializeObject (ProjectSelected));
					Intent2.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity (Intent2);

				} 
				else 
				{
					Toast.MakeText (this, "No project selected.", ToastLength.Short).Show ();
				}
				break;
			default:
				break;
			}

			return true;
		}

		//handle list item clicked
		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			ProjectSelected = ProjectHelper.GetProjectDetailById(this.projectList.GetItemAtPosition (e.Position).Id.Value);

			if (Settings.Orientation.Equals ("Portrait")) {
				
				Intent addAccountIntent = new Intent (this, typeof(ProjectDetailActivity));
				addAccountIntent.PutExtra ("Project", Newtonsoft.Json.JsonConvert.SerializeObject (ProjectSelected));
				StartActivity (addAccountIntent);

			} 
			else 
			{
				frame_Detail.Visibility = ViewStates.Visible;
				DisplayProject (ProjectSelected);
			}

		}


		//Init menu on action bar
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			inflater = this.MenuInflater;

			if (Settings.Orientation.Equals ("Portrait")) {
				inflater.Inflate (Resource.Menu.SearchMenu, menu);
			}
			else
				inflater.Inflate (Resource.Menu.EditSearchMenu, menu);

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

		public void DisplayProject(ProjectDetailList obj){

			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectDetailName);
			ProjectName.Text = obj.ProjectName;

			var Code = FindViewById<TextView> (Resource.Id.tv_CodeDetail);
			Code.Text = obj.ProjectCode;

			var RefCode = FindViewById<TextView> (Resource.Id.tv_RefDetailCode);
			RefCode.Text = obj.ReferenceCode;

			var ProjectStatus = FindViewById<TextView> (Resource.Id.tv_ProjectStatus);
			ProjectStatus.Text = obj.ProjectStatus;

			var AllocatedHours = FindViewById<TextView> (Resource.Id.tv_AlloHoursDetail);
			if (obj.AllocatedHours != null)
				AllocatedHours.Text = obj.AllocatedHours.Value.ToString();
			else
				AllocatedHours.Text = "";
			
			var tv_ProjectStartDate = FindViewById<TextView> (Resource.Id.tv_ProjectStartDate);
			if (obj.StartDateString != null)
				tv_ProjectStartDate.Text = obj.StartDateString;
			else
				tv_ProjectStartDate.Text = "";
			
			var tv_ProjectEndDate = FindViewById<TextView> (Resource.Id.tv_ProjectEndDate);
			if (obj.EndDateString != null)
				tv_ProjectEndDate.Text = obj.EndDateString;
			else
				tv_ProjectEndDate.Text = "";
			
			var ActualStartDate = FindViewById<TextView> (Resource.Id.tv_ActualStartDate);
			if (obj.ActualStartDate != null)
				ActualStartDate.Text = obj.ActualStartDate.Value.ToShortDateString ();
			else
				ActualStartDate.Text = "";

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
			if (obj.Notes != null)
				Notes.Text = obj.Notes;
			else
				Notes.Text = "";

			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
			if(obj.Description!=null)
				Description.Text = obj.Description;
			else
				Description.Text = "";

			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
			DepartmentName.Text = obj.DepartmentName;


			var milestoneListView = FindViewById<ListView> (Resource.Id.MilestonesListView);

			MilestoneFilter objFilter = new MilestoneFilter ();
			objFilter.ProjectId = obj.ProjectId.Value;

			var milestoneListAdapter = new MilestoneListAdapter (this, MilestonesHelper.GetMilestonesList(objFilter));
			milestoneListView.Adapter = milestoneListAdapter;
			milestoneListView.DividerHeight = 0;

			Utility.SetListViewHeightBasedOnChildren (milestoneListView);
		}
	}
}

