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
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Login);

            // Get each input EditBox (for entering longitude and latitude) and
            // the button from the layout resource:

            Button button = FindViewById<Button>(Resource.Id.getWeatherButton);

			button.Click += btloginClick;  
        }

		public void btloginClick(object sender, EventArgs e)
		{
			DoLogin ();
		}

		private async void DoLogin()
		{

			using (ProgressDialog progress = new ProgressDialog(this))
			{
				progress.Indeterminate = true;
				progress.SetProgressStyle(ProgressDialogStyle.Spinner);
				progress.SetMessage("Login in. Please wait...");
				progress.SetCancelable(false);
				progress.Show();

				await Task<bool>.Run (() => {

					EditText username = FindViewById<EditText>(Resource.Id.tv_username);
					EditText password = FindViewById<EditText>(Resource.Id.tv_password);

					string url = Settings.InstanceURL;

					url=url+"/api/logon";

					var logon = new
					{
						Item = new
						{
							UserName = username.Text,
							Password = password.Text
						}
					};

					string results= ConnectWebAPI.Request(url,logon);

					LoginJson obj = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginJson> (results);

					if (obj.Success) {
						var activity = new Intent (this, typeof(HomeActivity));
						activity.PutExtra ("TokenNumber", obj.TokenNumber);
						progress.Dismiss();
						StartActivity (activity);
						this.Finish();
					}
					else
						Toast.MakeText (this, obj.ErrorMessage, ToastLength.Short).Show ();

					return true;
				}); 

				progress.Dismiss();
			}

		}
    }
}
