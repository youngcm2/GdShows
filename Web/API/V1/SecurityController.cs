using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using GdShows.API.Services;
using GdShows.API.V1.Models;
using Microsoft.AspNetCore.Mvc;
using Models.Api;
using Models.ViewModels;
using IConfiguration = Common.IConfiguration;

namespace GdShows.API.V1
{
    [Route("api/v1/[controller]/[action]")]
    public class SecurityController : BaseController 
    {
	    private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
	    private readonly ISecurityTokenService _tokenService;

	    public SecurityController(IConfiguration configuration, IMapper mapper, ISecurityTokenService tokenService) 
		{
		    _configuration = configuration;
            _mapper = mapper;
			_tokenService = tokenService;
		}

        private LoginResult BuildSuccessfulLoginResult(Core.Models.LoginResult loginResult)
        {
	        var token = new SecurityToken
	        {
		        UserId = loginResult?.CurrentUser.Id ?? Guid.Empty,
                Username = loginResult?.CurrentUser?.EmailAddress,
		        Role = loginResult?.CurrentUser?.UserType,
		        Expires = DateTime.UtcNow.AddHours(6),
		        EnvName = _configuration.EnvName
	        };
			
	        //cleanup some secure profile info
	        var encryptedToken = _tokenService.Encrypt(token);
            var mappedUser = _mapper.Map<User>(loginResult?.CurrentUser);
	        var result = new LoginResult
	        {
		        SecurityToken = encryptedToken,
                CurrentUser = mappedUser
	        };
			
	        return result;
        }

        [HttpPost]
        [ExpandParameters]
        public async Task<LoginResult> Login([FromBody]LoginParameters parameters)
        {
            ValidateParameters(parameters);

	        var loginResult = new Core.Models.LoginResult
	        {
		        Success = true,
				CurrentUser = new Data.User
				{
					Id = Guid.NewGuid(),
					UserType = UserType.User
				}
	        };

            if (!loginResult.Success)
            {
                throw new ApiException(HttpStatusCode.Unauthorized, "Failed");
            }

            var result = BuildSuccessfulLoginResult(loginResult);
			await Task.FromResult(0);

			return result;

        }

        [HttpPost]
        [ExpandParameters]
        public async Task<GenericApiResponse> SendResetPasswordRequest([FromBody]EmailParameter parameter)
        {
            ValidateParameters(parameter);

			//send
			await Task.FromResult(0);

			return new GenericApiResponse { Success = true, Message = $"Sent password reset email to '{ parameter.Email }'" };
        }

        [HttpPost]
        [ExpandParameters]
        public async Task<GenericApiResponse> ResetPassword([FromBody]ResetPasswordParameters parameters)
        {
            ValidateParameters(parameters);

	        //reset
	        await Task.FromResult(0);
            return new GenericApiResponse { Success = true, Message = "Successfully changed password!" };
        }
    }
}
