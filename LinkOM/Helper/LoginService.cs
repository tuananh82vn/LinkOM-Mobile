using System;
using System.Threading;

namespace LinkOM
{
	public class LoginService
	{
		public LoginJson Login(string username,string password)
		{
			LoginJson obj = new LoginJson ();

			string url = Settings.InstanceURL;
			
			url=url+"/api/logon";

			var logon = new
			{
				Item = new
				{
					UserName = username,
					Password = password
				}
			};

			try {

				string results= ConnectWebAPI.Request(url,logon);

				obj = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginJson> (results);

				return obj;
				
			} catch (Exception ex) {

				return null;
			}
		}
	}
}

