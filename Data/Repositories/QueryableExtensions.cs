using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models.Api;

namespace Data.Repositories
{
	public static class QueryableExtensions
	{
		public static IPagedList<T> ToPagedList<T>(this IQueryable<T> queryable, Pagable pagable = null)
		{
			var totalCount = queryable.Count();

			List<T> list;

			if (pagable == null)
			{
				list = queryable.ToList();
				return new PagedList<T>(list, 0, list.Count);
			}

			var skip = pagable.Offset.GetValueOrDefault();

			queryable = queryable.Skip(skip);

			var limit = pagable.Limit;

			if (limit.HasValue)
			{
				queryable = queryable.Take(limit.Value);
			}

			list = queryable.ToList();

			var pagedList = new PagedList<T>(list, skip, totalCount, limit);

			return pagedList;

		}

		public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> queryable, Pagable pagable = null)
		{
			var totalCount = queryable.Count();

			List<T> list;

			if (pagable == null)
			{
				list = queryable.ToList();
				return new PagedList<T>(list, 0, list.Count);
			}

			var skip = pagable.Offset.GetValueOrDefault();

			queryable = queryable.Skip(skip);

			var limit = pagable.Limit;

			if (limit.HasValue)
			{
				queryable = queryable.Take(limit.Value);
			}

			list = await queryable.ToListAsync();

			var pagedList = new PagedList<T>(list, skip, totalCount, limit);

			return pagedList;

		}

		public static IOrderedQueryable<T> OrderingHelper<T>(this IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
		{
			var param = Expression.Parameter(typeof(T), "x");
			//allows for nested
			var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);
			var sort = Expression.Lambda(body, param);
			var call = Expression.Call(
				typeof(Queryable),
				(!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
				new [] { typeof(T), body.Type },
				source.Expression,
				Expression.Quote(sort));
			return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
		}

		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool descending = false)
		{
			return OrderingHelper(source, propertyName, descending, false);
		}

		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
		{
			return OrderingHelper(source, propertyName, true, false);
		}

		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName, bool descending = false)
		{
			return OrderingHelper(source, propertyName, descending, true);
		}

		public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
		{
			return OrderingHelper(source, propertyName, true, true);
		}

	}
}
