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
	[Activity(Label = "Link-OM", Icon = "@drawable/icon", NoHistory = true, Theme = "@style/Theme.Customtheme")]
    public class CheckActivity : Activity
    {
		public ProgressDialog progress;
		public EditText URLText;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.CheckInstance);

			URLText = FindViewById<EditText>(Resource.Id.URLText);
			URLText.Text = Settings.InstanceURL;

			Button button = FindViewById<Button>(Resource.Id.CheckButton);

			button.Click += (sender, e) => {
				progress.Show();
				ThreadPool.QueueUserWorkItem (o => CheckServerAgain ());
			};

			progress = new ProgressDialog (this);
			progress.Indeterminate = true;
			progress.SetProgressStyle(ProgressDialogStyle.Spinner);
			progress.SetMessage("Checking Server...");
			progress.SetCancelable(false);

			var CheckAgain = Intent.GetBooleanExtra ("CheckAgain",false);

			if (!CheckAgain) {
				progress.Show();
				ThreadPool.QueueUserWorkItem (o => CheckServer ());
			} 
        }

		public void CheckServer()
		{
			string url = Settings.InstanceURL;

			url = url + "/API/Verify";

			string results= ConnectWebAPI.Request(url,"");

			if (results != "" && results != null) {
				ResultsJson obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultsJson> (results);
				if (obj.Success) {
					StartActivity (typeof(LoginActivity));
					this.Finish ();
				}
			}
		}

		private void CheckServerAgain(){
			
			string url1 = URLText.Text;

			string url2 = url1 + "/API/Verify";

			string results1= ConnectWebAPI.Request(url2,"");

			DisplayResults(url1,results1);

		}

		private void DisplayResults(string url , string results){
			RunOnUiThread (() => progress.Dismiss());

			if (results != null && results != "") {
				ResultsJson obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultsJson> (results);
				if (obj.Success) {
					Settings.InstanceURL = url;
					StartActivity (typeof(LoginActivity));
					this.Finish ();
				} else {
					RunOnUiThread (() => Toast.MakeText (this, "No Connection to server, try again later", ToastLength.Short).Show ());
				}
			} else {
				RunOnUiThread (() => Toast.MakeText (this, "No Connection to server, try again later", ToastLength.Short).Show ());
			}
		}
			
    }
}
