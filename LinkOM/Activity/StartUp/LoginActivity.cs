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
using Android.Graphics;

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
		public CheckBox cb_rememberMe;

		public ProgressDialog progress;

		public RadialProgressView progressView;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Login);

			ImageView imageView1 = FindViewById<ImageView>(Resource.Id.imageView1);
			var imageBitmap = GetImageBitmapFromUrl(Settings.InstanceURL+"/FileReference/GetLogoImage");
			imageView1.SetImageBitmap(imageBitmap);


			Button button = FindViewById<Button>(Resource.Id.btLogin);
			button.Click += btloginClick;  

			username = FindViewById<EditText>(Resource.Id.tv_username);
			username.SetOnEditorActionListener (this);

			username.RequestFocus ();

			password = FindViewById<EditText>(Resource.Id.tv_password);
			password.SetOnEditorActionListener (this);

			cb_rememberMe  = FindViewById<CheckBox>(Resource.Id.cb_rememberMe);

			var RemmemberMe = Settings.RememberMe;

			if (RemmemberMe) {
				cb_rememberMe.Checked = true;
				username.Text = Settings.Username;
				password.Text = Settings.Password;
			}



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


			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}

		}


		private Bitmap GetImageBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}

			return imageBitmap;
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
			if (cb_rememberMe.Checked) {
				Settings.RememberMe = true;
				Settings.Password = password.Text;
			} else {
				Settings.RememberMe = false;
				Settings.Password = "";
			}


			StartActivity (new Intent (this, typeof (HomeActivity)));
			this.OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
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
