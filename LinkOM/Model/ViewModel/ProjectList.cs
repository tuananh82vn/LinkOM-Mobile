using System;

namespace LinkOM
{
	public class ProjectList
	{
		public virtual int? Id { get; set; }
		public virtual string ProjectCode { get; set; }
		public virtual string ProjectGuid { get; set; }
		public virtual string Name { get; set; }
		public virtual string ReferenceCode { get; set; }
		public virtual DateTime? StartDate { get; set; }
		public virtual DateTime? EndDate { get; set; }

		public virtual double? AllocatedHours { get; set; }
		public virtual int? ProjectStatusId {get;  set;}
		public virtual string ProjectStatus { get; set; }
		public virtual string ProjectStatusColor { get; set; }      
		public virtual int? ProjectMainStatus {get; set; }
		public virtual string DepartmentName {get; set; }
		public virtual string DepartmentColor {get; set; }
		public virtual int? ClientId {get; set; }
		public virtual string ClientName {get; set; }
		public virtual decimal? ActualHrs {get; set; }
		public virtual int? OpenTicketCount {get; set; }
		public virtual int? OpenTaskCount {get; set; }
		public virtual int? OpenIssueCount {get; set; }

		public virtual string StartDateString {get; set; }
		public virtual string EndDateString {get; set; }

		public virtual int? TotalRows { get; set; }
	}
}

