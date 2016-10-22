using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Api;
using Models.ViewModels;

namespace GdShows.API.Services
{
    public interface IServiceInvoker<TViewModel, TDataModel> where TViewModel : BaseViewModel
    {
        Task<TViewModel> AddAsync(TViewModel item, Func<TDataModel, Task<TDataModel>> add);
        Task<TViewModel> UpdateAsync(TViewModel item, Func<TDataModel, Task<TDataModel>> update);
        Task<Models.ViewModels.PagedList<TViewModel>> RetrievePagedItems(Func<Task<Models.Api.PagedList<TDataModel>>> retreive);
	    Task<IEnumerable<TViewModel>> RetrieveItems(Func<Task<IEnumerable<TDataModel>>> retreive);
	    Task<TViewModel> Retrieve(Func<Task<TDataModel>> retreive);
	    Task<bool> Delete(Func<Task<bool>> delete);
    }
}
