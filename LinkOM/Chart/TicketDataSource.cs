
using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Java.Lang;

using NChart3D_Android;
using System.Collections.Generic;

namespace LinkOM
{
	public class TicketDataSource : Activity, INChartSeriesDataSource , INChartValueAxisDataSource, INChartDelegate
	{
		Random random = new Random ();

		NChartPoint prevSelectedPoint;

		public TicketList taskList;

		public string[] TicketStatusName;

		public StatusList statusList;

		public TicketDataSource(){
			InitData ();
		}

		public void InitData (){
			
			string url = Settings.InstanceURL;

			//Load data
			string url_Ticket= url+"/api/TicketList";

			var objTicket = new
			{
				ProjectId = string.Empty,
				AssignedToId = Settings.UserId,
				TicketStatusId = string.Empty,
				DepartmentId = string.Empty,
				Title = string.Empty,
				PriorityId = string.Empty,
				Label= string.Empty,
				DueBefore = string.Empty,
				AssignTo = string.Empty,
				AssignByMe = string.Empty,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						UserId = Settings.UserId,
						TokenNumber =Settings.Token,
						PageSize = 100,
						PageNumber = 1,
						SortMember ="",
						SortDirection = "",
						MainStatusId=1,
						Item = objTicket
					}
				});

			string results_Ticket= ConnectWebAPI.Request(url_Ticket,objsearch);

			if (results_Ticket != null && results_Ticket != "") {

				taskList = Newtonsoft.Json.JsonConvert.DeserializeObject<TicketList> (results_Ticket);

			}


			string url_TicketStatusList= url+"/api/TicketStatusList";

			string results_TicketkList= ConnectWebAPI.Request(url_TicketStatusList,"");

			if (results_TicketkList != null && results_TicketkList != "") {

				JsonData data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results_TicketkList);

				statusList = Newtonsoft.Json.JsonConvert.DeserializeObject<StatusList> (data.Data);

				if (statusList.Items.Count > 0) {

					TicketStatusName = new string[statusList.Items.Count];

					for (int i = 0; i < statusList.Items.Count; i++) {
						TicketStatusName[i]=statusList.Items [i].Name;
					}
				}
			}
		}

		private int CheckTicket(string status, List<TicketObject>  list_Task){
			int count = 0;
			foreach (var task in list_Task) {
				if (task.TicketStatusName == status)
					count++;
			}
			return count;
		}

		public string Name (NChartSeries series)
		{
			return "Ticket";
		}


		public string Name (NChartValueAxis nChartValueAxis) {
			if (nChartValueAxis.Kind == NChartValueAxisKind.X)
				return "";
			else if (nChartValueAxis.Kind == NChartValueAxisKind.Y)
				return "Status Ticket";
			return null;
		}

		public string[] Ticks (NChartValueAxis nChartValueAxis)
		{
			if (nChartValueAxis.Kind == NChartValueAxisKind.Y)
				return TicketStatusName;
			
			return null;
		}

		public NChartPoint[] Points (NChartSeries series)
		{
			NChartPoint[] result = new NChartPoint[statusList.Items.Count];

			for (int i = 0; i < statusList.Items.Count; i++) {
				//Get number of task
				var NumberOfTask = CheckTicket (statusList.Items [i].Name, taskList.Items);

				result [i] = new NChartPoint (NChartPointState.PointStateAlignedToYWithXY (NumberOfTask, i), series);
			}

			return result;
		}

		public Bitmap Image (NChartSeries series)
		{
			return null;
		}

		public Number Min (NChartValueAxis nChartValueAxis)
		{
			return null;
		}

		public Number Max (NChartValueAxis nChartValueAxis)
		{
			return null;
		}

		public Number Step (NChartValueAxis nChartValueAxis)
		{
			return null;
		}

		public Number Length (NChartValueAxis nChartValueAxis) {
			return null;
		}

		public string DoubleToString (double v, NChartValueAxis nChartValueAxis) {
			return null;
		}

		public void PointSelected (NChart nChart, NChartPoint nChartPoint)
		{
			if (prevSelectedPoint != null)
				prevSelectedPoint.Tooltip.SetVisibleAnimated (false, 0.25f);

			if (nChartPoint != null) {
				if (nChartPoint.Tooltip != null) {
					if (nChartPoint == prevSelectedPoint)
						prevSelectedPoint = null;
					else {
						prevSelectedPoint = nChartPoint;
						UpdateTooltipText (nChartPoint);
						nChartPoint.Tooltip.SetVisibleAnimated (true, 0.25f);
					}
				} else {
					prevSelectedPoint = nChartPoint;
					nChartPoint.Tooltip = CreateTooltip ();
					UpdateTooltipText (nChartPoint);
					nChartPoint.Tooltip.SetVisibleAnimated (true, 0.25f);
				}
			} else
				prevSelectedPoint = null;
		}

		public void TimeIndexChanged (NChart chart, double timeIndex)
		{
		}

		public void DidEndAnimating (NChart nChart, Java.Lang.Object o, NChartAnimationType animationType) { }

		NChartTooltip CreateTooltip ()
		{
			NChartTooltip result = new NChartTooltip ();

			result.Background = new NChartSolidColorBrush (Color.Argb (255, 255, 255, 255));
			result.Background.Opacity = 0.9;
			result.Padding = new NChartMargin (10.0f, 10.0f, 10.0f, 10.0f);
			result.BorderColor = Color.Argb (255, 128, 128, 128);
			result.BorderThickness = 1;
			result.Font = new NChartFont (16);
			result.Visible = false;

			return result;
		}

		void UpdateTooltipText(NChartPoint point)
		{
			point.Tooltip.Text = string.Format ("{0} : {1}", point.Series.Name, point.CurrentState.DoubleX);
		}

	}
}