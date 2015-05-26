using System;
using System.Collections.Generic;

namespace LinkOM
{
	public static class LabelHelper
	{
		public static List<ProjectLabelList> GetProjectLabelByProject(int ProjectId){
			
			string url = Settings.InstanceURL;

			url=url+"/api/GetProjectLabelByProject";

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

				ApiResultList<IEnumerable<ProjectLabelList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<ProjectLabelList>>> (DataObject.Data);

				List<ProjectLabelList> returnObject = new List<ProjectLabelList> ();

				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						ProjectLabelList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectLabelList> (Item.ToString ());
						returnObject.Add (temp);
					}
				}
				else
					return null;
				
				return returnObject;
			} else
				return null;
		}
	}
}

