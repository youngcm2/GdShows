using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models.Api;
using Z.EntityFramework.Plus;

namespace Data.Repositories
{
	public abstract class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : BaseEntity
	{
		protected IGdShowContext Context { get; }
		public CurrentUserContext CurrentUserContext { get; }
		protected abstract DbSet<TModel> DbSet { get; }

		public IQueryable<TModel> Queryable => DbSet.AsNoTracking();

		protected BaseRepository(IGdShowContext context, CurrentUserContext currentUserContext)
		{
			Context = context;
			CurrentUserContext = currentUserContext;
		}

		public virtual async Task<TModel> AddAsync(TModel entity)
		{
			var added = DbSet.Add(entity);

			await Context.SaveChangesAsync();

			return added;
		}

		public virtual async Task<TModel> UpdateAsync(TModel entity)
		{
			if (entity == null)
				return null;

			var existing = await DbSet.FirstOrDefaultAsync(model => model.Id == entity.Id);

			if (existing != null)
			{
				
				Context.Entry(existing).CurrentValues.SetValues(entity);

				await Context.SaveChangesAsync();
			}

			return existing;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var existing = await DbSet.FindAsync(id);

			DbSet.Remove(existing);

			await Context.SaveChangesAsync();

			return true;
		}

		public async Task<bool> DeleteAsync(Expression<Func<TModel, bool>> predicate)
		{
			await DbSet.Where(predicate).DeleteAsync();

			return true;
		}
	}
}
