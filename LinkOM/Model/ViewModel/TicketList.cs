using System;

namespace LinkOM
{
	public class TicketList
	{

		public virtual int? Id { get; set; }
		public virtual string Code { get; set; }
		public virtual string Guid { get; set; }
		public virtual string Title { get; set; }
		public virtual DateTime? StartDate { get; set; }
		public virtual DateTime? EndDate { get; set; }
		public virtual int? ProjectId { get; set; }
		public virtual int? AssignedToId { get; set; }
		public virtual int? PriorityId { get; set; }
		public virtual int? TicketStatusId { get; set; }
		public virtual DateTime? ActualStartDate { get; set; }
		public virtual DateTime? ActualEndDate { get; set; }
		public virtual double? AllocatedHours { get; set; }
		public virtual double? ActualHours { get; set; }
		public virtual string ProjectName { get; set; }
		public virtual string StatusName { get; set; }
		public virtual string TicketStatusColor { get; set; }
		public virtual int? TicketMainStatus { get; set; }
		public virtual string PriorityName { get; set; }
		public virtual string PriorityColor { get; set; }
		public virtual string TicketTypeName { get; set; }
		public virtual string AssignedToName { get; set; }
		public virtual int? CanEdit { get; set; }
		public virtual int? CanDelete { get; set; }

		public virtual int? TotalRows { get; set; }

		public virtual string StartDateString  { get; set; }
		public virtual string EndDateString  { get; set; }

		public virtual string ActualStartDateString  { get; set; }
		public virtual string ActualEndDateString  { get; set; }
	}
}

