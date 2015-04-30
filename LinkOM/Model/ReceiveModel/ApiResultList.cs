using System;
using System.Collections.Generic;
using System.Linq;


namespace LinkOM
{
	public class ApiResultList<T> : ApiResult
	{
		public IEnumerable<object> Items;
		public int PageNumber{get;set;}
		public int TotalRecords { get; set; }

	}
}