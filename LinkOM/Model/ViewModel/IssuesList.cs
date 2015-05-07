using System;

namespace LinkOM
{
	public class IssuesList
	{
		public virtual Int32 Id { get; set; }

		public virtual Int32 ProjectId { get; set; }

		public virtual String Title { get; set; }

		public virtual String ProjectName { get; set; }

		public virtual string ProjectGuid { get; set; }

		public virtual string IssueGuid { get; set; }

		public virtual String StatusName { get; set; }

		public virtual String StatusSortName { get; set; }

		public virtual String OwnerName { get; set; }

		public virtual String OwnerGuid { get; set; }

		public virtual String AssignedToName { get; set; }

		public virtual String AssignedToGuid { get; set; }

		public virtual String PriorityName		{ get; set; }

		public virtual Int32 StatusId { get; set; }

		public virtual Int32? Status { get; set; }

		public bool IssueClose { get; set; }

		public virtual Int32 Priority { get; set; }

		public virtual DateTime? OpenDate { get; set; }

		public virtual string OpenDateString { get; set; }

		public virtual DateTime? CloseDate { get; set; }

		public virtual string CloseDateString { get; set; }

		public virtual DateTime? ResolutionTargetDate { get; set; }

		public virtual string ResolutionTargetDateString { get; set; }

		public virtual DateTime RessolutionTargetDate { get; set; }

		public virtual Int32 AssignToId { get; set; }

		public virtual Int32 OwnerId { get; set; }

		public virtual String Description { get; set; }

		public virtual String Action { get; set; }

		public virtual String CreatedBy { get; set; }

		public virtual DateTime CreatedDate { get; set; }

		public virtual String UpdatedBy { get; set; }

		public virtual DateTime UpdatedDate { get; set; }

		public virtual String PriorityColor { get; set; }

		public virtual String IssuetatusColor { get; set; }

		public virtual string Guid { get; set; }

		public virtual String Code { get; set; }

		public virtual String TaskCode { get; set; }

		public virtual int  TaskId { get; set; }

		public virtual string TaskGuid { get; set; }

		public virtual bool? IsAddToMyWatch { get; set; }

		public virtual int? TotalRows { get; set; }

		public virtual Nullable<Double> AllocatedHours { get; set; }
	}
}

