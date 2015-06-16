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
				new objSort{ColumnName = "P.Name", Direction = "1"},
				new objSort{ColumnName = "T.EndDate", Direction = "1"},
				new objSort{ColumnName = "T.Id", Direction = "1"},
				new objSort{ColumnName = "T.TicketStatusId", Direction = "1"},
				new objSort{ColumnName = "T.PriorityId", Direction = "1"},
			};


			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber =Settings.Token,
						IsShowAll = true,
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

		public static TicketDetailList GetTicketDetail(int TicketId){
			string url = Settings.InstanceURL;

			//Load data
			string url_Ticket = url + "/api/TicketDetailList";


			var objTicket = new
			{
				Id = TicketId
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						Item = objTicket
					}
				});

			string results_Task = ConnectWebAPI.Request (url_Ticket, objsearch);

			if (results_Task != null && results_Task != "") {

				ApiResultDetail<TicketDetailList> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultDetail<TicketDetailList>> (results_Task);
				if (objResult.Success) {

					TicketDetailList returnObject = (TicketDetailList)objResult.Item;
					return returnObject;
				}
				else
					return null;

			} else
				return null;
		}

		public static List<Status> GetTicketStatusList(){

			string url = Settings.InstanceURL;

			string url_TicketStatusList= url+"/api/TicketStatusList";

			string results_TicketkList= ConnectWebAPI.Request(url_TicketStatusList,"");

			if (results_TicketkList != null && results_TicketkList != "") {

				JsonData data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results_TicketkList);

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


		public static List<Status> GetReceivedMethodList(){

			string url = Settings.InstanceURL;

			string url_TicketStatusList= url+"/api/TicketReceivedMethodList";

			string results_TicketkList= ConnectWebAPI.Request(url_TicketStatusList,"");

			if (results_TicketkList != null && results_TicketkList != "") {

				JsonData data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results_TicketkList);

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

		public static List<Status> GetTicketTypeList(){

			string url = Settings.InstanceURL;

			string url_TicketStatusList= url+"/api/TicketTypeList";

			string results_TicketkList= ConnectWebAPI.Request(url_TicketStatusList,"");

			if (results_TicketkList != null && results_TicketkList != "") {

				JsonData data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results_TicketkList);

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

		public static List<TicketCommentList> GetTicketCommentList(int TicketId){
			
			string url = Settings.InstanceURL;

			//Load data
			string url_Task= url+"/api/TicketCommentList";


			var objTask = new
			{
				TicketId = TicketId,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber =Settings.Token,
						Item = objTask
					}
				});

			string results_TicketkList= ConnectWebAPI.Request(url_Task,objsearch);

			if (results_TicketkList != null && results_TicketkList != "") {



				ApiResultList<IEnumerable<TicketCommentList>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<TicketCommentList>>> (results_TicketkList);

				List<TicketCommentList> returnObject = new List<TicketCommentList> ();
				if (objResult.Items != null) {
					foreach (object Item in objResult.Items) {
						TicketCommentList temp = Newtonsoft.Json.JsonConvert.DeserializeObject<TicketCommentList> (Item.ToString ());
						returnObject.Add (temp);
					}
				}
				else
					return null;

				return returnObject;
			} else
				return null;
		}

		public static async System.Threading.Tasks.Task<ApiResultSave> EditTicket(TicketEdit editTicket)
		{

			ApiResultSave apiResultSave = new ApiResultSave();

			apiResultSave = await new WebApiHelper().EditAddObject("/API/EditTicket",editTicket);

			return apiResultSave;
		}

		public static async System.Threading.Tasks.Task<ApiResultSave> AddTicket(TicketAdd addTicket)
		{

			ApiResultSave apiResultSave = new ApiResultSave();

			apiResultSave = await new WebApiHelper().EditAddObject("/API/AddTicket",addTicket);

			return apiResultSave;
		}


	}
}

