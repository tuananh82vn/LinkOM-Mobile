﻿using System;
using Android.OS;
using Java.Interop;

namespace LinkOM
{
	[Serializable]
	public class IssuesCommentList
	{
		public virtual int? TotalRows { get; set; }
		public virtual int? ID { get; set; }
		public virtual int? IssueId { get; set; }
		public virtual int? UserId { get; set; }
		public virtual string Comment { get; set; }
		public virtual string Status { get; set; }
		public virtual bool? IsInternal { get; set; }
		public virtual Double? SpentHours { get; set; }
		public virtual string CreatedBy { get; set; }
		public virtual DateTime? CreatedDate { get; set; }
		public virtual int? OwnerId { get; set; }
		public virtual int? AssignedToId { get; set; }
		public virtual int? PriorityId { get; set; }
		public virtual string PriorityName { get; set; }
		public virtual string PriorityColor { get; set; }
		public virtual int? IssueStatusId { get; set; }
		public virtual string IssueStatusName { get; set; }
		public virtual string IssueStatusColor { get; set; }
		public virtual int? IssueMainStatus { get; set; }
		public virtual string UserName { get; set; }
		public virtual string AssignedTo { get; set; }
		public virtual int? FileId { get; set; }
		public virtual string TimeZoneCode { get; set; }

		public virtual String CreatedDateString { get; set; }
	}
}

