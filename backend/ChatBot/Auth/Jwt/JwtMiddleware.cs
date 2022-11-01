using ChatBot.Auth.Jwt.Impl;
using ChatBot.Services;
using ChatBot.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using ChatBot.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace ChatBot.Auth.Jwt;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        //get token, split bearer
        var token = context.Request.Headers["Authentication"]
            .FirstOrDefault()?
            .Split(" ")
            .Last();

        if (token is null)
            throw new TokenNotFoundException("No token found");
 
        //validate token    
        Guid? userId = jwtUtils.ValidateToken(token);

        //attach user to context on successful jwt validation
        //might break due to guid
        if (userId is not null)
        {
            var user = userService.GetById(userId.Value);

            if (user is not null)
                context.Items["UserID"] = user.ID;
            else
                throw new AuthenticationException();
        }

        await _next(context);
    }
}

