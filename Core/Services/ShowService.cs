using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Core.Repositories;
using Data;
using Data.Repositories;
using LinqKit;
using Models.Api;

namespace Core.Services
{
	class ShowService : IShowService
	{
		private readonly IShowRepository _repository;

		public ShowService(IShowRepository repository)
		{
			_repository = repository;
		}

		public async Task<Show> RetrieveShowAsync(Guid id)
		{
			var show = await _repository.Queryable.FirstOrDefaultAsync(c => c.Id == id);

			return show;
		}

		public async Task<PagedList<Show>> RetrieveShowsAsync(string searchValue, Pagable pagable, Sortable sortable, Guid? pledgeTypeId = null)
		{
			var query = _repository.Queryable;

			var predicate = PredicateBuilder.New<Show>(true);

			if (!string.IsNullOrEmpty(searchValue))
			{
				predicate = predicate.And(show => show.Venue.Contains(searchValue));
			}

			var propertyName = sortable.SortName ?? "ShowDate";
			var descending = sortable.Descending;

			query = query
				.AsExpandable()
				.Where(predicate)
				.OrderBy(propertyName, descending);

			var shows = await query.ToPagedListAsync(pagable);

			return shows;
		}

		public async Task<Show> AddShow(Show show)
		{
			var newShow = await _repository.AddAsync(show);

			return newShow;
		}

		public async Task<Show> UpdateShow(Show show)
		{

			var updatedShow = await _repository.UpdateAsync(show);

			return updatedShow;

		}

		public async Task<bool> DeleteShow(Guid id)
		{
			await _repository.DeleteAsync(id);
			return true;
		}
	}
}
