using System;
using Models.ViewModels;

namespace GdShows.API
{
    public class SecurityToken
    {
        public Guid UserId { get; set; }
	    public string Username { get; set; }
        public DateTimeOffset Expires { get; set; }
        public UserType? Role { get; set; }
        public string EnvName { get; set; }
    }
}
