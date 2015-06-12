using System;

namespace LinkOM
{
	public class ProjectTicketChart
	{
		public string StatusName { get; set; }
		public string TicketStatusColor { get; set; }
		public int? TotalTicket { get; set; }
	}

	public class ProjectTaskChart
	{
		public string StatusName { get; set; }
		public string TaskStatusColor { get; set; }
		public int? TotalTask { get; set; }
	}
}

