using System;
using System.Threading.Tasks;
using Data;
using Models.Api;

namespace Core.Services
{
	public interface IShowService
	{
		Task<Show> RetrieveShowAsync(Guid id);
		Task<PagedList<Show>> RetrieveShowsAsync(string searchValue, Pagable pagable, Sortable sortable, Guid? pledgeTypeId = null);
		Task<Show> AddShow(Show show);
		Task<Show> UpdateShow(Show show);
		Task<bool> DeleteShow(Guid id);
	}
}