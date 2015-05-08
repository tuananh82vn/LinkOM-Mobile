using System;
using System.Collections.Generic;

namespace LinkOM
{
	public class TaskList
	{
		public virtual Int32 Id { get; set; }

		public virtual string Guid { get; set; }

		public virtual Int32 ProjectId { get; set; }

		public virtual string ProjectGuid { get; set; }

		public virtual String ProjectName { get; set; }

		public virtual String Title { get; set; }

		public virtual String StatusName { get; set; }

		public virtual String StatusSortName { get; set; }

		public virtual String TaskStatusColor { get; set; }

		public virtual Int32? TaskStatusId { get; set; }

		public bool TaskClose  { get; set; }


		public virtual String OverdueDays { get; set; }

		public virtual int  CountOverDueTasks{ get; set; }


		public virtual int CountOverBudgetTasks { get; set; }


		public virtual String OverdueDaysString		{ get; set; }


		public virtual String OverdueFlagClass		{ get; set; }


		public virtual String TaskOverBudgetClass { get; set; }


		public virtual String ParentTaskCode		{ get; set; }


		public virtual String ParentTaskTitle { get; set; }


		public virtual String Hours { get; set; }


		public virtual String TaskStatusOverdue { get; set; }

		public virtual Int32 TaskMainStatus { get; set; }

		public virtual Int32 PriorityId { get; set; }


		public virtual String PriorityName { get; set; }

		public virtual String PriorityColor { get; set; }

		public virtual String PriorityClass { get { return PriorityColor; } }

		public virtual DateTime? StartDate { get; set; }


		public virtual String StartDateString { get; set; }

		public virtual DateTime? ExpectedCompletionDate { get; set; }

		public virtual String ExpectedCompletionDateString { get; set; }

		public virtual DateTime? EndDate { get; set; }

		public virtual String EndDateString { get; set; }

		public virtual DateTime? ActualExpDate { get; set; }

		public virtual String ActualExpDateString { get; set; }

		public virtual DateTime? ActualStartDate { get; set; }

		public virtual String ActualStartDateString { get; set; }

		public virtual DateTime? ActualEndDate { get; set; }

		public virtual String ActualEndDateString { get; set; }

		public virtual Nullable<Int32> AssignedToId { get; set; }

		public virtual String AssignedTo { get; set; }

		public virtual String AssignToGuid { get; set; }

		public virtual String TaskGuid { get; set; }

		public virtual String Owner { get; set; }

		public virtual String OwnerGuid { get; set; }

		public virtual String Code { get; set; }

		public virtual Int32 OwnerId { get; set; }

		public bool IsOverDueTasks { get; set; }

		public virtual Nullable<Double> AllocatedHours { get; set; }

		public virtual Nullable<Double> ActualHours { get; set; }

		public virtual Int32? ParentTaskId { get; set; }

		public virtual string ParentTaskGuid { get; set; }

		public virtual String CRNumber { get; set; }

		public virtual Boolean IsInternal { get; set; }

		public virtual Boolean IsManagerial { get; set; }

		public virtual String Description { get; set; }

		public virtual String Reply { get; set; }

		public virtual String CreatedBy { get; set; }

		public virtual DateTime? CreatedDate { get; set; }

		public virtual String UpdatedBy { get; set; }

		public virtual DateTime UpdatedDate { get; set; }

		public virtual Int32? TaskRecurrenceId { get; set; }

		public virtual string ActHours { get; set; }

		public virtual string SubTaskIcon { get; set; }

		public virtual string OverBudgetIcon { get; set; }

		public virtual string OverDueIcon { get; set; }

		public virtual string ProjectPhaseName { get; set; }

		public virtual string ProjectPhaseColorName { get; set; }
		public virtual Boolean IsActive { get; set; }


		public virtual bool? IsAddToMyWatch { get; set; }

		public virtual int? TotalRows { get; set; }

		public virtual bool IsEdit { get; set; }

	}
}

