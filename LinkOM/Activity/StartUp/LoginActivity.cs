using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using Android.Views.InputMethods;
using Android.Content.PM;

using RadialProgress;

namespace LinkOM
{
	[Activity(Label = "Link-OM", Icon = "@drawable/icon", Theme = "@style/Theme.Customtheme")]
	public class LoginActivity : Activity, TextView.IOnEditorActionListener
	{
		private LoginService _loginService;
		private System.Timers.Timer _timer;
		private int _countSeconds;

		public EditText username;
		public EditText password;

		public ProgressDialog progress;

		public RadialProgressView progressView;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//SetOrientaion ();

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Login);

			Button button = FindViewById<Button>(Resource.Id.btLogin);
			button.Click += btloginClick;  

			username = FindViewById<EditText>(Resource.Id.tv_username);
			username.SetOnEditorActionListener (this);
			username.RequestFocus ();

			password = FindViewById<EditText>(Resource.Id.tv_password);
			password.SetOnEditorActionListener (this);

//			progress = new ProgressDialog (this);
//			progress.Indeterminate = true;
//			progress.SetProgressStyle(ProgressDialogStyle.Spinner);
//			progress.SetMessage ("Please wait... 5");


			progressView = FindViewById<RadialProgressView> (Resource.Id.tinyProgress);
			progressView.MinValue = 0;
			progressView.MaxValue = 100;
			progressView.Visibility=ViewStates.Invisible;



			_timer = new System.Timers.Timer();
			//Trigger event every second
			_timer.Interval = 10;
			_timer.Elapsed += OnTimedEvent;
			//count down 5 seconds
			_countSeconds = 0;



		}


		private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
		{
	//		_countSeconds++;

			//Update visual representation here
			//Remember to do it on UI thread

//			RunOnUiThread (() => progress.SetMessage("Please wait... "+_countSeconds.ToString()));

			RunOnUiThread (() => progressView.Value ++);

			if (progressView.Value >= 100) {
				progressView.Value = 0;
			}


//			if (_countSeconds == 10)
//			{
////				RunOnUiThread (() => progress.SetCancelable(true));
////				RunOnUiThread (() => progress.SetMessage("No Connection..."));
//				_timer.Stop();
//
//			}
		}

		private void SetOrientaion(){
			int minWidth= Settings.SmallestWidth;
			if (minWidth > 360) {
				RequestedOrientation = ScreenOrientation.SensorLandscape;

			}
			else if (minWidth <= 360) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			}
		}

		private int ConvertPixelsToDp(float pixelValue)
		{
			var dp = (int) ((pixelValue)/Resources.DisplayMetrics.Density);
			return dp;
		}

		protected override void OnResume ()
		{
			base.OnResume ();
		}

		public void btloginClick(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty (username.Text) && !string.IsNullOrEmpty (password.Text)) {
				//this hides the keyboard
				progressView.Visibility=ViewStates.Visible;

				var imm = (InputMethodManager)GetSystemService (Context.InputMethodService);
				imm.HideSoftInputFromWindow (password.WindowToken, HideSoftInputFlags.NotAlways);

				//add progress when click login


				ThreadPool.QueueUserWorkItem (o => Login());
			}
		}

		private void Login(){

//			RunOnUiThread (() => progress.SetCancelable(false));
//			RunOnUiThread (() => progress.Show());

			_timer.Enabled = true;

			_loginService = new LoginService();
			LoginJson obj = _loginService.Login (username.Text, password.Text);

//			RunOnUiThread (() => progress.Dismiss());

			if (obj != null) {
				if (obj.Success)
					onSuccessfulLogin (obj);
				else
					onFailLogin (obj);
			} else {
				
				RunOnUiThread (() => Toast.MakeText (this, "No Connection", ToastLength.Short).Show ());
			}
		}

		private void onSuccessfulLogin(LoginJson obj)
		{
			Settings.UserId = obj.UserId;
			Settings.Token = obj.TokenNumber;
			Settings.Username = obj.UserName;

			var activity = new Intent (this, typeof(HomeActivity));
			StartActivity (activity);
			this.Finish();
		}

		private void onFailLogin(LoginJson obj)
		{
			RunOnUiThread (() => Toast.MakeText (this, obj.ErrorMessage, ToastLength.Short).Show ());
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.menu_login, menu);
			return base.OnPrepareOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.menu_reset:
				Toast.MakeText (this, "Coming Soon", ToastLength.Short).Show ();
				return true;
			case Resource.Id.menu_server:
				var activity = new Intent (this, typeof(CheckActivity));
				activity.PutExtra ("CheckAgain", true);
				StartActivity (activity);
				this.Finish ();
				return true;
			}
			return base.OnOptionsItemSelected(item);
		}

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Go) {
				if (!string.IsNullOrEmpty (username.Text) && !string.IsNullOrEmpty (password.Text)) {
					Login ();
				} else if (string.IsNullOrEmpty (username.Text)) {
					username.RequestFocus ();
				} else {
					password.RequestFocus ();
				}
				return true;
				//next action will set focus to password edit text.
			} else if (actionId == ImeAction.Next) {
				if (!string.IsNullOrEmpty (username.Text)) {
					password.RequestFocus ();
				}
				return true;
			}
			return false;
		}


	}
}
