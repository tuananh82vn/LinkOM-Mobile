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

		public static List<IssuesCommentList> GetIssuesCommentList(int IssueId)
		{
			string url = Settings.InstanceURL;

			//Load data
			string url_Task= url+"/api/IssueCommentList";


			var objTask = new
			{
				IssueId = IssueId,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber =Settings.Token,
						Item = objTask
					}
				});

			string results_Issue= ConnectWebAPI.Request(url_Task,objsearch);

			if (results_Issue != null) {

				ApiResultList<IEnumerable<IssuesCommentList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<IssuesCommentList>>> (results_Issue);

				List<IssuesCommentList> returnObject = new List<IssuesCommentList> ();

				foreach (object Item in objResult.Items) {
					IssuesCommentList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<IssuesCommentList> (Item.ToString ());
					returnObject.Add (temp);
				}

				return returnObject;
			} 
			else
				return null;
		}

	}
}

