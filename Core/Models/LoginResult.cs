using Data;

namespace Core.Models
{
	public class LoginResult
	{
		public bool Success { get; set; }
		public string Message { get; set; }
		public string SecurityToken { get; set; }
		public User CurrentUser { get; set; }
	}
}
