using System;

namespace LinkOM
{
	public class TaskFilter
	{
		public virtual int? Id { get; set; }
		public virtual string Code { get; set; }
		public virtual int? ProjectId { get; set; }
		public virtual string Title { get; set; }
		public virtual int? ClientId { get; set; }
		public virtual int? MainStatusId { get; set; }
		public virtual int? PriorityId { get; set; }
		public virtual int? AssignedToId { get; set; }
		public virtual int? OwnerId { get; set; }
		public virtual int? DepartmentId { get; set; }
		public virtual string TaskStatusId { get; set; }
		public virtual string Label { get; set; }
		public virtual string ProjectPhase { get; set; }
		public virtual DateTime? StartDateFrom { get; set; }
		public virtual DateTime? StartDateTo { get; set; }
		public virtual DateTime? EndDateFrom { get; set; }
		public virtual DateTime? EndDateTo { get; set; }
		public virtual bool? OverdueTasksFlag { get; set; }
	}
}

