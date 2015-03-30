using System;

namespace LinkOM
{
	[Serializable]
	public class Status
	{
		public virtual Int32 Id  { get; set; }

		public virtual String Name  { get; set; }


		public virtual String ColourName  { get; set; }


		public virtual Int32 DisplayOrder  { get; set; }


		public virtual String Description  { get; set; }


		public virtual Int32? MainStatus  { get; set; }

		public string MainStatusName { get; set; }
	}
}

