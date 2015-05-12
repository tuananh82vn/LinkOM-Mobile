using System;

namespace LinkOM
{
	public class StaffList
	{
		public virtual int Id { get; set; }

		public virtual string Name { get; set; }

		public virtual int? AccessLevel { get; set; }

		public virtual int IsDefaut { get; set; }
	}
}

