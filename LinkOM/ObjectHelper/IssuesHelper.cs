using System;
using System.Collections.Generic;

namespace LinkOM
{
	public static class IssuesHelper
	{
		public static List<IssuesList> GetIssuesList(IssuesFilter objIssues)
		{
			string url = Settings.InstanceURL;

			//Load data
			string url_Issues= url+"/api/IssueList";

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber =Settings.Token,
						PageSize = 100,
						PageNumber = 1,
						Item = objIssues
					}
				});


			string results_Issues= ConnectWebAPI.Request(url_Issues,objsearch);

			if (results_Issues != null) {

				ApiResultList<IEnumerable<IssuesList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<IssuesList>>> (results_Issues);

				List<IssuesList> returnObject = new List<IssuesList> ();
				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						IssuesList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<IssuesList> (Item.ToString ());
						returnObject.Add (temp);
					}
				}
				return returnObject;

			} 
			else
				return null;
		}
	}
}

