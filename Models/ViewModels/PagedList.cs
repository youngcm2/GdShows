using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.ViewModels
{
	public class PagedList<T>
	{
		public PagedList(IEnumerable<T> items, int offset, int totalItemCount, int? limit = null)
		{
			Items = items.ToArray();
			TotalCount = totalItemCount; //57
			Limit = limit ?? totalItemCount; //10
			Offset = offset; //20
			if (TotalCount > 0)
			{
				PageCount = (int)Math.Ceiling(TotalCount / (double)Limit); //6
				PageIndex = (int)Math.Ceiling(Offset / (double)Limit + 1); // 20 / 10 + 1 = 3
			}
			else
			{
				PageCount = 0;
			}

			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(offset), offset, "PageIndex cannot be below 0.");
			}
			if (limit < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(limit), limit, "PageSize cannot be less than 1.");
			}
		}

		public T [] Items { get; set; }

		public bool HasNextPage => Offset + Limit <= PageCount;

		public bool HasPreviousPage => Offset > 0;

		public bool IsFirstPage => Offset <= 0;

		public bool IsLastPage => Offset + Limit >= PageCount - 1;

		public int PageCount { get; protected set; } //6

		public int Offset { get; protected set; } //20 (page 3)

		public int PageIndex { get; protected set; }

		public int Limit { get; protected set; }

		public int TotalCount { get; set; }
	}
}
