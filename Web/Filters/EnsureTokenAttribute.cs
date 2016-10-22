using System;
using System.Linq;
using GdShows.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Api;

namespace GdShows.Filters
{
    public class EnsureTokenAttribute : TypeFilterAttribute
    {
        public bool Skip { get; set; }
          
        public EnsureTokenAttribute() : base(typeof(EnsureTokenFilter))
        { 

        }

        private class EnsureTokenFilter : IResourceFilter
        {
            private readonly ISecurityTokenService _securityTokenService;
            private readonly CurrentUserContext _currentUserContext;

            public EnsureTokenFilter(ISecurityTokenService securityTokenService, CurrentUserContext currentUserContext)
            {
                _securityTokenService = securityTokenService;
                _currentUserContext = currentUserContext;
            }

            public void OnResourceExecuting(ResourceExecutingContext context)
            {
                var httpRequest = context.HttpContext.Request;
                var ensureTokenAttribute = context.ActionDescriptor.GetCustomAttributes<EnsureTokenAttribute>().FirstOrDefault();

                if(ensureTokenAttribute != null)
                {
                    if (ensureTokenAttribute.Skip)
                    {
                        return;
                    }
                }

                //try in header first, then qs
                if (!httpRequest.Headers.ContainsKey("securityToken") && httpRequest.Query.ContainsKey("securityToken"))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var tokenData = httpRequest.Headers["securityToken"];

                if (string.IsNullOrWhiteSpace(tokenData))
                {
                    httpRequest.Query.TryGetValue("securityToken", out tokenData);
                }

                if (string.IsNullOrEmpty(tokenData))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                try
                {
                    var token = _securityTokenService.Decrypt(tokenData);

                    if (token.Expires < DateTime.UtcNow)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                    _currentUserContext.Username = token.Username;
                    _currentUserContext.Id = token.UserId;
                    _currentUserContext.Role = token.Role;
                }
                catch
                {
                    context.Result = new UnauthorizedResult();
                }
            }

            public void OnResourceExecuted(ResourceExecutedContext context)
            {

            }
        }

    }
}
