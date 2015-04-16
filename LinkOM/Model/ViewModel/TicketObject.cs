using System;

namespace LinkOM
{
	public class TicketObject
	{

		public virtual Int32 Id { get; set; }

		public virtual Int32 PreviousAssignToId { get; set; }

		public virtual String MailTo { get; set; }


		public virtual Int32 ProjectId { get; set; }

		public virtual String ProjectName { get; set; }

		public virtual String ProjectGuid { get; set; }


		public virtual String Title { get; set; }


		public virtual Int32 TicketStatusId { get; set; }


		public virtual String TicketStatusName { get; set; }

		public virtual String StatusColorName { get; set; }


		public virtual String PriorityName { get; set; }


		public virtual String PriorityColorName { get; set; }

		public virtual Int32 PriorityId { get; set; }

		public virtual Int32? TicketTypeId { get; set; }

		public virtual String TicketTypeName { get; set; }


		public virtual String Label { get; set; }


		public virtual Int32 TicketReceivedMethodId { get; set; }


		public virtual String TicketReceivedMethodName { get; set; }


		public virtual Int32 AssignedToId { get; set; }


		public virtual String AssignedToName { get; set; }


		public virtual Int32 OwnerId { get; set; }

		public virtual String OwnerName { get; set; }


		public virtual DateTime StartDate { get; set; }

		public string StartDateString { get; set; }

		public virtual DateTime? EndDate { get; set; }

		public string EndDateString		{ get; set; }

		public virtual DateTime? ActualStartDate { get; set; }

		public string ActualStartDateString	{ get; set; }

		public virtual DateTime? ActualEndDate { get; set; }

		public string ActualEndDateString { get; set; }

		public virtual String Description { get; set; }

		public virtual String TicketCommentText { get; set; }

		public virtual string AttachmentFileIdList { get; set; }

		public virtual Boolean IsInternal { get; set; }

		public virtual bool? IsSubTask { get; set; }

		public virtual Boolean IsManagement { get; set; }

		public virtual String CreatedBy { get; set; }

		public virtual DateTime? CreatedDate { get; set; }

		public virtual String Code { get; set; }

		public virtual String UpdatedBy { get; set; }

		public virtual DateTime UpdatedDate { get; set; }

		public virtual Double? AllocatedHours { get; set; }

		public virtual Double? ActualHours { get; set; }


		public virtual String ProjectManager { get; set; }

		public virtual String ProjectManagerGuid { get; set; }


		public virtual String DepartmentName { get; set; }

		public virtual bool IsPriorityChange { get; set; }

		//public virtual Guid Guid { get; set; }
		public virtual string Guid { get; set; }

		public virtual bool? IsAddToMyWatch { get; set; }


		public virtual String AssignGuid { get; set; }
		public virtual String OwnerGuid { get; set; }

		public virtual Int32 TicketMainStatus { get; set; }
		public virtual String OverdueDays { get; set; }
		public virtual String TicketStatusOverdue { get; set; }
		public virtual String OverdueFlagClass  { get; set; }

		public virtual int CountOverDueTickets  { get; set; }

		public string ClientName { get; set; }

		public string ProjectCoordinatorName { get; set; }

		public string ProjectDeliveryManagerName { get; set; }

		public string ProjectNotes { get; set; }

		public string ProjectDescription { get; set; }

		public virtual bool IsEdit { get; set; }
	}
}

