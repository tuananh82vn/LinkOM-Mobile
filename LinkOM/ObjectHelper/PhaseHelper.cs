using System;
using System.Collections.Generic;

namespace LinkOM
{
	public static class PhaseHelper
	{
		public static List<ProjectPhaseList> GetProjectPhaseByProject(int ProjectId){
			
			string url = Settings.InstanceURL;

			url=url+"/api/GetProjectPhaseByProject";

			var objProject = new
			{
				ProjectId = ProjectId
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						Item = objProject
					}
				});

			string results=  ConnectWebAPI.Request(url,objsearch);

			if (results != null) {

				JsonData DataObject = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results);

				ApiResultList<IEnumerable<ProjectPhaseList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<ProjectPhaseList>>> (DataObject.Data);

				List<ProjectPhaseList> returnObject = new List<ProjectPhaseList> ();

				foreach (object Item in objResult.Items) {
					ProjectPhaseList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectPhaseList> (Item.ToString ());
					returnObject.Add (temp);
				}

				return returnObject;
			} else
				return null;
		}
	}
}

