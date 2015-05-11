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
				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						IssuesCommentList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<IssuesCommentList> (Item.ToString ());
						returnObject.Add (temp);
					}
				}
				else
					return null;

				return returnObject;
			} 
			else
				return null;
		}

		public static List<Status> GetIssuesStatusList(){

			string url = Settings.InstanceURL;

			string url_TicketStatusList= url+"/api/IssueStatusList";

			string results_TicketkList= ConnectWebAPI.Request(url_TicketStatusList,"");


			if (results_TicketkList != null && results_TicketkList != "") {

				JsonData data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results_TicketkList);

				if (data.Data != null) {

					ApiResultList<IEnumerable<Status>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<Status>>> (data.Data);

					List<Status> returnObject = new List<Status> ();

					if (objResult.Items != null) {
						foreach (object Item in objResult.Items) {
							Status temp = Newtonsoft.Json.JsonConvert.DeserializeObject<Status> (Item.ToString ());
							returnObject.Add (temp);
						}
					}
					else
						return null;

					return returnObject;
				} else
					return null;
			} else
				return null;
		}

		public static IssuesDetailList GetIssuesDetail(int IssuesId){
			string url = Settings.InstanceURL;

			//Load data
			string url_Task = url + "/api/IssueDetailList";


			var objTask = new
			{
				Id = IssuesId
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						Item = objTask
					}
				});

			string results_Task = ConnectWebAPI.Request (url_Task, objsearch);

			if (results_Task != null && results_Task != "") {

				ApiResultDetail<IssuesDetailList> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultDetail<IssuesDetailList>> (results_Task);
				if (objResult.Success) {

					IssuesDetailList returnObject = (IssuesDetailList)objResult.Item;
					return returnObject;
				}
				else
					return null;

			} else
				return null;
		}


	}
}

