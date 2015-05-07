using System;
using System.Collections.Generic;

namespace LinkOM
{
	public static class MilestonesHelper
	{
		public static List<MilestonesList> GetMilestonesListByProjectId(int ProjectId)
		{
			
			var url = Settings.InstanceURL;

			url=url+"/api/MilestoneList";


			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "T.Title", Direction = "1"},
				new objSort{ColumnName = "T.ProjectName", Direction = "2"}
			};


			var objMilestone = new
			{
				ProjectId = ProjectId,
				StatusId = string.Empty,
				DepartmentId = string.Empty,
				Title = string.Empty,
				PriorityId = string.Empty,
				Label = string.Empty,
				DueBefore = string.Empty,
				AssignTo = string.Empty,
				AssignByMe = string.Empty,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						PageSize = 100,
						PageNumber = 1,
						Sort = objSort,
						Item = objMilestone
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

		public static List<MilestonesList> GetAllMilestonesList(MilestoneFilter objMilestone)
		{

			var url = Settings.InstanceURL;

			url=url+"/api/MilestoneList";


			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "T.EndDate", Direction = "1"}
			};


			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						PageSize = 100,
						PageNumber = 1,
						Sort = objSort,
						Item = objMilestone
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

	}
}

