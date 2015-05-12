using System;
using Android.OS;
using Java.Interop;

namespace LinkOM
{
	[Serializable]
	public class TaskDetailList 
	{
		public virtual Int32 Id { get; set; }
		public virtual Int32? PreviousAssignToId { get; set; }

		public virtual string Guid { get; set; }

		public virtual String ProjectGuid { get; set; }


		public virtual String MailTo { get; set; }

		public virtual Int32 ProjectId { get; set; }

		public virtual String ProjectName { get; set; }

		public virtual String ProjectManager { get; set; }

		public virtual String ProjectManagerGuid { get; set; }


		public virtual String DepartmentName { get; set; }

		public virtual String DepartmentColor { get; set; }


		public virtual String Title { get; set; }

		public virtual Int32 Status { get; set; }

		public virtual String StatusName { get; set; }

		public virtual String StatusColorName { get; set; }

		public virtual Int32 PriorityId { get; set; }

		public virtual String PriorityName { get; set; }

		public virtual String PriorityColorName { get; set; }

		public virtual DateTime StartDate { get; set; }

		public string StartDateString { get; set; }

		public virtual DateTime? EndDate { get; set; }

		public string EndDateString		{ get; set; }

		public virtual DateTime? ActualStartDate { get; set; }

		public string ActualStartDateString	{ get; set; }

		public virtual DateTime? ActualEndDate { get; set; }

		public string ActualEndDateString { get; set; }

		public virtual Int32 AssignedToId { get; set; }

		public virtual String AssignedToName { get; set; }

		public virtual String AssingToDesignation { get; set; }

		public virtual int? AssingToFileId { get; set; }

		public virtual String OwnerDesignation { get; set; }

		public virtual int? OwnerFileId { get; set; }

		public virtual Int32 OwnerId { get; set; }

		public virtual String OwnerName { get; set; }

		public virtual Nullable<Int32> ProjectPhaseId { get; set; }

		public virtual String ProjectPhaseName { get; set; }

		public virtual String ProjectPhaseColor { get; set; }

		public virtual String Label { get; set; }

		public virtual DateTime? ExpectedCompletionDate { get; set; }

		public string ExpectedCompletionDateString { get; set; }


		public virtual Double? AllocatedHours { get; set; }

		public virtual Double? ActualHours { get; set; }

		public virtual Int32 ParentTaskId { get; set; }

		public virtual String ParentTask { get; set; }

		public virtual String CRNumber { get; set; }

		public virtual double? Percentcomplete { get; set; }

		public virtual String TaskDescription { get; set; }

//		[DisplayName("Attachment")]
//		[File(AllowedFileExtensions = new string[] { ".jpeg", ".jpg", ".png", ".pdf", ".doc", ".docx", ".xls", ".bmp", ".xlsx" }, UploadFileType = AppEnums.UploadFileType.IssueAttachment, ErrorMessage = "Invalid File")]
//		public HttpPostedFileBase AttachmentFile1 { get; set; }
//
//		[File(AllowedFileExtensions = new string[] { ".jpeg", ".jpg", ".png", ".pdf", ".doc", ".docx", ".xls", ".bmp", ".xlsx" }, UploadFileType = AppEnums.UploadFileType.IssueAttachment, ErrorMessage = "Invalid File")]
//		public HttpPostedFileBase AttachmentFile2 { get; set; }
//
//		[File(AllowedFileExtensions = new string[] { ".jpeg", ".jpg", ".png", ".pdf", ".doc", ".docx", ".xls", ".bmp", ".xlsx" }, UploadFileType = AppEnums.UploadFileType.IssueAttachment, ErrorMessage = "Invalid File")]
//		public HttpPostedFileBase AttachmentFile3 { get; set; }

		public virtual String TaskCommentText { get; set; }

		public virtual Boolean IsInternal { get; set; }

		public virtual Boolean IsManagerial { get; set; }

		public virtual string AttachmentFileIdList { get; set; }

		public virtual int?  TaskRecurrenceId { get; set; }

		public virtual bool? TaskRecurrenceIsActive { get; set; }

		public virtual bool? IsAddToMyWatch { get; set; }

		public virtual String AssignGuid { get; set; }

		public virtual String OwnerGuid { get; set; }

		public virtual bool? IsSubTask { get; set; }

		public virtual String Code { get; set; }

		public virtual String UpdatedBy { get; set; }

		public virtual DateTime UpdatedDate { get; set; }


//		public ICollection<Priority> Priority { get; set; }
//
//		public ICollection<Project> Project { get; set; }
//
//		public ICollection<Staff> Staff { get; set; }
//
//		public ICollection<Staff> Staff1 { get; set; }
//
//		public ICollection<Task> Task1 { get; set; }
//
//		public ICollection<Task> Task2 { get; set; }
//
//		public ICollection<TaskComment> TaskComment { get; set; }
//
//		public ICollection<FileReference> FileReference { get; set; }

		public virtual String TaskStatusOverdue { get; set; }

		public virtual Int32 TaskMainStatus { get; set; }

		public virtual Int32 TaskStatusId { get; set; }

		public virtual String OverdueDays { get; set; }

		public virtual String OverdueFlagClass { get; set; }


		public virtual String TaskOverBudgetClass { get; set; }


		public string ClientName { get; set; }

		public string ProjectCoordinatorName { get; set; }

		public string ProjectDeliveryManagerName { get; set; }

		public string ProjectNotes { get; set; }

		public string ProjectDescription { get; set; }

		public virtual bool  IsEdit { get; set; }

//		public IEnumerable<ApiFileReference> Attachments { get; set; }
	}
}

