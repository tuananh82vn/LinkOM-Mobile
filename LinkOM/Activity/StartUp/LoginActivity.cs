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

namespace LinkOM
{
    [Activity(Label = "Link-OM", Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
		private LoginService _loginService;

		public EditText username;
		public EditText password;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Login);

			_loginService = new LoginService();

			Button button = FindViewById<Button>(Resource.Id.btLogin);
			username = FindViewById<EditText>(Resource.Id.tv_username);
			password = FindViewById<EditText>(Resource.Id.tv_password);

			button.Click += btloginClick;  
        }



		public void btloginClick(object sender, EventArgs e)
		{

			LoginJson obj = _loginService.Login(username.Text, password.Text);

			if(obj.Success)
				onSuccessfulLogin(obj);
			else
				onFailLogin(obj);
		}

		private void onSuccessfulLogin(LoginJson obj)
		{
			var activity = new Intent (this, typeof(HomeActivity));
			activity.PutExtra ("TokenNumber", obj.TokenNumber);
			StartActivity (activity);
			this.Finish();
		}

		private void onFailLogin(LoginJson obj)
		{
			Toast.MakeText (this, obj.ErrorMessage, ToastLength.Short).Show ();
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


    }
}
