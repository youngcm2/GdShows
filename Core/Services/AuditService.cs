using System.Threading.Tasks;
using Models.Security;

namespace Core.Services
{
	internal class AuditService : IAuditService
	{
		private readonly ICurrentUserService _currentUserService;

		public AuditService(ICurrentUserService currentUserService)
		{
			_currentUserService = currentUserService;
		}

		public async Task AddAuditAsync(AuditAction action, string message, string username = null)
		{
			if (string.IsNullOrEmpty(username))
				username = _currentUserService.GetCurrentUser() ?? "Anonymous";

			await Task.Run(() => { });
		}
	}
}