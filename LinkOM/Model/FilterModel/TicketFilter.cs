using System;

namespace LinkOM
{
	public class TicketFilter
	{
		public virtual string Code { get; set; }
		public virtual string Title { get; set; }
		public virtual int?   ProjectId  { get; set; }
		public virtual int?   DepartmentId 	{ get; set; }
		public virtual int? TicketStatusId { get; set; }
		public virtual DateTime? EndDateTo { get; set; }

		public virtual int?   MainStatusId  { get; set; }
		public virtual int?   AssignedToId  { get; set; }
		public virtual bool?  OverdueTicketFlag { get; set; }
	}
}

