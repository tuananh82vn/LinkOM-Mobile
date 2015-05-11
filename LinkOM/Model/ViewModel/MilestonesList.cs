using System;

namespace LinkOM
{
	public class MilestonesList
	{
		public virtual int? TotalRows { get; set; }
		public virtual int? Id { get; set; }
		public virtual string Guid { get; set; }
		public virtual string Title { get; set; }
		public virtual int? ProjectId { get; set; }
		public virtual DateTime? StartDate { get; set; }
		public virtual DateTime? EndDate { get; set; }
		public virtual DateTime? ExpectedCompletionDate { get; set; }
		public virtual DateTime? ActualStartDate { get; set; }
		public virtual DateTime? ActualEndDate { get; set; }
		public virtual int? AssignedToId { get; set; }
		public virtual int? OwnerId { get; set; }
		public virtual int? PriorityId { get; set; }
		public virtual int? MileStoneStatusId { get; set; }
		public virtual string StatusName { get; set; }
		public virtual string MileStoneStatusColor { get; set; }
		public virtual int? MileStoneMainStatus { get; set; }
		public virtual string ProjectName { get; set; }
		public virtual string PriorityName { get; set; }
		public virtual string PriorityColor { get; set; }
		public virtual string AssignedToName { get; set; }
		public virtual string OwnerName { get; set; }
		public virtual int? CanEdit { get; set; }
		public virtual int? CanDelete { get; set; }


		public virtual string StartDateString { get; set; }
		public virtual string EndDateString { get; set; }
		public virtual string ExpectedCompletionDateString { get; set; }

		public virtual string ActualStartDateString { get; set; }
		public virtual string ActualEndDateString { get; set; }
	}
}

