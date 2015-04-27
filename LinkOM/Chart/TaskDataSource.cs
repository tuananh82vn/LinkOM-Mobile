﻿
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
	public class TaskDataSource : Activity, INChartSeriesDataSource , INChartValueAxisDataSource, INChartDelegate
	{
		Random random = new Random ();

		NChartPoint prevSelectedPoint;

		public TaskList taskList;

		public string[] TaskStatusName;

		public StatusList statusList;

		public TaskDataSource(){
			InitData ();
		}

		public void InitData (){
			
			string url = Settings.InstanceURL;

			//Load data
			string url_Task= url+"/api/TaskList";

			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "T.ProjectName", Direction = "1"},
				new objSort{ColumnName = "T.EndDate", Direction = "2"}
			};


			var objTask = new
			{
				Title = string.Empty,
				AssignedToId = Settings.UserId,
				ClientId = string.Empty,
				TaskStatusId = string.Empty,
				PriorityId = string.Empty,
				DueBeforeDate = string.Empty,
				DepartmentId = string.Empty,
				ProjectId = string.Empty,
				AssignByMe = true,
				Filter = string.Empty,
				Label = string.Empty,
			};

			var objsearch = (new
				{
					objApiSearch = new
					{
						UserId = Settings.UserId,
						TokenNumber =Settings.Token,
						PageSize = 100,
						PageNumber = 1,
						Sort = objSort,
						Item = objTask
					}
				});

			string results_Task= ConnectWebAPI.Request(url_Task,objsearch);

			if (results_Task != null && results_Task != "") {

				taskList = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskList> (results_Task);

			}


			string url_TaskStatusList= url+"/api/TaskStatusList";

			string results_TaskList= ConnectWebAPI.Request(url_TaskStatusList,"");

			if (results_TaskList != null && results_TaskList != "") {

				JsonData data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonData> (results_TaskList);

				statusList = Newtonsoft.Json.JsonConvert.DeserializeObject<StatusList> (data.Data);

				if (statusList.Items.Count > 0) {

					TaskStatusName = new string[statusList.Items.Count];

					for (int i = 0; i < statusList.Items.Count; i++) {
						TaskStatusName[i]=statusList.Items [i].Name;
						//Get number of task
						var NumberOfTask = CheckTask (statusList.Items [i].Name, taskList.Items).ToString ();
					}
				}
			}
		}

		private int CheckTask(string status, List<TaskObject>  list_Task){
			int count = 0;
			foreach (var task in list_Task) {
				if (task.StatusName == status)
					count++;
			}
			return count;
		}

		public string Name (NChartSeries series)
		{
			return "Task";
		}


		public string Name (NChartValueAxis nChartValueAxis) {
			if (nChartValueAxis.Kind == NChartValueAxisKind.X)
				return "";
			else if (nChartValueAxis.Kind == NChartValueAxisKind.Y)
				return "Status";
			return null;
		}

		public string[] Ticks (NChartValueAxis nChartValueAxis)
		{
			if (nChartValueAxis.Kind == NChartValueAxisKind.Y)
				return TaskStatusName;
			
			return null;
		}

		public NChartPoint[] Points (NChartSeries series)
		{
			NChartPoint[] result = new NChartPoint[statusList.Items.Count];

			for (int i = 0; i < statusList.Items.Count; i++) {
				//Get number of task
				var NumberOfTask = CheckTask (statusList.Items [i].Name, taskList.Items);

				result [i] = new NChartPoint (NChartPointState.PointStateAlignedToYWithXY (NumberOfTask, i), series);
			}


//			for (int i = 0; i <= 4; ++i)
//				result [i] = new NChartPoint (NChartPointState.PointStateAlignedToYWithXY (random.Next (30) + 1, i), series);

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