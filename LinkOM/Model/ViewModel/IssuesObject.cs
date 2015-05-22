using System;
using System.Collections.Generic;

namespace LinkOM
{
	public class IssuesEdit : IIssue
	{
		public virtual Int32 Id { get; set; }

		public virtual string ProjectGuid { get; set; }

		public virtual Int32 PreviousAssignToId { get; set; }

		public virtual Int32 ProjectId { get; set; }

		public virtual String Title { get; set; }

		public virtual Int32 StatusId { get; set; }

		public virtual Int32 PriorityId { get; set; }

		public virtual String Label { get; set; }

		public virtual DateTime OpenDate { get; set; }

		public virtual DateTime? CloseDate { get; set; }

		public virtual DateTime? RessolutionTargetDate { get; set; }

		public virtual Int32 AssignedToId { get; set; }

		public virtual Int32 OwnerId { get; set; }

		public virtual String Description { get; set; }

		public virtual String ActionText { get; set; }

		public virtual String Code { get; set; }


		public virtual String IssueCommentText { get; set; }

		public virtual string Guid { get; set; }

		public virtual string AttachmentFileIdList { get; set; }

		public virtual String UpdatedBy { get; set; }

		public virtual DateTime UpdatedDate { get; set; }

		public virtual Boolean IsUserWatch { get; set; }

		public virtual String MailTo { get; set; }

		public virtual bool IsAssignChange { get; set; }

		public virtual bool IsEndDateChange { get; set; }

		public virtual bool IsStatusChange { get; set; }

		public virtual bool IsPriorityChange { get; set; }


		public string Message { get; set; }
		public virtual string removeFileRefIds { get; set; }

		public virtual bool IsDescriptionAttachmentChanged { get; set; }

		public virtual Double? AllocatedHours { get; set; }

		public virtual Double? ActualHours { get; set; }

		public virtual Double? SpentHours { get; set; }


		public virtual int? UserId { get; set; }

		public virtual string UserName { get; set; }

		public IEnumerable<ApiFileReference> Attachments { get; set; }
	}

	public class IssuesAdd : IIssue
	{
		public virtual Int32 PreviousAssignToId { get; set; }

		public virtual Int32 Id { get; set; }

		public virtual Int32 ProjectId { get; set; }

		public virtual String Title { get; set; }

		public virtual Int32 StatusId { get; set; }

		public virtual Int32 PriorityId { get; set; }

		public virtual String Label { get; set; }

		public virtual DateTime OpenDate { get; set; }

		public virtual DateTime? CloseDate { get; set; }

		public virtual DateTime? RessolutionTargetDate { get; set; }

		public virtual Int32 AssignedToId { get; set; }

		public virtual Int32 OwnerId { get; set; }

		public virtual String Description { get; set; }

		public virtual String ActionText { get; set; }

		public virtual String Code { get; set; }

		public virtual String CreatedBy { get; set; }

		public virtual DateTime CreatedDate { get; set; }

		public virtual string Guid { get; set; }

		public string urlName { get; set; }

		public virtual Boolean IsUserWatch { get; set; }

		public virtual String MailTo { get; set; }

		public virtual String IssueCommentText { get; set; }

		public virtual bool IsAssignChange { get; set; }
		public virtual bool IsEndDateChange { get; set; }
		public virtual bool IsStatusChange { get; set; }
		public virtual bool IsPriorityChange { get; set; }

		public string Message { get; set; }


		public virtual bool IsDescriptionAttachmentChanged { get; set; }

		public virtual Double? AllocatedHours { get; set; }

		public virtual Double? ActualHours { get; set; }

		public virtual Double? SpentHours { get; set; }
	
		public virtual int? UserId { get; set; }

		public virtual string UserName { get; set; }

		public IEnumerable<ApiFileReference> Attachments { get; set; }
	}

	public interface IIssue
	{
		Int32 Id { get; set; }
		Int32 ProjectId { get; set; }
		String MailTo { get; set; }
		Int32 StatusId { get; set; }
		Int32 PriorityId { get; set; }
		Int32 AssignedToId { get; set; }
		Int32 OwnerId { get; set; }
		DateTime? CloseDate { get; set; }
		String Title { get; set; }
		String IssueCommentText { get; set; }
		String Code { get; set; }
		String Description { get; set; }
		//Guid Guid { get; set; }
		string Guid { get; set; }
		bool IsAssignChange { get; set; }
		bool IsEndDateChange { get; set; }
		bool IsStatusChange { get; set; }
		bool IsPriorityChange { get; set; }
		Double? SpentHours { get; set; }
		string Message { get; set; }
		bool IsDescriptionAttachmentChanged { get; set; }

		int? UserId { get; set; }
		string UserName { get; set; }
	}
}

