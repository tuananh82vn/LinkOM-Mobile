using System;
using System.Collections.Generic;
using System.Linq;


namespace LinkOM
{
	public class ApiResultSingle<T> : ApiResult
	{
		public object Item;
		public int PageNumber{get;set;}
		public int TotalRecords { get; set; }

	}
}