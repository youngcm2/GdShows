using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Core.Services;
using LinqKit;
using Uw.EaPlatform.Core.Repositories;
using Uw.EaPlatform.Data.Repositories;
using Uw.EaPlatform.Models.Api;
using Uw.EaPlatform.Models.ViewModels;
using User = Uw.EaPlatform.Data.Models.User;

namespace Uw.EaPlatform.Core.Services
{
	class UserService : IUserService
	{
		private readonly IUserRepository _repository;
		private readonly ISecurityService _securityService;
		private readonly IUserAgencyRepository _userAgencyRepository;

		public UserService(IUserRepository repository, ISecurityService securityService, IUserAgencyRepository userAgencyRepository)
		{
			_repository = repository;
			_securityService = securityService;
			_userAgencyRepository = userAgencyRepository;
		}

        public async Task<EaPlatform.Models.Api.PagedList<Data.Models.User>> SearchUsers(string searchValue, Pagable pagable, Sortable sortable, Guid? agencyId, UserType? userTypeId)
		{
            var query = _repository.Queryable;

            var predicate = PredicateBuilder.New<User>(true);

            //agencyId filter
            if (agencyId.HasValue)
            {
                predicate = predicate.And(u => u.UserAgencies.Any(ua => ua.AgencyId == agencyId.Value));
            }

            //userTypeId filter
            if (userTypeId.HasValue)
			{
                predicate = predicate.And(u => u.UserType == userTypeId);
			}

            //search string filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                //Username
                //FirstName
                //LastName
                //EmailAddress
                //OldAgencyName
                predicate = predicate.And(predicate
                    .Or(user => user.LastName.Contains(searchValue) || user.FirstName.Contains(searchValue))
                    .Or(user => user.Username.Contains(searchValue) || user.EmailAddress.Contains(searchValue))
                    .Or(user => user.OldAgencyName.Contains(searchValue)));
            }


            var propertyName = sortable.SortName ?? "Username";
            var descending = sortable.Descending;

            query = query
                .AsExpandable()
                .Include(u => u.UserAgencies.Select(ua => ua.Agency))
                .Include(u => u.Company)
                .Where(predicate)
                .OrderBy(propertyName, descending);

            var users = await query.ToPagedListAsync(pagable);

            return users;
        }

		public async Task<User> GetUserById(Guid id)
		{
			return await _repository.Queryable
				.Where(u => u.Id == id)
				.Include(u => u.UserAgencies.Select(ua => ua.Agency))
				.FirstOrDefaultAsync();
		}

		public async Task<User> AddAsync(User user)
		{
			var createPassword = _securityService.CreatePasswordHash(user.Password);
			user.PasswordHash = createPassword.Hash;
			user.PasswordSalt = createPassword.Salt;

			var userAgencies = user.UserAgencies.ToList();

			user.UserAgencies.Clear();

			var addedUser = await _repository.AddAsync(user);

            foreach (var userAgency in userAgencies)
			{
                userAgency.Id = Guid.NewGuid();
                userAgency.UserId = addedUser.Id;
                userAgency.EnteredBy = addedUser.EnteredBy;
                userAgency.EnteredDate = addedUser.EnteredDate;

                addedUser.UserAgencies.Add(userAgency);
			}

			return await _repository.UpdateAsync(addedUser);
		}

		public async Task<User> UpdateUser(User user)
		{
			var createPassword = _securityService.CreatePasswordHash(user.Password);
			user.PasswordHash = createPassword.Hash;
			user.PasswordSalt = createPassword.Salt;

            var userAgencies = user.UserAgencies.ToList();

            user.UserAgencies.Clear();

            await _userAgencyRepository.DeleteAsync(agency => agency.UserId == user.Id);

            var updatedUser = await _repository.UpdateAsync(user);
                        
            foreach (var userAgency in userAgencies)
            {
                userAgency.Id = Guid.NewGuid();
                userAgency.UserId = updatedUser.Id;
                userAgency.EnteredBy = updatedUser.EnteredBy;
                userAgency.EnteredDate = updatedUser.EnteredDate;

                updatedUser.UserAgencies.Add(userAgency);
            }

            updatedUser = await _repository.UpdateAsync(updatedUser); 

            return updatedUser;
		}
	}
}