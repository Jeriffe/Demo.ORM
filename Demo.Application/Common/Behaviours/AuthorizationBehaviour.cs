using Demo.Application.Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    {
        //private readonly IUser _user;
        //private readonly IIdentityService _identityService;

        //public AuthorizationBehaviour( IUser user,IIdentityService identityService)
        //{
        //    _user = user;
        //    _identityService = identityService;
        //}

        public AuthorizationBehaviour()
        {
        }
        public async Task<TResponse> Handle(TRequest request,  CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            //var authorizeAttributes=new List<string>();
            //if (authorizeAttributes.Any())
            //{
            //    // Must be authenticated user
            //    if (_user.Id == null)
            //    {
            //        throw new UnauthorizedAccessException();
            //    }

            //    // Role-based authorization
            //    var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

            //    if (authorizeAttributesWithRoles.Any())
            //    {
            //        var authorized = false;

            //        foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
            //        {
            //            foreach (var role in roles)
            //            {
            //                var isInRole = await _identityService.IsInRoleAsync(_user.Id, role.Trim());
            //                if (isInRole)
            //                {
            //                    authorized = true;
            //                    break;
            //                }
            //            }
            //        }

            //        // Must be a member of at least one role in roles
            //        if (!authorized)
            //        {
            //            throw new ForbiddenAccessException();
            //        }
            //    }

            //    // Policy-based authorization
            //    var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
            //    if (authorizeAttributesWithPolicies.Any())
            //    {
            //        foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
            //        {
            //            var authorized = await _identityService.AuthorizeAsync(_user.Id, policy);

            //            if (!authorized)
            //            {
            //                throw new ForbiddenAccessException();
            //            }
            //        }
            //    }
            //}

            // User is authorized / authorization not required
            return await next();
        }
    }
}