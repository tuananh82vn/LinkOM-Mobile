using System;
using System.Collections.Generic;

namespace LinkOM
{
	public class TicketEdit : ITicket
	{
		public virtual Int32 Id { get; set; }
		public virtual Int32 PreviousAssignToId { get; set; }

		public virtual string Guid { get; set; }

		public virtual string ProjectGuid { get; set; }

		public virtual String MailTo { get; set; }

		public virtual Int32 ProjectId { get; set; }

		public virtual String Title { get; set; }

		public virtual Int32 TicketStatusId { get; set; }

		public virtual Int32 PriorityId { get; set; }

		public virtual Int32 TicketTypeId { get; set; }

		public virtual Int32 TicketReceivedMethodId { get; set; }

		public virtual Int32 AssignedToId { get; set; }

		public virtual Int32 OwnerId { get; set; }

		public virtual String Label { get; set; }

		public virtual DateTime StartDate { get; set; }

		public virtual DateTime EndDate { get; set; }

		public virtual DateTime? ActualStartDate { get; set; }

		public virtual DateTime? ActualEndDate { get; set; }

		public virtual String Description { get; set; }

		public virtual String TicketCommentText { get; set; }

		public virtual string AttachmentFileIdList { get; set; }

		public virtual Boolean IsInternal { get; set; }

		public virtual String IsInternalString { get; set; }


		public virtual Boolean IsManagement { get; set; }


		public virtual String UpdatedBy { get; set; }

		public virtual DateTime UpdatedDate { get; set; }


		public virtual String Code { get; set; }

		public virtual int AllocatedHours { get; set; }

		public virtual Double? ActualHours { get; set; }

		public virtual Boolean IsUserWatch { get; set; }


		public string Message { get; set; }
		public virtual bool IsAssignChange { get; set; }
		public virtual bool IsEndDateChange { get; set; }
		public virtual bool IsStatusChange { get; set; }
		public virtual bool IsPriorityChange { get; set; }
		public virtual bool IsCreatedByClient { get; set; }
		public string removeFileRefIds { get; set; }

		public virtual bool IsDescriptionAttachmentChanged { get; set; }

		public virtual Double? SpentHours { get; set; }

		public virtual bool IsApi { get; set; }
		public virtual int? UserId { get; set; }
		public virtual string UserName { get; set; }
		public IEnumerable<ApiFileReference> Attachments { get; set; }
	}

	public class TicketAdd : ITicket
	{
		public virtual Int32 Id { get; set; }

		public virtual Int32 PreviousAssignToId { get; set; }
		public virtual String MailTo { get; set; }

		public virtual Int32 ProjectId { get; set; }

		public virtual String Title { get; set; }

		public virtual Int32 TicketStatusId { get; set; }

		public virtual Int32 PriorityId { get; set; }

		public virtual Int32 TicketTypeId { get; set; }

		public virtual Int32 TicketReceivedMethodId { get; set; }

		public virtual Int32 AssignedToId { get; set; }

		public virtual Int32 OwnerId { get; set; }

		public virtual String Label { get; set; }

		public virtual string Guid { get; set; }

		public virtual DateTime StartDate { get; set; }

		public virtual DateTime EndDate { get; set; }

		public virtual DateTime? ActualStartDate { get; set; }

		public virtual DateTime? ActualEndDate { get; set; }

		public virtual String Description { get; set; }

		public virtual String Code { get; set; }

		public virtual String AllocatedHours { get; set; }

		public virtual Double? ActualHours { get; set; }

		public virtual Boolean IsInternal { get; set; }

		public virtual Boolean IsManagement { get; set; }

		public virtual String CreatedBy { get; set; }

		public virtual DateTime CreatedDate { get; set; }

		public virtual Boolean IsUserWatch { get; set; }

		public virtual String TicketCommentText { get; set; }
		public virtual bool IsAssignChange { get; set; }
		public virtual bool IsEndDateChange { get; set; }
		public virtual bool IsStatusChange { get; set; }
		public virtual bool IsPriorityChange { get; set; }
		public virtual bool IsCreatedByClient { get; set; }
		public string Message { get; set; }

		public virtual bool IsDescriptionAttachmentChanged { get; set; }

		public virtual Double? SpentHours { get; set; }

		public virtual bool IsApi { get; set; }
		public virtual int? UserId { get; set; }
		public virtual string UserName { get; set; }

		public IEnumerable<ApiFileReference> Attachments { get; set; }
	}

	public interface ITicket
	{
		Int32 Id { get; set; }
		Int32 ProjectId { get; set; }
		String MailTo { get; set; }
		//Guid Guid { get; set; }
		string Guid { get; set; }
		Int32 TicketStatusId { get; set; }
		Int32 PriorityId { get; set; }
		Int32 AssignedToId { get; set; }
		Int32 OwnerId { get; set; }
		DateTime EndDate { get; set; }
		String TicketCommentText { get; set; }
		String Title { get; set; }
		String Code { get; set; }
		String Description { get; set; }
		bool IsAssignChange { get; set; }
		bool IsEndDateChange { get; set; }
		bool IsStatusChange { get; set; }
		bool IsPriorityChange { get; set; }
		bool IsManagement { get; set; }
		bool IsInternal { get; set; }
		string Message { get; set; }
		bool IsDescriptionAttachmentChanged { get; set; }
		Double? SpentHours { get; set; }

		bool IsApi { get; set; }
		int? UserId { get; set; }
		string UserName { get; set; }
	}
}

