using System;
using Android.OS;
using Java.Interop;

namespace LinkOM
{
	[Serializable]
	public class TaskObject 
	{
		public Nullable<int> Id { get; set; }
		public string Guid { get; set; }
		public Nullable<int> ProjectId { get; set; }
		public string ProjectGuid { get; set; }
		public string ProjectName { get; set; }
		public string Title { get; set; }
		public string StatusName { get; set; }
		public string StatusSortName { get; set; }
		public string TaskStatusColor { get; set; }
		public Nullable<int> TaskStatusId { get; set; }
		public Nullable<bool> TaskClose { get; set; }
		public Nullable<int> OverdueDays { get; set; }
		public Nullable<int> CountOverDueTasks { get; set; }
		public Nullable<int> CountOverBudgetTasks { get; set; }
		public string OverdueDaysString { get; set; }
		public string OverdueFlagClass { get; set; }
		public string TaskOverBudgetClass { get; set; }
		public string ParentTaskCode { get; set; }
		public string ParentTaskTitle { get; set; }
		public string Hours { get; set; }
		public string TaskStatusOverdue { get; set; }
		public string TaskMainStatus { get; set; }
		public Nullable<int> PriorityId { get; set; }
		public string PriorityName { get; set; }
		public string PriorityColor { get; set; }
		public string PriorityClass { get; set; }
		public string StartDate { get; set; }
		public string StartDateString { get; set; }
		public string EndDate { get; set; }
		public string EndDateString { get; set; }
		public string ActualStartDate { get; set; }
		public string ActualStartDateString { get; set; }
		public string ActualEndDate { get; set; }
		public string ActualEndDateString { get; set; }
		public Nullable<int> AssignedToId { get; set; }
		public string AssignedTo { get; set; }
		public string AssignToGuid { get; set; }
		public string Owner { get; set; }
		public string OwnerGuid { get; set; }
		public string Code { get; set; } 
		public Nullable<int> OwnerId { get; set; }
		public Nullable<bool> IsOverDueTasks { get; set; }
		public string AllocatedHours { get; set; }
		public string ActualHours { get; set; }
		public Nullable<int> ParentTaskId { get; set; }
		public string ParentTaskGuid { get; set; }
		public string CRNumber { get; set; }
		public Nullable<bool> IsInternal { get; set; }
		public Nullable<bool> IsManagerial { get; set; }
		public string Description { get; set; }
		public string Reply { get; set; }
		public string CreatedBy { get; set; }
		public string CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public string UpdatedDate { get; set; }
		public Nullable<int> TaskRecurrenceId { get; set; }
		public string Priority { get; set; }
		public string TaskStatus { get; set; }
		public string Project { get; set; }
		public string Staff { get; set; }
		public string Staff1 { get; set; }
		public string Task1 { get; set; }
		public string Task2 { get; set; }
		public string TaskComment { get; set; }
		public string FileReference { get; set; }
		public string Ticket { get; set; }
		public string Issue { get; set; }
		public string ActHours { get; set; }
		public string SubTaskIcon { get; set; }
		public string OverBudgetIcon { get; set; }
		public string OverDueIcon { get; set; }
		public string ProjectPhaseName { get; set; }
		public Nullable<bool> IsActive { get; set; }
		public Nullable<bool> IsAddToMyWatch { get; set; }
		public Nullable<int> TotalRows { get; set; }
	}
}

