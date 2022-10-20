using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChatBot.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : System.Attribute
{
    //When a controller is decorated with the [Authorize] attribute all
    //action methods are restricted to authorized requests,
    //except for methods decorated with the custom [AllowAnonymous] attribute.

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //authorization is skipped if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata
            .OfType<AllowAnonymousAttribute>()
            .Any();
        if(allowAnonymous)
            return;
        
        //authorization
        var user = context.HttpContext.Items["User"];
        if (user is null)
            context.Result = 
                new JsonResult(
                    new { message = "Unauthorized" })
                    { StatusCode = StatusCodes.Status401Unauthorized };
    }
}