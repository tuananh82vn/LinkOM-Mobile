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
	[Activity(Label = "Link-OM", Icon = "@drawable/icon", NoHistory = true)]
    public class CheckActivity : Activity
    {
		public ProgressDialog progress;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.CheckInstance);

			EditText URLText = FindViewById<EditText>(Resource.Id.URLText);
			Button button = FindViewById<Button>(Resource.Id.CheckButton);

			Waiting ();

            button.Click += (sender, e) => {

				string url1 = URLText.Text;

				string url2 = url1 + "/API/Verify";

				string results1= ConnectWebAPI.Request(url2,"");

				DisplayResults(url1,results1);

            };

        }

		public void CheckServer()
		{
			string url = Settings.InstanceURL;

			url = url + "/API/Verify";

			string results= ConnectWebAPI.Request(url,"");

			if (results != "" && results != null) {
				ResultsJson obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultsJson> (results);
				if (obj.Success) {
					progress.Dismiss();
					StartActivity (typeof(LoginActivity));
				}
			}
		}
		private void DisplayResults(string url , string results){

			if (results != null &&  results != "") {

				ResultsJson obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultsJson> (results);
				if (obj.Success) {
					Settings.InstanceURL = url;
					progress.Dismiss();
					StartActivity (typeof(LoginActivity));
				}
				else
					Toast.MakeText (this, "URL is not correct, try again", ToastLength.Short).Show ();
			}
			else
				Toast.MakeText (this, "URL is not correct, try again", ToastLength.Short).Show ();
		}

		private async void Waiting()
		{

				progress = new ProgressDialog (this);
				progress.Indeterminate = true;
				progress.SetProgressStyle(ProgressDialogStyle.Spinner);
				progress.SetMessage("Contacting server. Please wait...");
				progress.SetCancelable(false);
				progress.Show();

				await Task<bool>.Run (() => {
					CheckServer();
					return true;
				}); 

				progress.Dismiss();


		}


    }
}
