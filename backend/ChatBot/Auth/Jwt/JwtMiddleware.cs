using ChatBot.Exceptions;
using ChatBot.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace ChatBot.Auth.Jwt;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils) //attaches userId to context
    {
        //get token, split bearer
        var token = context.Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Split(" ")
            .Last();

          //  await _next(context);
          //  throw new TokenNotFoundException("No token found");
        
        Guid? userId = jwtUtils.ValidateToken(token);
        
        if (userId is not null)
        {
            var user = userService.GetById(userId.Value);
            if(user is null)
                throw new AuthenticationException();
            context.Items["UserID"] = user.ID; //attaches userId to context
        }
        await _next(context);
    }
}

