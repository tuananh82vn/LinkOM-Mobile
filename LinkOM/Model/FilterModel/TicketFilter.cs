using System;

namespace LinkOM
{
	public class TicketFilter
	{
		public virtual string Title { get; set; }
		public virtual int? AssignedToId { get; set; }
		public virtual int? ClientId { get; set; }
		public virtual int? TicketStatusId { get; set; }
		public virtual int? PriorityId { get; set; }
		public virtual DateTime? DueBeforeDate { get; set; }
		public virtual int? DepartmentId { get; set; }
		public virtual int? ProjectId { get; set; }
		public virtual bool? AssignByMe { get; set; }
		public virtual string Filter { get; set; }
		public virtual string Label { get; set; }
	}
}

