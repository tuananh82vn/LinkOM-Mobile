using System;

namespace LinkOM
{
	public class MilestoneFilter
	{
		public virtual string Title { get; set; }
		public virtual int? StatusId { get; set; }
		public virtual int? DeparmentId { get; set; }
		public virtual int? ProjectId { get; set; }
		public virtual int? PriorityId { get; set; }
		public virtual string Label { get; set; }
	}
}

