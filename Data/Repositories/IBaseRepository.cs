using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories
{
	public interface IBaseRepository<TModel> where TModel : class
	{
		IQueryable<TModel> Queryable { get; }
		Task<TModel> AddAsync(TModel entity);
		Task<TModel> UpdateAsync(TModel entity);
		Task<bool> DeleteAsync(Guid id);
		Task<bool> DeleteAsync(Expression<Func<TModel, bool>> predicate);
	}
}