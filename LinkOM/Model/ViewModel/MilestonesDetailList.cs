using System;
using System.ComponentModel;

namespace LinkOM
{
	public class MilestonesDetailList
	{
		public virtual Int32 Id { get; set; }

		[DisplayName("Project")]
		public virtual String ProjectName { get; set; }
		public virtual String ProjectGuid { get; set; }


		[DisplayName("Title")]
		public virtual String Title { get; set; }

		[DisplayName("Status")]
		public virtual String Status { get; set; }


		public virtual Int32? MileStoneStatusId { get; set; }
		public virtual Int32? PriorityId { get; set; }

		public virtual String Code { get; set; }


		[DisplayName("Owner")]
		public virtual String OwnerName { get; set; }

		[DisplayName("Owner Guid")]
		public virtual String OwnerGuid { get; set; }

		[DisplayName("Assigned To")]
		public virtual String AssignByName { get; set; }

		[DisplayName("Assign By Guid")]
		public virtual String AssignByGuid { get; set; }

		[DisplayName("Assigned To")]
		public virtual String AssignedTo { get; set; }

		public virtual Int32? AssignedToId { get; set; }

		public virtual Int32? OwnerId { get; set; }


		[DisplayName("Priority")]
		public virtual String Priority { get; set; }

		[DisplayName("Priority")]
		public virtual String PriorityName
		{
			get;
			set;

		}

		[DisplayName("Project Phase")]
		public virtual String ProjectPhaseName { get; set; }


		public virtual DateTime? EndDate { get; set; }


		public virtual DateTime? DueDate { get; set; }

		[DisplayName("Due Date")]
		public virtual String EndDateString { get; set; }

		public virtual DateTime? ActualEndDate { get; set; }


		public virtual DateTime? ExpectedCompletionDate { get; set; }


		[DisplayName("Due Date")]
		public virtual String DueDateString { get; set; }

		[DisplayName("Ov. Days")]
		public virtual String OverdueDays  { get; set; }



		[DisplayName("Actual End Date")]
		public virtual String ActualEndDateString  { get; set; }


		[DisplayName("Expected Completion Date")]
		public virtual String ExpectedCompletionDateString { get; set; }


		public virtual String PriorityClass { get { return PriorityColor; } }

		public virtual String ProjectPhaseClass { get { return ProjectPhaseColor; } }


		[DisplayName("Project Phase Color")]
		public virtual String ProjectPhaseColor { get; set; }

		[DisplayName("Priority")]
		public virtual String PriorityColor { get; set; }

		public virtual Int32 ProjectId { get; set; }

		//public virtual Guid? Guid { get; set; }
		public virtual string Guid { get; set; }

//		public ICollection<Staff> Staff { get; set; }
//
//		public ICollection<Staff> Staff1 { get; set; }
//
//		public ICollection<MilestoneComment> MilestoneComment { get; set; }
//
//		public ICollection<FileReference> FileReference { get; set; }
//
//		public ICollection<Project> Project { get; set; }

		public virtual int CountOverDueMilestone		{ get; set; }
		public virtual string OverDueIcon { get; set; }

		//add TotalRows  by rutul soni on 30 dec 2014
		public virtual Int32? TotalRows { get; set; }

		/// <summary>
		/// Added on changes related to milestone status table
		/// </summary>
		public virtual string MileStoneStatusColour { get; set; }
		public virtual int? MileStoneMainStatus { get; set; }

		//add by rutul soni on  3  mar 2015
		public virtual String OverdueFlagClass	{ get; set; }

//		[DisplayName("Attachment")]
//		public HttpPostedFileBase AttachmentFile1 { get; set; }

//		[File(AllowedFileExtensions = new string[] { ".jpeg", ".jpg", ".png", ".pdf", ".doc", ".docx", ".xls", ".bmp", ".xlsx" }, UploadFileType = AppEnums.UploadFileType.IssueAttachment, ErrorMessage = "Invalid File")]
//		public HttpPostedFileBase AttachmentFile2 { get; set; }
//
//		[File(AllowedFileExtensions = new string[] { ".jpeg", ".jpg", ".png", ".pdf", ".doc", ".docx", ".xls", ".bmp", ".xlsx" }, UploadFileType = AppEnums.UploadFileType.IssueAttachment, ErrorMessage = "Invalid File")]
//		public HttpPostedFileBase AttachmentFile3 { get; set; }

		public virtual String ProjectManager { get; set; }
		public virtual String ProjectManagerGuid { get; set; }

		public virtual String StatusName { get; set; }
		public virtual String PriorityColorName { get; set; }

		public virtual String StatusColorName { get; set; }
		public virtual String DepartmentName { get; set; }
		public virtual String AssignedToName { get; set; }


		public virtual String AssignGuid { get; set; }
		public virtual String Description { get; set; }
		public virtual String AssingToDesignation { get; set; }
		public virtual int? AssingToFileId { get; set; }


		public virtual String OwnerDesignation { get; set; }
		public virtual int? OwnerFileId { get; set; }

		public virtual Boolean IsInternal { get; set; }
		public virtual Boolean IsManagerial { get; set; }

		public string ClientName { get; set; }

		public string ProjectCoordinatorName { get; set; }

		public string ProjectDeliveryManagerName { get; set; }

		public string ProjectNotes { get; set; }

		public string ProjectDescription { get; set; }
		public string DepartmentColor { get; set; }

//		public IEnumerable<ApiFileReference> Attachments { get; set; }
	}
}

