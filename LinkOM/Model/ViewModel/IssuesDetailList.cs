using System;
using System.Collections.Generic;

namespace LinkOM
{
	public class IssuesDetailList
	{
		public virtual int? Id { get; set; }
		public virtual string IssueCode { get; set; }
		public virtual string IssueGuid { get; set; }
		public virtual string Title { get; set; }
		public virtual DateTime? OpenDate { get; set; }
		public virtual DateTime? CloseDate { get; set; }
		public virtual DateTime? RessolutionTargetDate { get; set; }
		public virtual int? ProjectId { get; set; }
		public virtual int? IssueStatusId { get; set; }
		public virtual int? PriorityId { get; set; }
		public virtual int? AssignedToId { get; set; }
		public virtual int? OwnerId { get; set; }
		public virtual string CreatedBy { get; set; }
		public virtual DateTime? CreatedDate { get; set; }
		public virtual string Label { get; set; }
		public virtual string IssueDescription { get; set; }
		public virtual string IssueAction { get; set; }
		public virtual Double? AllocatedHours { get; set; }
		public virtual Double? ActualHours { get; set; }
		public virtual int? PreviousAssignToId { get; set; }
		public virtual int? ClientId { get; set; }
		public virtual int? DepartmentId { get; set; }
		public virtual string ProjectName { get; set; }
		public virtual string ClientName { get; set; }
		public virtual string DepartmentName { get; set; }
		public virtual string DepartmentColor { get; set; }
		public virtual string StatusName { get; set; }
		public virtual string IssueStatusColor { get; set; }
		public virtual int? IssueMainStatus { get; set; }
		public virtual string PriorityName { get; set; }
		public virtual string PriorityColor { get; set; }
		public virtual string AssignedToName { get; set; }
		public virtual string OwnerName { get; set; }
		public virtual string ProjectManager { get; set; }
		public virtual string DeliveryManager { get; set; }
		public virtual string ProjectCoordinator { get; set; }
		public virtual int? FileId { get; set; }
		public virtual long? FileSize { get; set; }
		public virtual string FileName { get; set; }
		public virtual string FileMimeType { get; set; }
		public virtual Guid? FileUniqueId { get; set; }
		public virtual string FileFolderPath { get; set; }
		public virtual int? CanEdit { get; set; }
		public virtual int? CanDelete { get; set; }

		public virtual string OpenDateString { get; set; }
		public virtual string CloseDateString  { get; set; }

		public virtual string RessolutionTargetDateString  { get; set; }

		public List<ApiFileReference> Attachments { get; set; }
	}
}

