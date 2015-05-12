using System;

namespace LinkOM
{
	public class SearchAssignedByProject
	{
		public virtual int? ProjectId { get; set; }
		public virtual bool? IsInternal { get; set; }
		public virtual bool? IsManagerial { get; set; }
	}
}

