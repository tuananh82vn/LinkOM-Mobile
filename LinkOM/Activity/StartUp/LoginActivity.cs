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
using Android.Views.Animations;

namespace LinkOM
{
	[Activity(Label = "Link-OM", Icon = "@drawable/icon", Theme = "@style/myTheme")]
	public class LoginActivity : Activity, TextView.IOnEditorActionListener
	{
		private LoginService _loginService;
		private System.Timers.Timer _timer;
		private int _countSeconds;

		public EditText username;
		public EditText password;
		public CheckBox cb_rememberMe;

		public ProgressDialog progress;

		public ImageView imageView_logo;

//		public RadialProgressView progressView;

		private const int DIALOG_TEXT_ENTRY = 7;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Login);

			ImageView imageView1 = FindViewById<ImageView>(Resource.Id.imageView_companylogo);
			var imageBitmap = GetImageBitmapFromUrl(Settings.InstanceURL+"/FileReference/GetLogoImage");

			if(imageBitmap!=null)
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


			imageView_logo = FindViewById<ImageView>(Resource.Id.imageView_logo);




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

			if(!Settings.WhatsNew)
				ShowDialog (DIALOG_TEXT_ENTRY);

		}


		protected override Dialog OnCreateDialog (int id)
		{
			switch (id) {
			case DIALOG_TEXT_ENTRY: {
					// This example shows how to add a custom layout to an AlertDialog
					var factory = LayoutInflater.From (this);

					var text_entry_view = factory.Inflate (Resource.Layout.WhatNews, null);

					TextView tv_Version = text_entry_view.FindViewById<TextView>(Resource.Id.tv_Version);

					tv_Version.Text = "VERSION 1.2.2 17 JUNE 2015";

					ListView lv_News = text_entry_view.FindViewById<ListView>(Resource.Id.lv_News);

					lv_News.Adapter = new WhatsNewListAdapter(this);

					var builder = new AlertDialog.Builder (this);

					builder.SetIconAttribute (Android.Resource.Attribute.AlertDialogIcon);

					builder.SetTitle (Resource.String.what_news_title);

					builder.SetView (text_entry_view);

					builder.SetPositiveButton (Resource.String.alert_dialog_ok, OkClicked);

					builder.SetIcon (Resource.Drawable.what_news);

					return builder.Create ();
				}
			}
			return null;
		}

		private void OkClicked (object sender, DialogClickEventArgs e)
		{
			Settings.WhatsNew = true;
		}

		protected override void OnResume()
		{
			base.OnResume();
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
//			RunOnUiThread (() => progressView.Value ++);
//
//			if (progressView.Value >= 100) {
//				progressView.Value = 0;
//			}

		}

		private int ConvertPixelsToDp(float pixelValue)
		{
			var dp = (int) ((pixelValue)/Resources.DisplayMetrics.Density);
			return dp;
		}


		public void btloginClick(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty (username.Text) && !string.IsNullOrEmpty (password.Text)) {
				//this hides the keyboard
//				progressView.Visibility=ViewStates.Visible;

				var imm = (InputMethodManager)GetSystemService (Context.InputMethodService);
				imm.HideSoftInputFromWindow (password.WindowToken, HideSoftInputFlags.NotAlways);

				ThreadPool.QueueUserWorkItem (o => Login ());
			} else {
				RunOnUiThread (() => Toast.MakeText (this, "Please enter username and password", ToastLength.Short).Show ());
			}
		}

		private void Login(){

			var rotateAboutCenterAnimation = AnimationUtils.LoadAnimation(this, Resource.Animation.rotate_center);


			RunOnUiThread (() => imageView_logo.StartAnimation(rotateAboutCenterAnimation));

			Thread.Sleep (1000);

			_timer.Enabled = true;

			_loginService = new LoginService();

			LoginObject obj = _loginService.Login (username.Text, password.Text);


			if (obj != null) 
			{
				if (obj.Success)
					onSuccessfulLogin (obj);
				else
					onFailLogin (obj);
			} 
			else 
			{
				RunOnUiThread (() => Toast.MakeText (this, "No Connection", ToastLength.Short).Show ());
			}
		}

		private void onSuccessfulLogin(LoginObject obj)
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

		private void onFailLogin(LoginObject obj)
		{
//			RunOnUiThread (() => progressView.Visibility=ViewStates.Invisible);

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
