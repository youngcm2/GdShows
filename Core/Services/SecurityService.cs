using Effortless.Net.Encryption;
using SmartFormat;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Uw.EaPlatform.Common.Models;
using Uw.EaPlatform.Common.Services;
using Uw.EaPlatform.Common.Utilities;
using Uw.EaPlatform.Core.Repositories;
using Uw.EaPlatform.Models.Api;
using Uw.EaPlatform.Models.Security;
using Uw.EaPlatform.Models.ViewModels;
using LoginResult = Uw.EaPlatform.Core.Models.LoginResult;
using User = Uw.EaPlatform.Data.Models.User;

namespace Uw.EaPlatform.Core.Services
{
    class SecurityService : ISecurityService
	{
		private readonly IAuditService _auditService;
		private readonly IMailService _mailService;
		private readonly IEncryptionService _encryptionService;
		private readonly IHasher _hasher;

		private readonly Regex _passwordComplexity = new Regex(@"(?=^.{8,255}$)((?=.*\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.*", RegexOptions.Compiled);
		private readonly IPathProvider _pathProvider;
		private readonly IUserRepository _repository;
        private readonly IAgencyRepository _agencyRepository;


        public SecurityService(
            IUserRepository repository,
            IAgencyRepository agencyRepository,
            IPathProvider pathProvider, 
            IAuditService auditService, 
            IMailService mailService, 
            IEncryptionService encryptionService, 
            IHasher hasher
            )
		{
			_repository = repository;
            _agencyRepository = agencyRepository;
            _pathProvider = pathProvider;
			_auditService = auditService;
			_mailService = mailService;
			_encryptionService = encryptionService;
			_hasher = hasher;
		}

		public async Task UnlockAccount(Guid id)
		{
			var currentUser = await _repository.Queryable.FirstOrDefaultAsync(user => user.Id == id);

			currentUser.LockoutEnd = null;
			currentUser.AccessFailedCount = 0;

			await _auditService.AddAuditAsync(AuditAction.UnlockAccount, $"Account has been unlocked for {currentUser.EmailAddress}");

			await _repository.UpdateAsync(currentUser);
		}

		public async Task SendPasswordResetTokenAsync(string email)
		{
			var baseUrl = _pathProvider.GetBaseUrl();
			var user = await _repository.Queryable.FirstOrDefaultAsync(x => x.EmailAddress == email);

			if (user == null)
				return;

			var token = Strings.CreatePassword(30, false);
			var resetToken = $"{email}|{token}|{user.Id}";
			string hash;
			_encryptionService.TryEncrypt(resetToken, out hash);

			var queryString = $"token={hash}";
			var url = $"{baseUrl}/resetpassword?{queryString}";
			var templatePath = _pathProvider.MapPath("emailtemplates/PasswordReset.html");
			var templateContent = File.ReadAllText(templatePath);
			var content = Smart.Format(templateContent, new { url });
			await _mailService.SendEmailAsync(content, new [] { email }, "Password Reset Link");

			await _auditService.AddAuditAsync(AuditAction.ChangePasswordRequest, $"Password reset token requested for {email}");
		}

		private bool IsPasswordValid(string password)
		{
			return _passwordComplexity.IsMatch(password);
		}

		private bool IsPastMaximumAge(User user)
		{
			return user.PasswordExpire <= DateTimeOffset.UtcNow;
		}

		public bool IsLockedOut(User user)
		{
			return user.LockoutEnd >= DateTimeOffset.UtcNow;
		}

		public async Task<LoginResult> LoginAsync(string email, string password)
		{
			email = email.Trim().ToLower();
			password = password.Trim();

			var user = await _repository.Queryable.FirstOrDefaultAsync(x => x.EmailAddress == email);

			if (user == null)
			{
				await _auditService.AddAuditAsync(AuditAction.LoginFailed, $"Invalid Email or Password: {email}");

				return new LoginResult { Success = false, Message = "Invalid Email or Password" };
			}

			if (IsLockedOut(user))
			{
				await _auditService.AddAuditAsync(AuditAction.LoginFailed, $"Account is locked out: {email}");

				return new LoginResult { Success = false, Message = "Account is locked out" };
			}

			if (IsPastMaximumAge(user))
			{
				await _auditService.AddAuditAsync(AuditAction.LoginFailed, $"Password is expired: {email}");

				return new LoginResult { Success = false, Message = "Password is expired" };
			}

            var verified = _hasher.Verify(user.PasswordHash, password, user.PasswordSalt);

			if (!verified)
			{
				if (user.AccessFailedCount < 5)
					user.AccessFailedCount++;
				else
					user.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(5);

				await _repository.UpdateAsync(user);

				await _auditService.AddAuditAsync(AuditAction.LoginFailed, $"Invalid Email or Password: {email}");

				return new LoginResult { Success = false, Message = "Invalid Email or Password" };
			}

			if (user.Status != UserStatusType.Active)
			{
				await _auditService.AddAuditAsync(AuditAction.LoginFailed, $"User is not active: {email}");

				return new LoginResult { Success = false, Message = "Inactive user" };
			}

			user.AccessFailedCount = 0;
			user.LockoutEnd = null;

			await _auditService.AddAuditAsync(AuditAction.LoginSuccess, $"User logged in: {email}");

            ///TODO/discuss if many to many how do we pick one for the initial client load
            //var agency = await _agencyRepository.Queryable.FirstOrDefaultAsync(x => x.Id == user.UserAgencies.FirstOrDefault().AgencyId);

            return new LoginResult { Success = true, CurrentUser = user };
		}

		public async Task<string> UpdatePasswordAsync(string token, string newPassword)
		{
			string tokenElements;
			if (!_encryptionService.TryDecrypt(token, out tokenElements))
			{
				return "Token is invalid.";
			}

			var userId = tokenElements.Split('|').LastOrDefault();

			if (string.IsNullOrWhiteSpace(userId))
			{
				return "Token is invalid.";
			}

			Guid id;
			if (!Guid.TryParse(userId, out id))
			{
				return "Invalid user.";
			}

			var user = await _repository.Queryable.FirstOrDefaultAsync(u => u.Id == id);

			if (user == null)
			{
				return "Invalid user.";
			}

			if (IsPasswordValid(newPassword))
			{
				return "Password does not meet minimum requirements.";
			}

			var passwordResult = _hasher.Hash(newPassword);

			user.PasswordHash = passwordResult.Hash;
			user.PasswordSalt = passwordResult.Salt;

			await _repository.UpdateAsync(user);
			await _auditService.AddAuditAsync(AuditAction.ChangePassword, $"Password changed for {user.EmailAddress}");
			return "Success";
		}

	    public HashResult CreatePasswordHash(string password)
	    {

            if (IsPasswordValid(password))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Password does not meet minimum requirements.");
            }

            var passwordResult = _hasher.Hash(password);

	        return passwordResult;
	    }
	}
}