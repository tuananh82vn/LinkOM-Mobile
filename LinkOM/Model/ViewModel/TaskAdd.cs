using System;
using System.Collections.Generic;

namespace LinkOM
{
	public class TaskAdd : ITask
	{
		public virtual Int32 Id { get; set; }

		public virtual Int32 TaskRecurrenceId { get; set; }

		public virtual String MailTo { get; set; }

		public virtual Int32 ProjectId { get; set; }

		public virtual String Title { get; set; }

		public virtual Int32 TaskStatusId { get; set; }

		public virtual Int32 PriorityId { get; set; }

		public virtual String Label { get; set; }

		public virtual DateTime StartDate { get; set; }

		public virtual DateTime EndDate { get; set; }

		public virtual DateTime? ActualStartDate { get; set; }

		public virtual DateTime? ActualEndDate { get; set; }

		public virtual Int32 AssignedToId { get; set; }

		public virtual Int32 PreviousAssignToId { get; set; }

		public virtual Int32 OwnerId { get; set; }

		public virtual Nullable<Int32> ProjectPhaseId { get; set; }

		public virtual string Guid { get; set; }

		public virtual DateTime? ExpectedCompletionDate { get; set; }

		public virtual Double? AllocatedHours { get; set; }

		public virtual Double? ActualHours { get; set; }

		public virtual Double? SpentHours { get; set; }

		public virtual Double? Percentcomplete { get; set; }

		public virtual Int32? ParentTaskId { get; set; }

		public virtual String ParentTask { get; set; }

		public virtual String CRNumber { get; set; }

		public virtual String Description { get; set; }

		public virtual Boolean IsInternal { get; set; }

		public virtual Boolean IsManagerial { get; set; }

		public virtual String Code { get; set; }

		public virtual String CreatedBy { get; set; }

		public virtual DateTime CreatedDate { get; set; }

		//public HttpPostedFileBase AttachmentFile1 { get; set; }

//		[File(AllowedFileExtensions = new string[] { ".jpeg", ".jpg", ".png", ".pdf", ".doc", ".docx", ".xls", ".bmp", ".xlsx", ".ppt", ".pptx", ".zip", ".txt" }, UploadFileType = AppEnums.UploadFileType.IssueAttachment, ErrorMessage = "Invalid File")]
//		public HttpPostedFileBase AttachmentFile2 { get; set; }
//
//		[File(AllowedFileExtensions = new string[] { ".jpeg", ".jpg", ".png", ".pdf", ".doc", ".docx", ".xls", ".bmp", ".xlsx", ".ppt", ".pptx", ".zip", ".txt" }, UploadFileType = AppEnums.UploadFileType.IssueAttachment, ErrorMessage = "Invalid File")]
//		public HttpPostedFileBase AttachmentFile3 { get; set; }

		public virtual Boolean IsUserWatch { get; set; }

		public virtual Boolean IsRecurrTask { get; set; }

		public virtual Boolean IsActive { get; set; }

		public virtual string  TaskOccuranceOperation { get; set; }

		public virtual String TaskCommentText { get; set; }

		public virtual bool IsAssignChange { get; set; }

		public virtual bool IsEndDateChange { get; set; }

		public virtual bool IsStatusChange { get; set; }

		public virtual bool IsPriorityChange { get; set; }

		public virtual string JsonObjectString { get; set; }

		public virtual DateTime?  TaskRecStartDate { get; set; }

		public virtual DateTime?  TaskRecEndDate { get; set; }

		public virtual Int32? DailyDays { get; set; }

		public virtual Int32? MonthlyOnDay { get; set; }

		public virtual Int32? MonthlyEveryMonths { get; set; }


		public virtual Int32? WeeklyEveryWeeks { get; set; }

		public virtual Int32? YealryAfterYears { get; set; }

		public virtual Int32? YearlyonEveryDays { get; set; }

		public virtual Int32? YearlyonEveryMonths { get; set; }

		public virtual string WeekSelection { get; set; }

		public virtual int? RecurrenceFrequency { get; set; }

		public string Message { get; set; }

		public virtual bool IsDescriptionAttachmentChanged { get; set; }

		public virtual bool IsApi { get; set; }

		public virtual int? UserId { get; set; }

		public virtual string UserName { get; set; }

		public IEnumerable<ApiFileReference> Attachments { get; set; }
	}

	public interface ITask
	{
		Int32 Id { get; set; }
		Int32 ProjectId { get; set; }
		String MailTo { get; set; }
		Int32 TaskStatusId { get; set; }
		Int32 PriorityId { get; set; }
		Int32 AssignedToId { get; set; }
		Int32 OwnerId { get; set; }
		DateTime EndDate { get; set; }
		String TaskCommentText { get; set; }
		String Title { get; set; }
		String Code { get; set; }
		String Description { get; set; }
		bool IsAssignChange { get; set; }
		bool IsEndDateChange { get; set; }
		bool IsStatusChange { get; set; }
		bool IsPriorityChange { get; set; }
		Double? SpentHours { get; set; }
		string Guid { get; set; }
		string Message { get; set; }
		bool IsInternal { get; set; }
		bool IsManagerial { get; set; }
		bool IsDescriptionAttachmentChanged { get; set; }
		bool IsApi { get; set; }
		int? UserId { get; set; }
		string UserName { get; set; }

	}
}

