using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkOM
{
	public static class DashboardHelper
	{
		public static DashboardObject GetDashboard ()
		{

			string url = Settings.InstanceURL;

			url = url + "/api/DashboardList";

			var objDashBoard = new
			{
				AssignedToId = Settings.UserId,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						TokenNumber = Settings.Token,
						Item = objDashBoard
					}
				});

			string results = ConnectWebAPI.Request (url, objsearch);

			if (results != null) {

				ApiResultList<IEnumerable<DashboardObject>> objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResultList<IEnumerable<DashboardObject>>> (results);
				if (objResult.Success) {

					DashboardObject temp = Newtonsoft.Json.JsonConvert.DeserializeObject<DashboardObject> (objResult.Items.FirstOrDefault ().ToString ());
					return temp;
				} else
					return null;
			} else
				return null;
		}

	}
}

