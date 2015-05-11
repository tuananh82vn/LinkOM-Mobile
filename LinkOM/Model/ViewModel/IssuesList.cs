using System;

namespace LinkOM
{
	public class IssuesList
	{
		public virtual int? TotalRows { get; set; }
		public virtual int? Id { get; set; }
		public virtual string IssueCode { get; set; }
		public virtual string IssueGuid { get; set; }
		public virtual string Title { get; set; }
		public virtual DateTime? OpenDate { get; set; }
		public virtual DateTime? CloseDate { get; set; }
		public virtual DateTime? RessolutionTargetDate { get; set; }
		public virtual int? ProjectId { get; set; }
		public virtual int? AssignedToId { get; set; }
		public virtual int? OwnerId { get; set; }
		public virtual int? PriorityId { get; set; }
		public virtual string CreatedBy { get; set; }
		public virtual DateTime? CreatedDate { get; set; }
		public virtual string Label { get; set; }
		public virtual string ProjectName { get; set; }
		public virtual string PriorityName { get; set; }
		public virtual string PriorityColor { get; set; }
		public virtual int? IssueStatusId { get; set; }
		public virtual string StatusName { get; set; }
		public virtual string IssueStatusColor { get; set; }
		public virtual int? IssueMainStatus { get; set; }
		public virtual string AssignedToName { get; set; }
		public virtual string OwnerName { get; set; }
		public virtual int? CanEdit { get; set; }
		public virtual int? CanDelete { get; set; }



		public virtual string OpenDateString { get; set; }
		public virtual string CloseDateString { get; set; }

		public virtual string RessolutionTargetDateString { get; set; }
	}
}

