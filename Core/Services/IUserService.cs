using System;
using System.Threading.Tasks;
using Models.Api;
using Models.ViewModels;

namespace Core.Services
{
	public interface IUserService
	{
		Task<Models.Api.PagedList<User>> SearchUsers(string searchValue, Pagable pageable, Sortable sortable, Guid? agencyId, UserType? userTypeId);

		Task<User> GetUserById(Guid id);
		Task<User> AddAsync(User user);
		Task<User> UpdateUser(User user);
	}
}