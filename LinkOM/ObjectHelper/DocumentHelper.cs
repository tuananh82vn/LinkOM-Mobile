using System;
using System.Collections.Generic;

namespace LinkOM
{
	public static class DocumentHelper
	{
		public static List<DocumentList> GetDocumentList(DocumentFilter objDocument)
		{

			string url = Settings.InstanceURL;

			url=url+"/api/DocumentList";


			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						PageSize = 100,
						PageNumber = 1,
						Item = objDocument
					}
				});

			string results=  ConnectWebAPI.Request(url,objsearch);

			if (results != null) {

				ApiResultList<IEnumerable<DocumentList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<DocumentList>>> (results);

				List<DocumentList> returnObject = new List<DocumentList> ();
				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						DocumentList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<DocumentList> (Item.ToString ());
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

		public static DocumentDetailList GetDocumentDetail(int DocumentId){
			string url = Settings.InstanceURL;

			//Load data
			string url_Task = url + "/api/DocumentDetailList";


			var objDocument = new
			{
				Id = DocumentId
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						Item = objDocument
					}
				});

			string results_Task = ConnectWebAPI.Request (url_Task, objsearch);

			if (results_Task != null && results_Task != "") {

				ApiResultDetail<DocumentDetailList> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultDetail<DocumentDetailList>> (results_Task);
				if (objResult.Success) {

					DocumentDetailList returnObject = (DocumentDetailList)objResult.Item;
					return returnObject;
				}
				else
					return null;

			} else
				return null;
		}
	}
}

