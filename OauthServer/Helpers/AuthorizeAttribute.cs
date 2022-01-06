﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OauthServer.Helpers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (IdentityUser) context.HttpContext.Items["User"];
        if (user == null)
        {
            context.Result = new JsonResult(new {message = "Unauthorized"})
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}