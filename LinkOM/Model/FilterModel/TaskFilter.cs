using System;

namespace LinkOM
{
	public class TaskFilter
	{

		public virtual string Code { get; set; }
		public virtual string Title { get; set; }
		public virtual int? ProjectId { get; set; }
		public virtual int? DepartmentId { get; set; }
		public virtual string TaskStatusId { get; set; }
		public virtual DateTime? EndDateTo { get; set; }
		public virtual int? MainStatusId { get; set; }
		public virtual bool? OverdueTasksFlag { get; set; }
		public virtual int? AssignedToId { get; set; }
	}
}

