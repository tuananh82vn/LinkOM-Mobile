using System;
using System.Collections.Generic;

namespace LinkOM
{
	public static class TaskHelper
	{
		public static List<TaskList> GetTaskList(TaskFilter objTask){

			string url = Settings.InstanceURL;

			//Load data
			string url_Task= url+"/api/TaskList";

			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "T.AssignedToMeOrder", Direction = "1"},
				new objSort{ColumnName = "T.PriorityId", Direction = "2"},
				new objSort{ColumnName = "T.EndDate", Direction = "1"},
				new objSort{ColumnName = "T.ProjectName", Direction = "1"},
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

				ApiResultList<IEnumerable<TaskList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<TaskList>>> (results_Task);

				List<TaskList> returnObject = new List<TaskList> ();

				foreach (object Item in objResult.Items) {
					TaskList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskList> (Item.ToString ());
					returnObject.Add (temp);
				}

				return returnObject;
			} else
				return null;
			
		}

		public static List<TaskCommentObject> GetTaskCommentList(int TaskId){
			
			string url = Settings.InstanceURL;

			//Load data
			string url_Task= url+"/api/TaskCommentList";


			var objTask = new
			{
				TaskId = TaskId,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber =Settings.Token,
						Item = objTask
					}
				});

			string results_Task= ConnectWebAPI.Request(url_Task,objsearch);

			if (results_Task != null && results_Task != "") {

				ApiResultList<IEnumerable<TaskCommentObject>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<TaskCommentObject>>> (results_Task);

				List<TaskCommentObject> returnObject = new List<TaskCommentObject> ();

				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						TaskCommentObject temp = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskCommentObject> (Item.ToString ());
						returnObject.Add (temp);
					}
				}
				else
					return null;

				return returnObject;
			} else
				return null;
			
		}

		public static TaskDetailList GetTaskDetail(int TaskId){
			string url = Settings.InstanceURL;

			//Load data
			string url_Task = url + "/api/TaskDetailList";


			var objTask = new
			{
				Id = TaskId
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

				ApiResultDetail<TaskDetailList> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultDetail<TaskDetailList>> (results_Task);
				if (objResult.Success) {

					TaskDetailList returnObject = (TaskDetailList)objResult.Item;
					return returnObject;
				}
				else
					return null;

			} else
				return null;
		}

		public static List<Status> GetTaskStatus(){

			string url = Settings.InstanceURL;

			string url_TaskStatusList= url+"/api/TaskStatusList";

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

