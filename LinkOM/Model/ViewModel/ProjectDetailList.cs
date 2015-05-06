using System;
using System.ComponentModel;
using Android.Util;

namespace LinkOM
{
	[Serializable]
	public class ProjectDetailList
	{
		public virtual int? ProjectId { get; set; }
		public virtual string ProjectCode { get; set; }
		public virtual string Guid { get; set; }
		public virtual string ProjectName { get; set; }
		public virtual string ReferenceCode { get; set; }
		public virtual int? ClientId { get; set; }
		public virtual string ClientName { get; set; }
		public virtual int? DepartmentId { get; set; }
		public virtual string DepartmentName { get; set; }
		public virtual string DepartmentColor { get; set; }
		public virtual DateTime? StartDate { get; set; }
		public virtual DateTime? EndDate { get; set; }
		public virtual DateTime? ActualStartDate { get; set; }
		public virtual DateTime? ActualEndDate { get; set; }
		public virtual int? ProjectStatusId { get; set; }
		public virtual string ProjectStatus { get; set; }
		public virtual string ProjectStatusColor { get; set; }
		public virtual int? ProjectMainStatus { get; set; }
		public virtual double? AllocatedHours { get; set; }
		public virtual decimal? ActualHrs { get; set; }
		public virtual int? DeliveryManagerId { get; set; }
		public virtual string DeliveryManagerName { get; set; }
		public virtual int? ProjectManagerId { get; set; }
		public virtual string ProjectManagerName { get; set; }
		public virtual int? ProjectCoordinatorId { get; set; }
		public virtual string ProjectCoordinatorName { get; set; }
		public virtual string Notes { get; set; }
		public virtual string Description { get; set; }


		public virtual int? TaskOverdueCount { get; set; }
		public virtual int? TaskOpenCount { get; set; }
		public virtual int? TaskClosedCount { get; set; }
		public virtual int? TaskOtherCount { get; set; }
		public virtual int? TaskTotalCount { get; set; }

		public virtual int? TicketOverdueCount { get; set; }
		public virtual int? TicketOpenCount { get; set; }
		public virtual int? TicketClosedCount { get; set; }
		public virtual int? TicketOtherCount { get; set; }
		public virtual int? TicketTotalCount { get; set; }

		public virtual int? IssueOverdueCount { get; set; }
		public virtual int? IssueOpenCount { get; set; }
		public virtual int? IssueClosedCount { get; set; }
		public virtual int? IssueOtherCount { get; set; }
		public virtual int? IssueTotalCount { get; set; }

		public virtual int? MilestoneOverdueCount { get; set; }
		public virtual int? MilestoneOpenCount { get; set; }
		public virtual int? MilestoneClosedCount { get; set; }
		public virtual int? MilestoneOtherCount { get; set; }
		public virtual int? MilestoneTotalCount { get; set; }

		public virtual string StartDateString { get; set; }
		public virtual string EndDateString { get; set; }

		public virtual string ActualStartDateString { get; set; }
		public virtual string ActualEndDateString { get; set; }

	}
}

