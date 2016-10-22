using System;
using System.Threading.Tasks;
using Uw.EaPlatform.Common.Models;
using Uw.EaPlatform.Core.Models;
using Uw.EaPlatform.Data.Models;

namespace Uw.EaPlatform.Core.Services
{
	public interface ISecurityService
	{
		Task UnlockAccount(Guid id);
		Task SendPasswordResetTokenAsync(string email);
		bool IsLockedOut(User user);
		Task<LoginResult> LoginAsync(string email, string password);
		Task<string> UpdatePasswordAsync(string token, string newPassword);
	    HashResult CreatePasswordHash(string password);
	}
}