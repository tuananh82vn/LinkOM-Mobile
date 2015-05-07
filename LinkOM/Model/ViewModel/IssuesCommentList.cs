using System;
using Android.OS;
using Java.Interop;

namespace LinkOM
{
	[Serializable]
	public class IssuesCommentList
	{
		public int Id { get; set; }

		public virtual string UserName { get; set; }

		public virtual String Comment { get; set; }

		public virtual String CommentDetail { get; set; }

		public virtual String Status { get; set; }

		public virtual String AssignedTo { get; set; }

		public virtual Boolean IsInternal { get; set; }

		public virtual Int32? OwnerId { get; set; }

		public virtual string OwnerName { get; set; }

		public virtual Int32? AssignedToId { get; set; }

		public virtual string AssignedToName { get; set; }

		public virtual Int32? PriorityId { get; set; }

		public virtual string PriorityName { get; set; }

		public virtual Int32? StatusId { get; set; }

		public virtual String CreatedBy { get; set; }

		public virtual DateTime? CreatedDate { get; set; }

		public string StatusColor { get; set; }

		public string PriorityColor { get; set; }

		public int? FileId { get; set; }

		public bool IsIconDisplay { get; set; }

		public double? SpentHours { get; set; }
	}
}

