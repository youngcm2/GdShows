using System;

namespace Models.ViewModels
{
	public class User
	{
		public Guid Id { get; set; }
		public UserType UserType { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public string Password { get; set; }
		public UserStatusType Status { get; set; }
		public DateTimeOffset? PasswordExpire { get; set; } // PasswordExpire
		public int AccessFailedCount { get; set; } // AccessFailedCount
		public DateTimeOffset? LockoutEnd { get; set; } // LockoutEnd
	}
}
