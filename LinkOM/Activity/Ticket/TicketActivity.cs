﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Graphics;
using Android.Support.V4.Widget;
using Android.Util;

using RadialProgress;
using System.Timers;
using Android.Views.InputMethods;
using Android.Text;

namespace LinkOM
{
	[Activity (Label = "Ticket", Theme = "@style/Theme.Customtheme")]					
	public class TicketActivity : Activity, TextView.IOnEditorActionListener
	{
		public LinearLayout LinearLayout_Master;
//		public ProgressDialog progress;
		public List<Button> buttonList;
		public List<TicketList> taskList;
		public TicketListAdapter taskListAdapter;

		public ListView taskListView ;

		public RadialProgressView progressView;
		private System.Timers.Timer _timer;

		public EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;

		public InputMethodManager inputManager;

		public TicketList TicketSelected;
		public FrameLayout frame_TicketDetail;

		protected override void OnCreate (Bundle bundle)
		{

				base.OnCreate (bundle);

				RequestWindowFeature (WindowFeatures.ActionBar);


				SetContentView (Resource.Layout.Ticket);

				
				// Create your application here

				ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
				ActionBar.SetTitle (Resource.String.ticket_title);
				ActionBar.SetDisplayShowTitleEnabled (true);
				ActionBar.SetDisplayHomeAsUpEnabled (true);
				ActionBar.SetHomeButtonEnabled (true);

				if(!Settings.Orientation.Equals("Portrait")){
					mSearch = FindViewById<EditText>(Resource.Id.etSearch);
					mSearch.Alpha = 0;
					mSearch.SetOnEditorActionListener (this);
					mSearch.Focusable = false;
					mSearch.FocusableInTouchMode = false;
					mSearch.TextChanged += InputSearchOnTextChanged;
					inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
					frame_TicketDetail  = FindViewById<FrameLayout> (Resource.Id.frame_taskdetail);
					frame_TicketDetail.Visibility = ViewStates.Invisible;

				}

				progressView = FindViewById<RadialProgressView> (Resource.Id.tinyProgress);
				progressView.MinValue = 0;
				progressView.MaxValue = 100;



				//Lock Orientation
				if (Settings.Orientation.Equals ("Portrait")) {
					RequestedOrientation = ScreenOrientation.SensorPortrait;
				} else {
					RequestedOrientation = ScreenOrientation.SensorLandscape;
				}


				_timer = new System.Timers.Timer (10);
				_timer.Elapsed += HandleElapsed;
				_timer.Start ();

				ThreadPool.QueueUserWorkItem (o => GetTicketStatus ());

				
		}

		private void InputSearchOnTextChanged(object sender, TextChangedEventArgs args)
		{
			taskListAdapter.Filter.InvokeFilter(mSearch.Text);
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
				case Resource.Id.add:
					Intent Intent2 = new Intent (this, typeof(TicketAddActivity));
					Intent2.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity(Intent2);
					break;

			case Resource.Id.edit:
				if (TicketSelected != null) {
					Intent Intent = new Intent (this, typeof(TicketEditActivity));
					Intent.PutExtra ("Ticket", Newtonsoft.Json.JsonConvert.SerializeObject (TicketSelected));
					Intent.SetFlags (ActivityFlags.ClearWhenTaskReset);
					StartActivity (Intent);
				}
				else
					Toast.MakeText (this, "No Ticket Selected.", ToastLength.Short).Show ();
					
					break;
				case Resource.Id.search:
					btSearchClick ();
					break;
				default:
					break;
			}

			return true;
		}

		//Init menu on action bar
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;

