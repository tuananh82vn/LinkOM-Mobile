using System;
using Android.OS;
using Java.Interop;

namespace LinkOM
{
	[Serializable]
	public class DocumentCommentObject
	{
		public virtual Int32 Id { get; set; }

		public virtual String DocumnetversionGuid { get; set; }

		public virtual String DocumentVersion { get; set; }        

		public virtual Int32 DocumentCategoryId { get; set; }

		public virtual String DocumentCategoryName { get; set; }

		public virtual String Title { get; set; }

		public virtual Int32? FileReferenceId { get; set; }

		public virtual String FileName { get; set; }

		public virtual String PublishBy { get; set; }

		public virtual int? DocumentVersionNo { get; set; }

		public virtual Nullable<DateTime> CreatedDate { get; set; }

		public virtual string CreatedDateString { get; set; }
	}
}

