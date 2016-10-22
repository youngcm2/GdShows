using Models.ViewModels;

namespace Models.Api
{
	public class LoginResult
	{
		public string SecurityToken { get; set; }
		public User CurrentUser { get; set; }
	}
}
