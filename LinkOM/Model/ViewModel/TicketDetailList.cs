using System;

namespace LinkOM
{
	public class TicketDetailList
	{

		public virtual int? Id { get; set; }
		public virtual string Code { get; set; }
		public virtual string Guid { get; set; }
		public virtual string Title { get; set; }
		public virtual DateTime? StartDate { get; set; }
		public virtual DateTime? EndDate { get; set; }
		public virtual int? ProjectId { get; set; }
		public virtual int? AssignedToId { get; set; }
		public virtual int? OwnerId { get; set; }
		public virtual int? PriorityId { get; set; }
		public virtual int? TicketStatusId { get; set; }
		public virtual DateTime? ActualStartDate { get; set; }
		public virtual DateTime? ActualEndDate { get; set; }
		public virtual string Label { get; set; }
		public virtual int? PreviousAssignToId { get; set; }
		public virtual string CreatedBy { get; set; }
		public virtual DateTime? CreatedDate { get; set; }
		public virtual int? TicketTypeId { get; set; }
		public virtual int? TicketReceivedMethodId { get; set; }
		public virtual double? AllocatedHours { get; set; }
		public virtual double? ActualHours { get; set; }
		public virtual bool? IsInternal { get; set; }
		public virtual bool? IsManagement { get; set; }
		public virtual string TicketDiscription { get; set; }
		public virtual int? ClientId { get; set; }
		public virtual int? DepartmentId { get; set; }
		public virtual string ProjectName { get; set; }
		public virtual string ClientName { get; set; }
		public virtual string DepartmentName { get; set; }
		public virtual string DepartmentColor { get; set; }
		public virtual string StatusName { get; set; }
		public virtual string TicketStatusColor { get; set; }
		public virtual int? TicketMainStatus { get; set; }
		public virtual string PriorityName { get; set; }
		public virtual string PriorityColor { get; set; }
		public virtual string TicketTypeName { get; set; }
		public virtual string TicketReceivedMethod { get; set; }
		public virtual string AssignedToName { get; set; }
		public virtual string OwnerName { get; set; }
		public virtual string ProjectManager { get; set; }
		public virtual string DeliveryManager { get; set; }
		public virtual string ProjectCoordinator { get; set; }
		public virtual int? CanEdit { get; set; }
		public virtual int? CanDelete { get; set; }

		public virtual string StartDateString { get; set; }
		public virtual string EndDateString { get; set; }

		public virtual string ActualStartDateString { get; set; }
		public virtual string ActualEndDateString { get; set; }

//		public IEnumerable<ApiFileReference> Attachments { get; set; }
	}
}

