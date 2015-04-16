using System;
using System.ComponentModel;
using Android.Util;

namespace LinkOM
{
	[Serializable]
	public class ProjectObject
	{
		[DisplayName("Id")]
		public virtual Int32 Id  { get; set; }

		public virtual string Guid { get; set; }

		public virtual String Name  { get; set; }

		public virtual String ClientName { get; set; }

		public virtual string ClientGuid { get; set; }

		[DisplayName("Department")]
		public virtual String DepartmentName { get; set; }

		[DisplayName("Department")]
		public virtual Int32 DepartmentId  { get; set; }

		[DisplayName("Delivery Manager")]
		public virtual Int32 DeliveryManagerId  { get; set; }

		[DisplayName("Delivery Manager")]
		public virtual String DeliveryManagerName { get; set; }


		[DisplayName("Project Coordinator")]
		public virtual String ProjectCoordinatorName { get; set; }


		[DisplayName("Project Manager")]
		public virtual Int32 ProjectManagerId  { get; set; }

		[DisplayName("Project Manager")]
		public virtual String ProjectManagerName { get; set; }


		public virtual String OverdueDays { get; set; }

		[DisplayName("Project Coordinator")]
		public virtual Int32 ProjectCoordinatorId  { get; set; }

		[DisplayName("Project Status")]
		public virtual Int32? ProjectStatusId { get; set; }


		[DisplayName("Status")]
		public virtual String  ProjectStatus  { get; set; }

		public virtual String ProjectStatusSortName { get; set; }


		[DisplayName("Status")]
		public virtual String ProjectPhase { get; set; }

		[DisplayName("Status")]
		public virtual String ProjectPhaseColour { get; set; }                   

		public virtual DateTime? StartDate  { get; set; }

		[DisplayName("Start Date")]    			  
		public virtual String StartDateString { get; set; }

		public virtual DateTime? EndDate { get; set; }

		[DisplayName("End Date")]
		public virtual String EndDateString { get; set; }

		public virtual int CountOverDueProjects { get; set; }

		public virtual String OverdueFlagClass { get; set; }

		[DisplayName("Actual Start Date")]
		public virtual DateTime? ActualStartDate  { get; set; }

		[DisplayName("Actual Start Date")]
		public virtual String ActualStartDateString { get; set; }

		[DisplayName("Actual End Date")]
		public virtual DateTime? ActualEndDate  { get; set; }

		[DisplayName("Actual End Date")]
		public virtual String ActualEndDateString { get; set; }


		[DisplayName("Allocated Hours")]
		public virtual Double? AllocatedHours  { get; set; }

		[DisplayName("Notes")]
		public virtual String Notes  { get; set; }

		[DisplayName("Description")]
		public virtual String Description  { get; set; }

		[DisplayName("Reference Code")]
		public virtual String ReferenceCode { get; set; }

		[DisplayName("Code")]
		public virtual String Code { get; set; }

		[DisplayName("Created By")]
		public virtual String CreatedBy  { get; set; }

		[DisplayName("Created Date")]
		public virtual DateTime CreatedDate  { get; set; }

		[DisplayName("Updated By")]
		public virtual String UpdatedBy  { get; set; }

		[DisplayName("Updated Date")]
		public virtual DateTime UpdatedDate  { get; set; }

		[DisplayName("Ticket Status")]
		public virtual String ProjectStatusColor { get; set; }

		public virtual Int32 ClientId { get; set; }

//		public ICollection<Client> Client { get; set; }
//
//		public ICollection<Department> Department { get; set; }
//
//		public ICollection<Issue> Issue { get; set; }

		// public ICollection<ProjectStatus> ProjectStatus { get; set; }

//		public ICollection<Staff> Staff { get; set; }
//
//		public ICollection<Staff> Staff1 { get; set; }
//
//		public ICollection<Staff> Staff2 { get; set; }
//
//		public ICollection<ClientContact> ClientContact { get; set; }
//
//		public ICollection<Staff> Staff3 { get; set; }

		[DisplayName("O/S Tickets")]
		public virtual Int32 OutstandingTickets { get; set; }

		[DisplayName("O/S Tasks")]
		public virtual Int32 OutstandingTask { get; set; }

		public virtual string OverDueIcon { get; set; }

		public virtual Int32 UserId { get; set; }
		public virtual Int32 UserStaffId { get; set; }
		public Int32 Staffcount { get; set; }


		//created by sanjay patel on 30 12 2014
		public virtual int? TotalRows { get; set; }

		public virtual int? OpenTickets { get; set; }  
		public virtual int? OpenTasks { get; set; }
		public virtual int? OpenIssues { get; set; }
		public virtual double? ActualHrs { get; set; }

		public virtual String DepartmentColor { get; set; }

	}
}

