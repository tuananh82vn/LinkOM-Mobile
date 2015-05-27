using System;

namespace LinkOM
{
	public class DocumentDetailList
	{

		public virtual Int32 Id { get; set; }

		public virtual string Guid { get; set; }


		public virtual string ProjectGuid { get; set; }

		public virtual Int32 ProjectId { get; set; }

		public virtual String ProjectName { get; set; }

		public virtual String Title { get; set; }

		public virtual Int32 DocumentCategoryId { get; set; }

		public virtual String DocumentCategoryName { get; set; }

		public virtual String Label { get; set; }

		public virtual Int32 FileReferenceId { get; set; }

		public virtual String Description { get; set; }

		public virtual String CreatedBy { get; set; }

		public virtual DateTime CreatedDate { get; set; }

		public virtual bool IsFromClient { get; set; }

		public virtual bool IsInternal { get; set; }

		public virtual bool IsSendEmailToClient { get; set; }

		public virtual String ProjectManager { get; set; }

		public virtual String ProjectManagerGuid { get; set; }

		public virtual String DepartmentName { get; set; }

		public virtual String ProjectCoordinatorName { get; set; }

		public virtual String ProjectDeliveryManagerName { get; set; }

		public virtual String ClientName { get; set; }
	}
}

