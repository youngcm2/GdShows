using System.Collections.Generic;

namespace Models.Api
{
	public interface IPagedList<T> : IList<T>
	{

		bool HasNextPage { get; }

		bool HasPreviousPage { get; }

		bool IsFirstPage { get; }

		bool IsLastPage { get; }

		int PageCount { get; }

		int PageIndex { get; }

		int Limit { get; }

		int Offset { get; }

		int TotalCount { get; }
	}
}