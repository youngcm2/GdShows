using System.Threading.Tasks;
using Models.Security;

namespace Core.Services
{
	public interface IAuditService
	{
		Task AddAuditAsync(AuditAction action, string message, string username = null);
	}
}