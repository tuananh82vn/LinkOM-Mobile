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
using Android.Content.PM;
using Android.Views.InputMethods;

namespace LinkOM
{
	[Activity(Label = "Change Server", NoHistory = true, Theme = "@style/Theme.Customtheme")]
	public class CheckActivity : Activity, TextView.IOnEditorActionListener
    {
		public ProgressDialog progress;
		public EditText URLText;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.CheckInstance);

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
			ActionBar.SetDisplayHomeAsUpEnabled(true);

			URLText = FindViewById<EditText>(Resource.Id.URLText);
			URLText.Text = Settings.InstanceURL;
			URLText.RequestFocus ();
			URLText.SetOnEditorActionListener (this);

			Button button = FindViewById<Button>(Resource.Id.btCheck);

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

			//Lock Orientation
			if (Settings.Orientation.Equals ("Portrait")) {
				RequestedOrientation = ScreenOrientation.SensorPortrait;
			} else {
				RequestedOrientation = ScreenOrientation.SensorLandscape;
			}
        }

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			base.OnOptionsItemSelected (item);

			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				StartActivity (typeof(LoginActivity));
				break;
			default:
				break;
			}

			return true;
		}


		public void CheckServer()
		{
			string url = Settings.InstanceURL;

			url = url + "/API/Verify";

			string results= ConnectWebAPI.Request(url,"");

			if (results != "" && results != null) {
				ResultsJson obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultsJson> (results);
				if (obj.Success) {
					RunOnUiThread (() => progress.Dismiss());
					StartActivity (typeof(LoginActivity));
					this.Finish ();
				}
			} else
				RunOnUiThread (() => progress.Dismiss());
		}

		private void CheckServerAgain(){

			string url1 = URLText.Text.ToLower();

			bool validURL = false;

			if (url1.StartsWith ("http://")) {
				validURL = true;
			}

			if (url1.StartsWith ("www.")) {
				url1 = url1.Remove (0, 4);
			}

			if(!validURL)
				url1 = "http://" + url1;

			string url2 = url1 + "/API/Verify";

			string results1= ConnectWebAPI.Request(url2,"");

			DisplayResults(url1,results1);

		}

		private void DisplayResults(string url , string results){
			if (results != null && results != "") {
				ResultsJson obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultsJson> (results);
				if (obj.Success) {
					Settings.InstanceURL = url;
					StartActivity (typeof(LoginActivity));
					this.Finish ();
				} 
				else 
				{
					RunOnUiThread (() => progress.Dismiss());
					RunOnUiThread (() => Toast.MakeText (this, "No Connection to server, try again later", ToastLength.Short).Show ());
				}
			} 
			else 
			{
				RunOnUiThread (() => progress.Dismiss());
				RunOnUiThread (() => Toast.MakeText (this, "No Connection to server, try again later", ToastLength.Short).Show ());
			}
		}

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Go) {
				if (!string.IsNullOrEmpty (URLText.Text)) {
					CheckServerAgain ();
				} 
				else if (string.IsNullOrEmpty (URLText.Text)) {
					URLText.RequestFocus ();
				} 
				return true;
				//next action will set focus to password edit text.
			} 
			return false;
		}

			
    }
}
