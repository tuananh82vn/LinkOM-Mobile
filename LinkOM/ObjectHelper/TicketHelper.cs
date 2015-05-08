using System;
using System.Collections.Generic;

namespace LinkOM
{
	public static class TicketHelper
	{
		public static List<TicketList> GetTicketList(TicketFilter objTask){

			string url = Settings.InstanceURL;

			//Load data
			string url_Task= url+"/api/TicketList";

			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "T.Code", Direction = "2"},
			};


			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber =Settings.Token,
						PageSize = 100,
						PageNumber = 1,
						Sort = objSort,
						Item = objTask
					}
				});

			string results_Task= ConnectWebAPI.Request(url_Task,objsearch);

			if (results_Task != null && results_Task != "") {

				ApiResultList<IEnumerable<TicketList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<TicketList>>> (results_Task);

				List<TicketList> returnObject = new List<TicketList> ();
				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						TicketList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<TicketList> (Item.ToString ());
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

