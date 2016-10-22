using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace GdShows.API.V1
{
    public static class MappingExtensions
    {
	    public static global::Models.ViewModels.PagedList<TDestination> MapPagedList<TSource, TDestination>(this IMapper mapper, global::Models.Api.PagedList<TSource> source)
	    {
		    var items = mapper.Map<List<TDestination>>(source.ToList());
		    var pagedList = new global::Models.ViewModels.PagedList<TDestination>(items, source.Offset, source.TotalCount, source.Limit);
		    return pagedList;
	    }
    }
}
