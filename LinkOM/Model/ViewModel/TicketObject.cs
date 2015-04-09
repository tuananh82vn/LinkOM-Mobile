using System;

namespace LinkOM
{
	public class TicketObject
	{
		public virtual Int32 Id { get; set; }

		//public virtual Guid? Guid { get; set; }
		public virtual string Guid { get; set; }

		public virtual Int32 ProjectId { get; set; }

		public virtual String ProjectName { get; set; }

		public virtual string ProjectGuid { get; set; }


		public virtual String Title { get; set; }

		public virtual String OverdueDays { get; set; }

		public virtual int CountOverDueTickets { get; set; }

		public virtual int CountOverBudgetTickets { get; set; }


		public virtual String OverdueDaysString { get; set; }


		public virtual String OverdueFlagClass	{ get; set; }

		public virtual String TicketStatusOverdue { get; set; }

		public virtual Int32 TicketMainStatus { get; set; }

		public virtual Int32? TicketStatusId { get; set; }

		public bool TicketClose { get; set; }


		public virtual String TicketStatusName { get; set; }

		public virtual String TicketStatusColor { get; set; }

		public virtual Int32 PriorityId { get; set; }

		public virtual String PriorityName { get; set; }

		public virtual String PriorityColor { get; set; }

		public virtual Int32? TicketTypeId { get; set; }

		public virtual String TicketTypeName { get; set; }

		public virtual String PriorityClass { get; set; }

		public bool IsOverDueTickets { get; set; }

		public virtual Int32 TicketReceivedMethodId { get; set; }

		public virtual Int32 AssignedToId { get; set; }

		public virtual String AssignedTo { get; set; }

		public virtual String AssignToGuid { get; set; }

		public virtual Int32 OwnerId { get; set; }

		public virtual String OwnerGuid { get; set; }

		public virtual DateTime? StartDate { get; set; }

		public virtual String StartDateString { get; set; }

		public virtual DateTime? ActualStartDate { get; set; }

		public virtual String ActualStartDateString { get; set; }

		public virtual DateTime? ActualDueDate { get; set; }

		public virtual String ActualDueDateString { get; set; }

		public virtual DateTime? ActualEndDate { get; set; }

		public virtual String ActualEndDateString { get; set; }

		public virtual DateTime? EndDate { get; set; }


		public virtual String EndDateString { get; set; }

		public virtual String Owner { get; set; }


		public virtual String CreatedBy { get; set; }


		public virtual DateTime? CreatedDate { get; set; }

		public virtual String Code { get; set; }

		public virtual Nullable<Double> AllocatedHours { get; set; }


		public virtual Nullable<Double> ActualHours { get; set; }

		public virtual int CountOverBudgetTasks { get; set; }

		public virtual String TicketOverBudgetClass { get; set; }

		public virtual String Hours { get; set; }

		public virtual string ActHours { get; set; }

		public virtual int TaskCount { get; set; }

		public virtual string OverBudgetIcon { get; set; }
		public virtual string OverDueIcon { get; set; }



		public virtual String TicketStatusSortName { get; set; }


		public virtual bool? IsAddToMyWatch { get; set; }

		public virtual int? TotalRows { get; set; }
	}
}

