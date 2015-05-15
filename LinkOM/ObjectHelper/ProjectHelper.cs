using System;
using System.Collections.Generic;

namespace LinkOM
{
	public static class ProjectHelper
	{

		public static List<ProjectList> GetProjectList(){
			
			string url = Settings.InstanceURL;

			url=url+"/api/ProjectList";


			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "P.Name", Direction = "1"},
				new objSort{ColumnName = "P.ProjectStatusId", Direction = "2"}
			};

			var objProject = new
			{

				IsShowAll = true,
				ProjectName = string.Empty,
				MainStatusId = string.Empty,
				ProjectStatusId	 = string.Empty,
				DepartmentId	 = string.Empty,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						PageSize = 100,
						PageNumber = 1,
						Sort = objSort,
						Item = objProject
					}
				});

			string results=  ConnectWebAPI.Request(url,objsearch);

			if (results != null) {

				ApiResultList<IEnumerable<ProjectList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<ProjectList>>> (results);

				List<ProjectList> returnObject = new List<ProjectList> ();

				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						ProjectList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectList> (Item.ToString ());
						returnObject.Add (temp);
					}
				}
				else
					return null;

				return returnObject;
			} else
				return null;
		}

		public static ProjectDetailList GetProjectDetailById(int projectId)
		{

			ProjectDetailList returnObject = new ProjectDetailList ();

			string url = Settings.InstanceURL;

			url=url+"/api/ProjectDetailList";


			var objProject = new
			{
				IsShowAll = true,
				ProjectId	 = projectId,
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
				ApiResultSingle<IEnumerable<ProjectDetailList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultSingle<IEnumerable<ProjectDetailList>>> (results);
				if (objResult.Success) {
					returnObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectDetailList> (objResult.Item.ToString());
				}
			}

			return returnObject;
		}

		public static List<Status> GetProjectStatus(){

			string url = Settings.InstanceURL;

			string url_TaskStatusList= url+"/api/ProjectStatusList";

			string results_TaskList= ConnectWebAPI.Request(url_TaskStatusList,"");

			if (results_TaskList != null && results_TaskList != "") {

				JsonData data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results_TaskList);

				if (data.Data != null) {

					ApiResultList<IEnumerable<Status>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<Status>>> (data.Data);

					List<Status> returnObject = new List<Status> ();

					if (objResult.Items != null) {
						foreach (object Item in objResult.Items) {
							Status temp = Newtonsoft.Json.JsonConvert.DeserializeObject<Status> (Item.ToString ());
							returnObject.Add (temp);
						}
					}
					else
						return null;

					return returnObject;
				} else
					return null;
			} else
				return null;
		}
	}
}

