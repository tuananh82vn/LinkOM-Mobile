using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkOM
{
	public static class ChartHelper
	{
		public static List<ProjectTicketChart>  GetProjectTicketChart ()
		{

			string url = Settings.InstanceURL;

			url = url + "/api/ProjectTicketChart";

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						IsShowAll = false
					}
				});

			string results = ConnectWebAPI.Request (url, objsearch);

			if (results != null) {

				ApiResultList<IEnumerable<ProjectTicketChart>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<ProjectTicketChart>>> (results);

				List<ProjectTicketChart> returnObject = new List<ProjectTicketChart> ();
				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						ProjectTicketChart temp = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectTicketChart> (Item.ToString ());
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

		public static List<ProjectTaskChart>  GetProjectTaskChart ()
		{

			string url = Settings.InstanceURL;

			url = url + "/api/ProjectTaskChart";

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						IsShowAll = false
					}
				});

			string results = ConnectWebAPI.Request (url, objsearch);

			if (results != null) {

				ApiResultList<IEnumerable<ProjectTaskChart>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<ProjectTaskChart>>> (results);

				List<ProjectTaskChart> returnObject = new List<ProjectTaskChart> ();
				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						ProjectTaskChart temp = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectTaskChart> (Item.ToString ());
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

