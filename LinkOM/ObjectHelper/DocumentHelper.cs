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
	}
}

