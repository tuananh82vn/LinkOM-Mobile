using System;
using System.ComponentModel;

namespace LinkOM
{
	public class Milestone
	{
		public virtual Int32 Id  { get; set; }

		public virtual String ProjectName  { get; set; }
		public virtual String ProjectGuid { get; set; }



		public virtual String Title  { get; set; }

		[DisplayName("Status")]
		public virtual String  Status  { get; set; }


		public virtual Int32? StatusId { get; set; }


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
		public virtual String PriorityName { get; set; }

		[DisplayName("Project Phase")]
		public virtual String ProjectPhaseName { get; set; }

		public virtual DateTime? StartDate { get; set; }
		public virtual DateTime? EndDate  { get; set; }

		public virtual DateTime? ActualStartDate { get; set; }
		public virtual DateTime? ActualEndDate { get; set; }

		public virtual DateTime? ExpectedCompletionDate { get; set; }

		[DisplayName("Due Date")]
		public virtual String EndDateString { get; set; }

		[DisplayName("Ov. Days")]
		public virtual String OverdueDays{ get; set; }

		[DisplayName("Due Date")]
		public virtual String ActualStartDateString { get; set; }

		[DisplayName("Actual End Date")]
		public virtual String ActualEndDateString { get; set; }

		[DisplayName("Start Date")]
		public virtual String StartDateString { get; set; }

		[DisplayName("Expected Completion Date")]
		public virtual String ExpectedCompletionDateString { get; set; }


		public virtual String PriorityClass { get { return PriorityColor; } }

		public virtual String ProjectPhaseClass { get { return ProjectPhaseColor; } }


		[DisplayName("Project Phase Color")]
		public virtual String ProjectPhaseColor { get; set; }

		[DisplayName("Priority")]
		public virtual String PriorityColor { get; set; }

		public virtual Int32 ProjectId { get; set; }

		public virtual string Guid { get; set; }

		public virtual int CountOverDueMilestone { get; set; }

		public virtual string OverDueIcon { get; set; }

		public virtual Int32? TotalRows { get; set; }

		public virtual string MileStoneStatusColour { get; set; }
		public virtual int? MileStoneMainStatus { get; set; }

		public virtual String OverdueFlagClass { get; set; }
	}
}

