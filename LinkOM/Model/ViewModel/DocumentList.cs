using System;

namespace LinkOM
{
	public class DocumentList
	{
		public virtual Int32 Id { get; set; }

		public virtual String ProjectGuid { get; set; }

	
		public virtual String Title { get; set; }

		public virtual Int32 DocumentCategoryId { get; set; }

		public virtual String DocumentCategoryName { get; set; }

		public virtual Int32 ProjectId { get; set; }

		public virtual String ProjectName { get; set; }

		public virtual Int32? FileReferenceId { get; set; }

		public virtual String FileName { get; set; }

		public virtual Boolean IsSendEmailToClient { get; set; }

		public virtual Boolean IsInternal { get; set; }

		public virtual String Description { get; set; }

		public virtual String CreatedBy { get; set; }

		public virtual Nullable<DateTime> CreatedDate { get; set; }

		public virtual string CreatedDateString { get; set; }

		public virtual DateTime calDays { get; set; }

		public virtual string Guid { get; set; }

		//created by sanjay patel on  30 12 2014
		public virtual int? TotalRows { get; set; }

		public virtual bool? IsFromClient { get; set; }
	}
}

