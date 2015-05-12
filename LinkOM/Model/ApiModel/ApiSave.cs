using System;

namespace LinkOM
{
	public class ApiSave<T>
	{
		public int UserId { get; set; }
		public string UserName { get; set; }

		public string TokenNumber { get; set; }


		public T Item { get; set; }
	}
}

