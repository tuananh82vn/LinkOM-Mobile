using System;

namespace LinkOM
{
	public class MilestonesList
	{
		public virtual Int32 Id  { get; set; }

		public virtual String ProjectName  { get; set; }
		public virtual String ProjectGuid { get; set; }


		public virtual String Title  { get; set; }

		public virtual String  Status  { get; set; }


		public virtual Int32? StatusId { get; set; }

		public virtual String OwnerName { get; set; }


		public virtual String OwnerGuid { get; set; }

		public virtual String AssignByName { get; set; }


		public virtual String AssignByGuid { get; set; }


		public virtual String AssignedTo { get; set; }

		public virtual Int32? AssignedToId { get; set; }

		public virtual Int32? OwnerId { get; set; }  


		public virtual String Priority { get; set; }

		public virtual String PriorityName	{ get; set; }


		public virtual String ProjectPhaseName { get; set; }

		public virtual DateTime? StartDate { get; set; }
		public virtual DateTime? EndDate  { get; set; }

		public virtual DateTime? ActualStartDate { get; set; }
		public virtual DateTime? ActualEndDate { get; set; }

		public virtual DateTime? ExpectedCompletionDate { get; set; }


		public virtual String EndDateString { get; set; }


		public virtual String OverdueDays { get; set; }


		public virtual String ActualStartDateString { get; set; }


		public virtual String ActualEndDateString { get; set; }


		public virtual String StartDateString { get; set; }

	
		public virtual String ExpectedCompletionDateString { get; set; }


		public virtual String PriorityClass { get { return PriorityColor; } }
		public virtual String ProjectPhaseClass { get { return ProjectPhaseColor; } }

		public virtual String ProjectPhaseColor { get; set; }


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

		public virtual int CountOverDueMilestone	{ get; set; }
		public virtual string OverDueIcon { get; set; }

		//add TotalRows  by rutul soni on 30 dec 2014
		public virtual Int32? TotalRows { get; set; }

		/// <summary>
		/// Added on changes related to milestone status table
		/// </summary>
		public virtual string MileStoneStatusColour { get; set; }
		public virtual int? MileStoneMainStatus { get; set; }

		//add by rutul soni on  3  mar 2015
		public virtual String OverdueFlagClass { get; set; }
	}
}

