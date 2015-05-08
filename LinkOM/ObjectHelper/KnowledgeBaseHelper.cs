using System;
using System.Collections.Generic;

namespace LinkOM
{
	public static class KnowledgeBaseHelper
	{
		public static List<KnowledgebaseList> GetKnowledgebaseList(KnowledgebaseFilter objFilter)
		{

			string url = Settings.InstanceURL;

			url=url+"/api/KnowledgeBaseList";


			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "K.Title", Direction = "1"},
				new objSort{ColumnName = "P.ProjectId", Direction = "2"}
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						PageSize = 20,
						PageNumber = 1,
						Sort = objSort,
						Item = objFilter
					}
				});

			string results=  ConnectWebAPI.Request(url,objsearch);

			if (results != null) {

				ApiResultList<IEnumerable<KnowledgebaseList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<KnowledgebaseList>>> (results);

				List<KnowledgebaseList> returnObject = new List<KnowledgebaseList> ();
				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						KnowledgebaseList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<KnowledgebaseList> (Item.ToString ());
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

