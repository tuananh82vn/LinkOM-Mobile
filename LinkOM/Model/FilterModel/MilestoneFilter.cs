using System;

namespace LinkOM
{
	public class MilestoneFilter
	{
		public virtual string Title { get; set; }
		public virtual int?   ProjectId  { get; set; }
		public virtual int?   DepartmentId 	 { get; set; }
		public virtual int?  StatusId 	 { get; set; }
		public virtual int?  MainStatusId  { get; set; }
		public virtual bool?  OverdueMilestoneFlag { get; set; }
		public virtual int? PriorityId	 { get; set; }
	}
}

