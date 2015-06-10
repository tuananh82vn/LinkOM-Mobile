using System;
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
using com.refractored.fab;

namespace LinkOM
{
	[Activity (Label = "Ticket", Theme = "@style/Theme.Customtheme")]					
	public class TicketActivity : Activity, TextView.IOnEditorActionListener
	{
		public LinearLayout LinearLayout_Master;
		public List<Button> buttonList;
		public List<TicketList> ticketList;
		public TicketListAdapter ticketListAdapter;

		public ListView ticketListView ;

		public EditText mSearch;
		private bool mAnimatedDown;
		private bool mIsAnimating;

		public InputMethodManager inputManager;

		public TicketDetailList TicketSelected;

		public FrameLayout frame_TicketDetail;
		public FloatingActionButton fab;

		public TicketCommentListAdapter TicketCommentListAdapter;
		public ListView ticketCommentListView ;

		public ProgressDialog progress;

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

					ticketListView = FindViewById<ListView> (Resource.Id.TicketListView);

				}

				progress = new ProgressDialog (this,Resource.Style.StyledDialog);
				progress.Indeterminate = true;
				progress.SetMessage("Please wait...");
				progress.SetCancelable (true);
				progress.Show ();



				//Lock Orientation
				if (Settings.Orientation.Equals ("Portrait")) {
					RequestedOrientation = ScreenOrientation.SensorPortrait;
				} else {
					RequestedOrientation = ScreenOrientation.SensorLandscape;
				}

