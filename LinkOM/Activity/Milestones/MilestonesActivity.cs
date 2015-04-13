
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MonoDroid.TimesSquare;

namespace LinkOM
{
	[Activity (Label = "MilestonesActivity")]			
	public class MilestonesActivity : Activity
	{
//		CustomCalendar CalendarControl;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Milestones);


			var BackButton = FindViewById(Resource.Id.BackButton);
			BackButton.Click += btBackClick;

			var nextYear = DateTime.Now.AddYears(1);

			var calendar = FindViewById<CalendarPickerView>(Resource.Id.calendar_view);

			List<DateTime> milestone = new List<DateTime> ();
			DateTime nextday1 = DateTime.Now.AddDays(1);
			DateTime nextday2 = DateTime.Now.AddDays(3);
			DateTime nextday3 = DateTime.Now.AddDays(5);
			milestone.Add (nextday1);
			milestone.Add (nextday2);
			milestone.Add (nextday3);

			calendar.Init (DateTime.Now.AddYears (-1), DateTime.Now.AddYears (1))
				.InMode (CalendarPickerView.SelectionMode.Single)
				.WithHighlightedDates(milestone)
				.WithSelectedDate (DateTime.Now);

			//calendar.HighlightDates (milestone);



			//calendar.HighlightDates (milestone);


//			CalendarControl = FindViewById<CustomCalendar>(Resource.Id.CalendarControl);
//			CalendarControl.NextButtonText= "Next";
//			CalendarControl.PreviousButtonText= "Prev";

			//CalendarControl.NextButtonVisibility= ViewStates.Invisible;
			//CalendarControl.PreviousButtonStyleId = Resource.Drawable.default_dim_selector;

			//CalendarControl.ShowOnlyCurrentMonth = true;
//			CalendarControl.ShowFromDate = new DateTime();


//			List<CustomCalendarData> customData = new List<CustomCalendarData>();
//
//			customData = new List<CustomCalendarData>
//			{
//				new CustomCalendarData(DateTime.Now.AddDays(2)),
//				new CustomCalendarData(DateTime.Now.AddDays(4)),
//				new CustomCalendarData(DateTime.Now.AddDays(-4))
//			};
//			CalendarControl.CustomDataAdapter = customData;
//
//
//			CalendarControl.OnCalendarMonthChange += CalendarControl_CalendarMonthChange;
//			CalendarControl.OnCalendarSelectedDate += CalendarControl_CalendarDateSelected;

		}

		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}

//		private void CalendarControl_CalendarDateSelected(object sender, CalendarDateSelectionEventArgs e)
//		{
//			Toast.MakeText(this, e.SelectedDate.ToString(), ToastLength.Short).Show();
//		}

//		private void CalendarControl_CalendarMonthChange(object sender, CalendarNavigationEventArgs e)
//		{
//			if (e.MonthChange == CalendarHelper.MonthChangeOn.Next)
//			{
//
//			}
//			else if (e.MonthChange == CalendarHelper.MonthChangeOn.Previous)
//			{
//
//			}
//		}
	}
}

