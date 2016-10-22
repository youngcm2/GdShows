using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Logging;
using GdShows.API.V1;
using Models.Api;
using Models.ViewModels;

namespace GdShows.API.Services
{
	class ServiceInvoker<TViewModel, TDataModel> : IServiceInvoker<TViewModel, TDataModel> where TViewModel : BaseViewModel
    {
		private readonly IMapper _mapper;
		private readonly ILog _log = LogProvider.For<ServiceInvoker<TViewModel, TDataModel>>();

		public ServiceInvoker(IMapper mapper)
		{
			_mapper = mapper;
		}

		public async Task<Models.ViewModels.PagedList<TViewModel>> RetrievePagedItems(Func<Task<Models.Api.PagedList<TDataModel>>> retreive)
		{
			try
			{
				var items = await retreive();

				var mapped = _mapper.MapPagedList<TDataModel, TViewModel>(items);

				return mapped;
			}
			catch (Exception exception)
			{
				_log.ErrorException($"Error occured in {nameof(RetrievePagedItems)}", exception);

				throw new ApiException(HttpStatusCode.BadRequest, exception.Message, exception);
			}
		}

		public async Task<IEnumerable<TViewModel>> RetrieveItems(Func<Task<IEnumerable<TDataModel>>> retreive)
		{
			var items = await retreive();

			var mapped = _mapper.Map<IEnumerable<TViewModel>>(items);

			return mapped;
		}

		public async Task<TViewModel> Retrieve(Func<Task<TDataModel>> retreive)
		{
			var item = await retreive();

			var mapped = _mapper.Map<TViewModel>(item);

			return mapped;
		}

		public async Task<bool> Delete(Func<Task<bool>> delete)
		{
            try
            {
                return await delete();
            }
            catch (Exception exception)
            {
                _log.ErrorException($"Error occured in {nameof(Delete)}", exception);

                throw new ApiException(HttpStatusCode.BadRequest, exception.Message, exception);
            }
        }
		
		public async Task<TViewModel> AddAsync(TViewModel item, Func<TDataModel, Task<TDataModel>> add) 
		{
            try
            {
                item.Id = Guid.NewGuid();

                var mapped = _mapper.Map<TDataModel>(item);

                var newItem = await add(mapped);

                var mappedNewItem = _mapper.Map<TViewModel>(newItem);

                return mappedNewItem;
            }
            catch (Exception exception)
            {
                _log.ErrorException($"Error occured in {nameof(AddAsync)}", exception);

                throw new ApiException(HttpStatusCode.BadRequest, exception.Message, exception);
            }
        }

        public async Task<TViewModel> UpdateAsync(TViewModel item, Func<TDataModel, Task<TDataModel>> update)
        {
            try
            {
                var mapped = _mapper.Map<TDataModel>(item);

                var newItem = await update(mapped);

                var mappedNewItem = _mapper.Map<TViewModel>(newItem);

                return mappedNewItem;
            }
            catch (Exception exception)
            {
                _log.ErrorException($"Error occured in {nameof(UpdateAsync)}", exception);

                throw new ApiException(HttpStatusCode.BadRequest, exception.Message, exception);
            }
        }
	}
}
