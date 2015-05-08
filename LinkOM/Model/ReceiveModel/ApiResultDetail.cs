using System;

namespace LinkOM
{
	public class ApiResultDetail<T> : ApiResult
	{
		public T Item { get; set; }
	}
}

