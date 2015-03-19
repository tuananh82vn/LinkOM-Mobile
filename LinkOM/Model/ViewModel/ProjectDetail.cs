using System;
using System.ComponentModel;
using Android.Util;

namespace LinkOM
{
	[Serializable]
	public class ProjectDetail
	{
		[DisplayName("Id")]
		public virtual Int32 ProjectId  { get; set; }

		public virtual string ProjectGuid { get; set; }

		[DisplayName("Name")]
		public virtual String ProjectName  { get; set; }

		[DisplayName("Client Name")]
		public virtual String ClientName { get; set; }

		[DisplayName("Department")]
		public virtual String DepartmentName { get; set; }

		[DisplayName("Department")]
		public virtual Int32 DepartmentId  { get; set; }

		[DisplayName("Delivery Manager")]
		public virtual Int32 DeliveryManagerId  { get; set; }

		[DisplayName("Delivery Manager")]
		public virtual String DeliveryManagerName { get; set; }


		[DisplayName("Project Coordinator")]
		public virtual String ProjectCoordinatorName { get; set; }


		[DisplayName("Project Manager")]
		public virtual Int32 ProjectManagerId  { get; set; }

		[DisplayName("Project Manager")]
		public virtual String ProjectManagerName { get; set; }


		public virtual String OverdueDays  { get; set; }

		[DisplayName("Project Coordinator")]
		public virtual Int32 ProjectCoordinatorId  { get; set; }

		[DisplayName("Project Status")]
		public virtual Int32? ProjectStatusId { get; set; }


		[DisplayName("Status")]
		public virtual String  ProjectStatus  { get; set; }

		public virtual String ProjectStatusSortName { get; set; }


		[DisplayName("Status")]
		public virtual String ProjectPhase { get; set; }

		[DisplayName("Status")]
		public virtual String ProjectPhaseColour { get; set; }                   

		public virtual DateTime? StartDate  { get; set; }

		[DisplayName("Start Date")]    			  
		public virtual String StartDateString  { get; set; }

		public virtual DateTime? EndDate { get; set; }

		[DisplayName("End Date")]
		public virtual String EndDateString  { get; set; }

		public virtual int CountOverDueProjects { get; set; }

		public virtual String OverdueFlagClass  { get; set; }

		[DisplayName("Actual Start Date")]
		public virtual DateTime? ActualStartDate  { get; set; }

		[DisplayName("Actual Start Date")]
		public virtual String ActualStartDateString  { get; set; }

		[DisplayName("Actual End Date")]
		public virtual DateTime? ActualEndDate  { get; set; }

		[DisplayName("Actual End Date")]
		public virtual String ActualEndDateString { get; set; }


		[DisplayName("Allocated Hours")]
		public virtual Double? AllocatedHours  { get; set; }

		[DisplayName("Notes")]
		public virtual String Notes  { get; set; }

		[DisplayName("Description")]
		public virtual String Description  { get; set; }

		[DisplayName("Reference Code")]
		public virtual String ReferenceCode { get; set; }

		[DisplayName("Code")]
		public virtual String Code { get; set; }

		[DisplayName("Created By")]
		public virtual String CreatedBy  { get; set; }

		[DisplayName("Created Date")]
		public virtual DateTime CreatedDate  { get; set; }

		[DisplayName("Updated By")]
		public virtual String UpdatedBy  { get; set; }

		[DisplayName("Updated Date")]
		public virtual DateTime UpdatedDate  { get; set; }

		[DisplayName("Ticket Status")]
		public virtual String ProjectStatusColor { get; set; }

		public virtual Int32 UserId { get; set; }

		public virtual int? TaskOverdueCount { get; set; }
		public virtual int?  TaskOpenCount{ get; set; } 
		public virtual int?  TaskClosedCount{ get; set; }
		public virtual int?  TaskOtherCount{ get; set; }
		public virtual int?  TaskTotalCount	{ get; set; }

		public virtual int?  TicketOverdueCount { get; set; }
		public virtual int?  TicketOpenCount{ get; set; }
		public virtual int?  TicketClosedCount{ get; set; }
		public virtual int?  TicketOtherCount{ get; set; }
		public virtual int?  TicketTotalCount{ get; set; }

		public virtual int?  IssueOverdueCount { get; set; }
		public virtual int?  IssueOpenCount{ get; set; }
		public virtual int?  IssueClosedCount{ get; set; }
		public virtual int?  IssueOtherCount{ get; set; }
		public virtual int?  IssueTotalCount{ get; set; }

		public virtual int?  MilestoneOverdueCount { get; set; }
		public virtual int?  MilestoneOpenCount{ get; set; }
		public virtual int?  MilestoneClosedCount{ get; set; }
		public virtual int?  MilestoneOtherCount{ get; set; }
		public virtual int? MilestoneTotalCount { get; set; }

	}
}

