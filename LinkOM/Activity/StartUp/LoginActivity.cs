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

namespace LinkOM
{
	[Activity(Label = "Link-OM", Icon = "@drawable/icon")]
	public class LoginActivity : Activity, TextView.IOnEditorActionListener
	{
		private LoginService _loginService;

		public EditText username;
		public EditText password;

		public ProgressDialog progress;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//SetOrientaion ();

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Login);

			Button button = FindViewById<Button>(Resource.Id.btLogin);
			button.Click += btloginClick;  

			username = FindViewById<EditText>(Resource.Id.tv_username);
			password = FindViewById<EditText>(Resource.Id.tv_password);


			progress = new ProgressDialog (this);
			progress.Indeterminate = true;
			progress.SetProgressStyle(ProgressDialogStyle.Spinner);
			progress.SetMessage("Login...");
			progress.SetCancelable(false);


			//Set edit action listener to allow the next & go buttons on the input keyboard to interact with login.
			username.SetOnEditorActionListener (this);
			password.SetOnEditorActionListener (this);

			username.RequestFocus ();


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
				var imm = (InputMethodManager)GetSystemService (Context.InputMethodService);
				imm.HideSoftInputFromWindow (password.WindowToken, HideSoftInputFlags.NotAlways);

				ThreadPool.QueueUserWorkItem (o => Login());
			}
		}

		private void Login(){
			
			RunOnUiThread (() => progress.Show());

			_loginService = new LoginService();
			LoginJson obj = _loginService.Login (username.Text, password.Text);

			RunOnUiThread (() => progress.Dismiss());

			if (obj != null) {
				if (obj.Success)
					onSuccessfulLogin (obj);
				else
					onFailLogin (obj);
			}
		}

		private void onSuccessfulLogin(LoginJson obj)
		{
			Settings.UserId = obj.UserId;
			Settings.Token = obj.TokenNumber;
			Settings.Username = obj.UserName;

			var activity = new Intent (this, typeof(MainActivity));
			activity.PutExtra ("TokenNumber", obj.TokenNumber);
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
				Toast.MakeText (this, "Menu Reset Clicked", ToastLength.Short).Show ();
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
