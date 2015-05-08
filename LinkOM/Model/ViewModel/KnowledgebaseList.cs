using System;

namespace LinkOM
{
	public class KnowledgebaseList
	{
		public virtual Int32 Id { get; set; }

		public virtual String Title { get; set; }

		public virtual Int32 KnowledgebaseCategoryId { get; set; }

		public virtual String KnowledgebaseCategoryName { get; set; }

		public virtual Int32 ArticleStatusId { get; set; }

		public virtual String ArticleStatusName { get; set; }

		public virtual Nullable<DateTime> PublishDate { get; set; }

		public virtual string PublishDateString { get; set; }

		public virtual Nullable<DateTime> ExpiryDate { get; set; }

		public virtual string ExpiryDateString { get; set; }

		public virtual Int32 ImageId { get; set; }

		public virtual Int32 VideoId { get; set; }

		public virtual Boolean IsInternal { get; set; }

		public virtual Boolean IsAvailableForEveryone { get; set; }

		public virtual String Summary { get; set; }

		public virtual String Content { get; set; }

		public virtual Nullable<Int32> ViewCount { get; set; }

		public virtual Int32 DisplayOrder { get; set; }

		public virtual String UpdatedBy { get; set; }

		public virtual DateTime calDays { get; set; }

		public virtual string Guid { get; set; }

		public virtual int? TotalRows { get; set; }
	}
}