				ThreadPool.QueueUserWorkItem (o => GetTicketStatus ());

				
		}

		private void InputSearchOnTextChanged(object sender, TextChangedEventArgs args)
		{
			ticketListAdapter.Filter.InvokeFilter(mSearch.Text);
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

		void Fab_Click (object sender, EventArgs e)
		{
			Intent Intent2 = new Intent (this, typeof(TicketAddActivity));
			Intent2.SetFlags (ActivityFlags.ClearWhenTaskReset);
			StartActivity(Intent2);
		}


		public void GetTicketStatus ()
		{

			if (!Settings.Orientation.Equals ("Portrait")) 
			{
				fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
				if (fab != null) {
					fab.Visibility = ViewStates.Invisible;
					fab.Hide ();
					fab.AttachToListView (ticketListView);
					fab.Click += Fab_Click;
				}
			}

			TicketFilter objFilter = new TicketFilter ();
			objFilter.AssignedToId = Settings.UserId;

			ticketList = TicketHelper.GetTicketList (objFilter);

			//Init layout
			LinearLayout_Master = FindViewById<LinearLayout>(Resource.Id.linearLayout_Main);

			var statusList = TicketHelper.GetTicketStatusList ();

			if (statusList != null) {

				if (statusList.Count > 0) {

					List<Status> SortedList = statusList.OrderBy(o=>o.DisplayOrder).ToList();

					buttonList = new List<Button> (SortedList.Count);


					for (int i = 0; i < SortedList.Count; i++) {
						//Init button
						Button button = new Button (this);
						int NumberOfTicket = 0;

						if (ticketList != null) {
							//Get number of task
							NumberOfTicket = CheckTicket (SortedList [i].Name, ticketList);
						}

						//Add button into View
						AddRow (SortedList [i].Id, SortedList [i].Name, ColorHelper.GetColor (SortedList [i].ColourName), button, NumberOfTicket);
							
						RunOnUiThread (() => button.Text = NumberOfTicket.ToString ());

						buttonList.Add (button);
					}
				}
			}
			RunOnUiThread (() => progress.Dismiss ());

		}



		private int CheckTicket(string status, List<TicketList>  list_Ticket){
			int count = 0;
			foreach (var task in list_Ticket) {
				if (task.StatusName == status)
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
			textView.TextSize = 18;

			if (Settings.Orientation.Equals ("Portrait"))
				textView.Text = Title;
			else 
			{
				textView.Text = Title + " (" + NumberOfTicket.ToString () + ")";

			}
			textView.Tag = id;
			textView.Click += (sender, eventArgs) => {
				if(NumberOfTicket!=0)
				{
					if (Settings.Orientation.Equals ("Portrait")) 
					{
						var activity = new Intent (this, typeof(TicketListActivity));
						activity.PutExtra ("TicketStatusId", id);
						StartActivity (activity);
					}
					else
					{
						GetTicketDetailList (id);
					}
				}
				else
				{
					Toast.MakeText (this, "No Ticket Available.", ToastLength.Short).Show ();
				}
			};


			TableRow.LayoutParams layoutParams_button = new TableRow.LayoutParams (dpToPx(70), dpToPx(70));
			button.LayoutParameters = layoutParams_button;
			button.Background =  Resources.GetDrawable(Resource.Drawable.RoundButton);
			button.Tag = id;
			button.Text="0";
			if (color == Color.White) {
				button.SetBackgroundColor (Color.Gray);
			}
			else
				button.SetBackgroundColor (color);

			if (color == Color.Black) {
				button.SetTextColor (Color.White);
			}
			else
				if (color == Color.Blue) {
					button.SetTextColor (Color.White);
				}
				else
					if (color == Color.Purple) {
						button.SetTextColor (Color.White);
					}
					else
						button.SetTextColor (Color.Black);

			button.Tag = id;
			//button.Click += HandleMyButton;
			button.Click += (sender, eventArgs) => {
				if(NumberOfTicket!=0)
				{
					if (Settings.Orientation.Equals ("Portrait")) 
					{
						var activity = new Intent (this, typeof(TicketListActivity));
						activity.PutExtra ("TicketStatusId", id);
						StartActivity (activity);
					}
					else
					{
						GetTicketDetailList (id);
					}
				}
				else
				{
					Toast.MakeText (this, "No Ticket Available.", ToastLength.Short).Show ();
				}
			};




			View view = new View (this);
			TableRow.LayoutParams layoutParams_view = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, dpToPx(1));
			view.LayoutParameters = layoutParams_view;
			view.SetBackgroundColor (Color.ParseColor("#AEAEAE"));

			//LinearLayout_Inside.AddView (textView);

			RunOnUiThread (() => LinearLayout_Inside.AddView (textView));

			if (Settings.Orientation.Equals ("Portrait"))
				//LinearLayout_Inside.AddView (button);
				RunOnUiThread (() => LinearLayout_Inside.AddView (button));

//			tableRow.AddView (LinearLayout_Inside);
//			LinearLayout_Master.AddView (tableRow);
//			LinearLayout_Master.AddView (view);

			RunOnUiThread (() => tableRow.AddView (LinearLayout_Inside));
			RunOnUiThread (() => LinearLayout_Master.AddView (tableRow));
			RunOnUiThread (() => LinearLayout_Master.AddView (view));
		}

		private int dpToPx(int dp)
		{
			float density = Resources.DisplayMetrics.Density;
			return Int32.Parse(Math.Round((float)dp * density).ToString());
		}


		private void HandleMyText(object sender, EventArgs e)
		{
			
		}

		private void HandleMyButton(object sender, EventArgs e)
		{
			int whichOne = 0;
			Button myObject2 = (Button)sender;
			whichOne = (int)myObject2.Tag;

			if (Settings.Orientation.Equals ("Portrait")) 
			{
				var activity = new Intent (this, typeof(TicketListActivity));
				activity.PutExtra ("TicketStatusId", whichOne);
				StartActivity (activity);
			}
			else
			{
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
					if(TicketReturn!=null && TicketReturn.Count!=0){
						
							ticketListAdapter = new TicketListAdapter (this,TicketReturn);

							ticketListView.Adapter = ticketListAdapter;

							ticketListView.ItemClick += listView_ItemClick;

							fab.Visibility = ViewStates.Visible;
							fab.Show ();
					} 
					else 
					{
							fab.Visibility = ViewStates.Invisible;
							fab.Hide ();
							ticketListView.Adapter = null;
							Toast.MakeText (this, "No Ticket Available.", ToastLength.Short).Show ();

					}

					frame_TicketDetail.Visibility = ViewStates.Invisible;
				}
		}

		//handle list item clicked
		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var temp = this.ticketListAdapter.GetItemAtPosition (e.Position);
			frame_TicketDetail.Visibility = ViewStates.Visible;

			TicketSelected = TicketHelper.GetTicketDetail (temp.Id.Value);

			DisplayTicket (TicketSelected);

			LoadTicketComment (TicketSelected.Id.Value);
		}

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Search) {
				if (!string.IsNullOrEmpty (mSearch.Text)) {
					ticketListAdapter.Filter.InvokeFilter(mSearch.Text);
				} 
				return true;
				//next action will set focus to password edit text.
			} 
			return false;
		}

		public void btSearchClick()
		{
			if (ticketListAdapter != null) {
				if (!mAnimatedDown) {
					mSearch.Focusable = true;
					mSearch.FocusableInTouchMode = true;
					mSearch.RequestFocus ();
					MyAnimation anim = new MyAnimation (ticketListView, ticketListView.Height - mSearch.Height);
					anim.Duration = 500;
					ticketListView.StartAnimation (anim);
					anim.AnimationStart += anim_AnimationStartDown;
					anim.AnimationEnd += anim_AnimationEndDown;
					ticketListView.Animate ().TranslationYBy (mSearch.Height).SetDuration (500).Start ();

					inputManager.ShowSoftInput (mSearch, ShowFlags.Implicit);

				} else {

					mSearch.Focusable = false;
					mSearch.FocusableInTouchMode = false;

					MyAnimation anim = new MyAnimation (ticketListView, ticketListView.Height + mSearch.Height);
					anim.Duration = 500;
					ticketListView.StartAnimation (anim);
					anim.AnimationStart += anim_AnimationStartUp;
					anim.AnimationEnd += anim_AnimationEndUp;
					ticketListView.Animate ().TranslationYBy (-mSearch.Height).SetDuration (500).Start ();

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

		public void DisplayTicket(TicketDetailList obj)
		{

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


			var Description = FindViewById<TextView> (Resource.Id.tv_Description);
			if(obj.TicketDiscription!=null)
				Description.Text = obj.TicketDiscription;

			var DepartmentName = FindViewById<TextView> (Resource.Id.tv_Department);
			DepartmentName.Text = obj.DepartmentName;

			var ClientName = FindViewById<TextView> (Resource.Id.tv_Client);
			ClientName.Text = obj.ClientName;

		}

		public void LoadTicketComment(int TicketId){

			TicketCommentListAdapter = new TicketCommentListAdapter (this, TicketHelper.GetTicketCommentList(TicketId));

			ticketCommentListView = FindViewById<ListView> (Resource.Id.TicketCommentListView);

			ticketCommentListView.Adapter = TicketCommentListAdapter;

			ticketCommentListView.DividerHeight = 0;

			Utility.setListViewHeightBasedOnChildren (ticketCommentListView);

			//ticketCommentListView.ItemClick += listView_ItemClick;

		}

		public override void OnBackPressed()
		{
			this.Finish ();
			base.OnBackPressed();
		}
	}
}

