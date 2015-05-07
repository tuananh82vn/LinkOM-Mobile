using System;

namespace LinkOM
{
	public class DocumentFilter
	{
		public virtual string Title { get; set; }
		public virtual int? DocumentCategoryId { get; set; }
		public virtual int? ProjectId { get; set; }
		public virtual int? DepartmentId { get; set; }
		public virtual string Label { get; set; }
	}
}

