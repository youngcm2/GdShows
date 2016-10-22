using System;
using Models.ViewModels;

namespace Models.Api
{
	public class CurrentUserContext
	{
		private string _username;
		public Guid Id { get; set; }
		public string Username
		{
			get
			{
				return _username ?? (_username = "missing");
			}
			set
			{
				_username = value;
			}
		}

		public UserType? Role { get; set; }

	}
}
