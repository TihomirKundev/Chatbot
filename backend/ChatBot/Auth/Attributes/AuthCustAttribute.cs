
using System;
using ChatBot.Models;
using Microsoft.AspNetCore.Http;

namespace ChatBot.Auth.Attributes;


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        object user = context.HttpContext.Items["UserID"];
        if (user is null)
        {
            context.Result = new JsonResult(new { message = "Unauthorizeeed" }) { StatusCode = StatusCodes.Status401Unauthorized };
          
        }
    }
}