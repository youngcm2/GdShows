using System;
using System.Threading.Tasks;
using AutoMapper;
using Core.Services;
using GdShows.API.Services;
using GdShows.Filters;
using Microsoft.AspNetCore.Mvc;
using Models.Api;
using Models.ViewModels;
using VM = Models.ViewModels;

namespace GdShows.API.V1
{
	[Route("api/v1")]
	//[EnsureToken]
	public class ShowController : BaseController
	{
		private readonly IShowService _showService;
		private readonly IMapper _mapper;
		private readonly IServiceInvoker<Show, Data.Show> _serviceInvoker;

		public ShowController(
			IShowService showService,
			IMapper mapper,
			IServiceInvoker<Show, Data.Show> serviceInvoker)
		{
			_showService = showService;
			_mapper = mapper;
			_serviceInvoker = serviceInvoker;
		}

		[HttpGet]
		//[EnsureToken]
		[Pagable("ShowDate")]
		[Route("api/v1/shows")]
		public async Task<VM.PagedList<Show>> SearchShows([FromQuery]string searchValue)
		{
			var result = await _serviceInvoker.RetrievePagedItems(async () => await _showService.RetrieveShowsAsync(searchValue, Paging, Sorting));

			return result;
		}

		[HttpGet]
		[Route("api/v1/shows/{id}")]
		public async Task<Show> RetrieveShow([FromRoute]Guid id)
		{
			var result = await _serviceInvoker.Retrieve(async () => await _showService.RetrieveShowAsync(id));

			return result;
		}

		[HttpPost]
		[Route("api/v1/shows")]
		public async Task<Show> AddShow([FromBody]Show show)
		{
			return await _serviceInvoker.AddAsync(show, _showService.AddShow);
		}

		[HttpPut]
		[Route("api/v1/shows/{id}")]
		public async Task<Show> UpdateShow([FromBody] Show show)
		{
			return await _serviceInvoker.UpdateAsync(show, _showService.UpdateShow);
		}

		[HttpDelete]
		[Route("api/v1/shows/{id}")]
		public async Task DeleteShow([FromRoute] Guid id)
		{
			await _serviceInvoker.Delete(() => _showService.DeleteShow(id));
		}

	}
}