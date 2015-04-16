using System;
using System.Collections.Generic;

namespace LinkOM
{
	public class IssuesList
	{
		public List<IssuesObject> Items { get; set; }
		public int PageNumber { get; set; }
		public int TotalRecords { get; set; }
		public bool Success { get; set; }
		public string ErrorMessage { get; set; }
	}
}