			if (Settings.Orientation.Equals ("Portrait")) {
				inflater.Inflate (Resource.Menu.AddMenu, menu);
			}
			else
				inflater.Inflate (Resource.Menu.AddEditSearchMenu, menu);
			return true;
		}

		void HandleElapsed (object sender, ElapsedEventArgs e)
		{
			progressView.Value ++;
			if (progressView.Value >= 100) {
				progressView.Value = 0;
			}
		}

		public void GetTicketStatus ()
		{
			TicketFilter objFilter = new TicketFilter ();
			objFilter.AssignedToId = Settings.UserId;



			taskList = TicketHelper.GetTicketList (objFilter);

			//Init layout
			LinearLayout_Master = FindViewById<LinearLayout>(Resource.Id.linearLayout_Main);


			var statusList = TicketHelper.GetTicketStatusList ();
			if (statusList != null) {

				if (statusList.Count > 0) {

					buttonList = new List<Button> (statusList.Count);


					for (int i = 0; i < statusList.Count; i++) {
						//Init button
						Button button = new Button (this);

						//Get number of task
						int NumberOfTicket = CheckTicket (statusList [i].Name, taskList);

						//Add button into View
						AddRow (statusList [i].Id, statusList [i].Name, ColorHelper.GetColor (statusList [i].ColourName), button, NumberOfTicket);

						RunOnUiThread (() => button.Text = NumberOfTicket.ToString ());

						buttonList.Add (button);
					}
				}
			}
			RunOnUiThread (() => progressView.Visibility=ViewStates.Invisible);

		}



		private int CheckTicket(string status, List<TicketList>  list_Ticket){
			int count = 0;
			foreach (var task in list_Ticket) {
				if (task.TicketStatusName == status)
					count++;
			}
			return count;
		}

		private void AddRow(int id,string Title, Color color, Button button, int NumberOfTicket){

			TableRow tableRow = new TableRow (this);
			TableRow.LayoutParams layoutParams_TableRow = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent,dpToPx(70));
			layoutParams_TableRow.TopMargin = dpToPx(1);
			layoutParams_TableRow.BottomMargin = dpToPx(1);
			tableRow .LayoutParameters = layoutParams_TableRow;

			LinearLayout LinearLayout_Inside = new LinearLayout (this);
			TableRow.LayoutParams layoutParams_Linear = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent,dpToPx(70));
			LinearLayout_Inside.LayoutParameters = layoutParams_Linear;
			LinearLayout_Inside.Orientation = Orientation.Horizontal;

			TextView textView = new TextView (this);
			TableRow.LayoutParams layoutParams_textView = new TableRow.LayoutParams (dpToPx(280), dpToPx(70));
			layoutParams_textView.LeftMargin = dpToPx (10);
			textView.LayoutParameters = layoutParams_textView;
			textView.Gravity = GravityFlags.CenterVertical;
			textView.TextSize = 20;

			if(Settings.Orientation.Equals("Portrait"))
				textView.Text = Title;
			else
				textView.Text = Title +" (" +NumberOfTicket.ToString()+")";
			
			textView.Click += HandleMyButton;
			textView.Tag = id;

			TableRow.LayoutParams layoutParams_button = new TableRow.LayoutParams (dpToPx(70), dpToPx(70));
			button.LayoutParameters = layoutParams_button;
			button.Background =  Resources.GetDrawable(Resource.Drawable.RoundButton);
			button.Text="0";
			button.Gravity = GravityFlags.Center;
			button.SetTextColor (Color.Black);
			button.SetBackgroundColor (color);
			button.Tag = id;
			button.Click += HandleMyButton;


			View view = new View (this);
			TableRow.LayoutParams layoutParams_view = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, dpToPx(1));
			view.LayoutParameters = layoutParams_view;
			view.SetBackgroundColor (Color.ParseColor("#AEAEAE"));

			RunOnUiThread (() => LinearLayout_Inside.AddView (textView));

			if(Settings.Orientation.Equals("Portrait"))
				RunOnUiThread (() => LinearLayout_Inside.AddView (button));

			RunOnUiThread (() => tableRow.AddView (LinearLayout_Inside));
			RunOnUiThread (() => LinearLayout_Master.AddView (tableRow));
			RunOnUiThread (() => LinearLayout_Master.AddView (view));
		}

		private int dpToPx(int dp)
		{
			float density = Resources.DisplayMetrics.Density;
			return Int32.Parse(Math.Round((float)dp * density).ToString());
		}

		public void btBackClick(object sender, EventArgs e)
		{
			_timer.Stop ();
			this.Finish ();
			OnBackPressed ();
		}

		private void HandleMyButton(object sender, EventArgs e)
		{
			int whichOne = 0;
			if (Settings.Orientation.Equals ("Portrait")) {
				Button myObject1 = (Button)sender;
				whichOne = (int)myObject1.Tag;

				var activity = new Intent (this, typeof(TicketListActivity));
				activity.PutExtra ("TicketStatusId", whichOne);
				StartActivity (activity);
			}
			else{
					taskListView = FindViewById<ListView> (Resource.Id.TicketListView);
					TextView myObject2 = (TextView)sender;
					whichOne = (int)myObject2.Tag;
					GetTicketDetailList (whichOne);
			}
		}


		private void GetTicketDetailList(int StatusId)
		{

			if (StatusId != 0) {

					TicketFilter objFilter = new TicketFilter ();
					objFilter.AssignedToId = Settings.UserId;
					objFilter.TicketStatusId = StatusId;

					var TicketReturn = TicketHelper.GetTicketList (objFilter);
					if(TicketReturn!=null){
						
							var taskListAdapter = new TicketListAdapter (this,TicketReturn);

							taskListView.Adapter = taskListAdapter;

							taskListView.ItemClick += listView_ItemClick;
					} 
					else 
					{
							taskListView.Adapter = null;
							Toast.MakeText (this, "No Ticket Available.", ToastLength.Short).Show ();

					}

					frame_TicketDetail.Visibility = ViewStates.Invisible;
				}
		}

		//handle list item clicked
		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			TicketSelected = this.taskListAdapter.GetItemAtPosition (e.Position);
			frame_TicketDetail.Visibility = ViewStates.Visible;
			DisplayTicket (TicketSelected);
		}

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Search) {
				if (!string.IsNullOrEmpty (mSearch.Text)) {
					taskListAdapter.Filter.InvokeFilter(mSearch.Text);
				} 
				return true;
				//next action will set focus to password edit text.
			} 
			return false;
		}

		public void btSearchClick()
		{
			if (taskListAdapter != null) {
				if (!mAnimatedDown) {
					mSearch.Focusable = true;
					mSearch.FocusableInTouchMode = true;
					mSearch.RequestFocus ();
					MyAnimation anim = new MyAnimation (taskListView, taskListView.Height - mSearch.Height);
					anim.Duration = 500;
					taskListView.StartAnimation (anim);
					anim.AnimationStart += anim_AnimationStartDown;
					anim.AnimationEnd += anim_AnimationEndDown;
					taskListView.Animate ().TranslationYBy (mSearch.Height).SetDuration (500).Start ();

					inputManager.ShowSoftInput (mSearch, ShowFlags.Implicit);

				} else {

					mSearch.Focusable = false;
					mSearch.FocusableInTouchMode = false;

					MyAnimation anim = new MyAnimation (taskListView, taskListView.Height + mSearch.Height);
					anim.Duration = 500;
					taskListView.StartAnimation (anim);
					anim.AnimationStart += anim_AnimationStartUp;
					anim.AnimationEnd += anim_AnimationEndUp;
					taskListView.Animate ().TranslationYBy (-mSearch.Height).SetDuration (500).Start ();

					inputManager.HideSoftInputFromWindow (this.mSearch.WindowToken, 0);


				}

				mAnimatedDown = !mAnimatedDown;
			}
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

		public void DisplayTicket(TicketList obj)
		{

			var TicketName = FindViewById<TextView> (Resource.Id.tv_TicketDetailName);
			TicketName.Text = obj.Title;

			var Code = FindViewById<TextView> (Resource.Id.tv_Code);
			Code.Text = obj.Code;

			var Status = FindViewById<TextView> (Resource.Id.tv_Status);
			Status.Text = obj.TicketStatusName;

			var Internal = FindViewById<CheckBox> (Resource.Id.cb_Internal);
			Internal.Checked = obj.IsInternal;

			var Management = FindViewById<CheckBox> (Resource.Id.cb_Management);
			Management.Checked = obj.IsManagement;


			var ProjectName = FindViewById<TextView> (Resource.Id.tv_ProjectDetailName);
			ProjectName.Text = obj.ProjectName;

			var ProjectManager = FindViewById<TextView> (Resource.Id.tv_ProjectManager);
			ProjectManager.Text = obj.ProjectManager;


			var Label = FindViewById<TextView> (Resource.Id.tv_Label);
			Label.Text = obj.Label;

			var Type = FindViewById<TextView> (Resource.Id.tv_Type);
			Type.Text = obj.TicketTypeName;

			var Receive  = FindViewById<TextView> (Resource.Id.tv_Receive);
			Receive.Text = obj.TicketReceivedMethodName;


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


			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
			if(obj.Description!=null)
				Description.Text = obj.Description;

			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
			DepartmentName.Text = obj.DepartmentName;

		}
	}
}

