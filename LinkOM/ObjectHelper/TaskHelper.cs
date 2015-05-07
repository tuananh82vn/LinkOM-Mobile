using System;
using System.Collections.Generic;

namespace LinkOM
{
	public static class TaskHelper
	{
		public static List<TaskObject> GetTaskList(){

			string url = Settings.InstanceURL;

			//Load data
			string url_Task= url+"/api/TaskList";

			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "T.AssignedToMeOrder", Direction = "1"},
				new objSort{ColumnName = "T.PriorityId", Direction = "2"},
				new objSort{ColumnName = "T.EndDate", Direction = "1"},
				new objSort{ColumnName = "T.ProjectName", Direction = "1"},
			};


			var objTask = new
			{
				Title = string.Empty,
				MainStatusId = string.Empty,
				AssignedToId = Settings.UserId,

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

				var taskList = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskList> (results_Task);
				return taskList.Items;
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

				foreach (object Item in objResult.Items) {
					TaskCommentObject temp = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskCommentObject> (Item.ToString ());
					returnObject.Add (temp);
				}

				return returnObject;
			} else
				return null;
			
		}

		public static TaskObject GetTaskDetail(int TaskId){
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

				TaskDetailJson taskDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskDetailJson> (results_Task);
				if (taskDetail.Success) {
					return taskDetail.Item;
				} else
					return null;
			} else
				return null;
		}

	}
}

