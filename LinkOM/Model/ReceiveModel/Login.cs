using System;

namespace LinkOM
{
	public class LoginJson
	{
		public int UserId { get; set; }
		public string TokenNumber { get; set; }
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Designation { get; set; }
		public string Phone { get; set; }
		public string Mobile { get; set; }
		public int RoleId { get; set; }
		public int UserPhotoId { get; set; }
		public int TimeZoneId { get; set; }
		public bool Success { get; set; }
		public string ErrorMessage { get; set; }

	}
}

