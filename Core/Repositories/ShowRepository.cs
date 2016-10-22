using System.Data.Entity;
using Data;
using Data.Repositories;
using Models.Api;

namespace Core.Repositories
{
	class ShowRepository : BaseRepository<Show>, IShowRepository
	{
		public ShowRepository(IGdShowContext context, CurrentUserContext currentUserContext) : base(context, currentUserContext)
		{

		}

		protected override DbSet<Show> DbSet => Context.Shows;
	}
}
