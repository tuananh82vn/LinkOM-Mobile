using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace LinkOM
{
	public class LoginTask : AsyncTask <string , string  , string>
	{
		private ProgressDialog _progressDialog;
		private LoginService _loginService;
		private Context _context;

		public LoginTask(Context context, LoginService loginService)
		{
			_context = context;
			_loginService = loginService;
		}

		protected override void OnPreExecute()
		{
			base.OnPreExecute();

			_progressDialog = ProgressDialog.Show(_context, "Login In Progress", "Please wait...");
		}

		protected override string DoInBackground(params string[] temp)
		{
			LoginJson obj = _loginService.Login();
			return obj.ErrorMessage;
		}



		protected override void OnPostExecute()
		{
			base.OnPostExecute();

			_progressDialog.Hide();

//			if (obj.Success) {
//				var activity = new Intent (_context, typeof(HomeActivity));
//				activity.PutExtra ("TokenNumber", obj.TokenNumber);
//				_context.StartActivity (activity);
//			}
//			else
//				Toast.MakeText (_context, obj.ErrorMessage, ToastLength.Short).Show ();
		}
	}
}