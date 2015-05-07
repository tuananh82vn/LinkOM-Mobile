using System;

namespace LinkOM
{
	public class ProjectPhaseList
	{
		public virtual Int32 Id  { get; set; }

		public virtual Int32 ProjectId  { get; set; }

		public virtual string Guid { get; set; }

		public virtual string ProjectGuid { get; set; }


		public virtual String Name  { get; set; }

		public virtual Int32 DisplayOrder  { get; set; }


		public virtual String Description  { get; set; }

		public virtual String Colorname { get; set; }

		public virtual DateTime? PhaseStartDate { get; set; }

		public virtual String PhaseStartDateString { get; set; }


		public virtual DateTime? PhaseEndDate { get; set; }


		public virtual String PhaseEndDateString { get; set; }

		public virtual String CreatedBy  { get; set; }

		public virtual DateTime CreatedDate  { get; set; }

		public virtual String UpdatedBy  { get; set; }

		public virtual DateTime UpdatedDate  { get; set; }

//		public ICollection<Project> Project { get; set; }
	}
}

