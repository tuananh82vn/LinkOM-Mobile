using System;

namespace LinkOM
{
	[Serializable]
	public class DashboardObject
	{
		public virtual int? ProjectOpenCount { get; set; }

		public virtual int? TaskOverdueCount { get; set; }
		public virtual int? TaskOpenCount { get; set; }
		public virtual int? TaskClosedCount { get; set; }
		public virtual int? TaskOtherCount { get; set; }


		public virtual int? TicketOverdueCount { get; set; }
		public virtual int? TicketOpenCount { get; set; }
		public virtual int? TicketClosedCount { get; set; }
		public virtual int? TicketOtherCount { get; set; }


		public virtual int? IssueOverdueCount { get; set; }
		public virtual int? IssueOpenCount { get; set; }
		public virtual int? IssueClosedCount { get; set; }
		public virtual int? IssueOtherCount { get; set; }


		public virtual int? MilestoneOverdueCount { get; set; }
		public virtual int? MilestoneOpenCount { get; set; }
		public virtual int? MilestoneClosedCount { get; set; }
		public virtual int? MilestoneOtherCount { get; set; }


	}
}

