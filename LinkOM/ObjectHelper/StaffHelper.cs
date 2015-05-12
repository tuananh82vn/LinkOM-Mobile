using System;
using System.Collections.Generic;

namespace LinkOM
{
	public static class StaffHelper
	{
		public static List<StaffList> GetAssignedToByProject(SearchAssignedByProject objFilter){
			
			string url = Settings.InstanceURL;

			url=url+"/api/GetAssignedToByProject";


			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						Item = objFilter
					}
				});

			string results=  ConnectWebAPI.Request(url,objsearch);

			if (results != null) {

				JsonData DataObject = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results);

				ApiResultList<IEnumerable<StaffList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<StaffList>>> (DataObject.Data);

				List<StaffList> returnObject = new List<StaffList> ();

				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						StaffList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<StaffList> (Item.ToString ());
						returnObject.Add (temp);
					}
				}
				else
					return null;
				
				return returnObject;
			} else
				return null;
		}

		public static List<StaffList> GetOwnerByProject(SearchAssignedByProject objFilter){

			string url = Settings.InstanceURL;

			url=url+"/api/GetOwnerByProject";


			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						Item = objFilter
					}
				});

			string results=  ConnectWebAPI.Request(url,objsearch);

			if (results != null) {

				JsonData DataObject = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results);

				ApiResultList<IEnumerable<StaffList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<StaffList>>> (DataObject.Data);

				List<StaffList> returnObject = new List<StaffList> ();

				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						StaffList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<StaffList> (Item.ToString ());
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

