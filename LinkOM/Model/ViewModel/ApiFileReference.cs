using System;

namespace LinkOM
{
	public class ApiFileReference
	{
		public virtual int? FileId { get; set; }
		public virtual string FileName { get; set; }
		public virtual string ContentType { get; set; }
		public virtual long Size { get; set; }
		public virtual string FileContent { get; set; }
	}
}

