
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

		public List<TaskList> taskList;

		public string[] TaskStatusName;

		public List<Status> statusList;

		public TaskDataSource(){
			InitData ();
		}

		public void InitData (){
			
			TaskFilter objFilter = new TaskFilter ();

			objFilter.AssignedToId = Settings.UserId;
				
			taskList = TaskHelper.GetTaskList (objFilter);

			statusList = TaskHelper.GetTaskStatus ();


			if (statusList!=null) {

				TaskStatusName = new string[statusList.Count];
				int NumberOfTask = 0;

				if (taskList != null) {
					for (int i = 0; i < statusList.Count; i++) {
						TaskStatusName [i] = statusList [i].Name;
					}
				}
			}

		}

		private int CheckTask(string status, List<TaskList>  list_Task){
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
			NChartPoint[] result = new NChartPoint[statusList.Count];

			for (int i = 0; i < statusList.Count; i++) {
				//Get number of task
				int NumberOfTask =0;

				if(taskList!=null)
					NumberOfTask = CheckTask (statusList [i].Name, taskList);

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