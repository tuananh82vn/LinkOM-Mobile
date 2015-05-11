using System;
using System.Collections.Generic;

namespace LinkOM
{
	public static class MilestonesHelper
	{
		public static List<MilestonesList> GetMilestonesList(MilestoneFilter objFilter)
		{
			
			var url = Settings.InstanceURL;

			url=url+"/api/MilestoneList";


			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "P.Name", Direction = "1"},
				new objSort{ColumnName = "M.EndDate", Direction = "2"},
				new objSort{ColumnName = "M.MileStoneStatusId", Direction = "1"},
				new objSort{ColumnName = "M.PriorityId", Direction = "1"},
			};


			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						IsShowAll = true,
						PageSize = 100,
						PageNumber = 1,
						Sort = objSort,
						Item = objFilter
					}
				});

			string results=  ConnectWebAPI.Request(url,objsearch);

			if (results != null) {

				ApiResultList<IEnumerable<MilestonesList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<MilestonesList>>> (results);

				List<MilestonesList> returnObject = new List<MilestonesList> ();
				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						MilestonesList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<MilestonesList> (Item.ToString ());
						returnObject.Add (temp);
					}

					return returnObject;
				}
				else
					return null;
			} else
				return null;
		}

		public static MilestonesDetailList GetMilestonesDetail(int MilestoneId){
			string url = Settings.InstanceURL;

			//Load data
			string url_Task = url + "/api/MilestoneDetailList";


			var objTask = new
			{
				Id = MilestoneId
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

				ApiResultDetail<MilestonesDetailList> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultDetail<MilestonesDetailList>> (results_Task);
				if (objResult.Success) {

					MilestonesDetailList returnObject = (MilestonesDetailList)objResult.Item;
					return returnObject;
				}
				else
					return null;

			} else
				return null;
		}



	}
}

