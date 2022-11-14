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

        //validate token    

        //attach user to context on successful jwt validation
        //might break due to guid
        Guid? userId = jwtUtils.ValidateToken(token);
        if (userId is not null)
        {
            var user = userService.GetById(userId.Value);

            if (user is not null)
                context.Items["UserID"] = user.ID; //dude on the internet is puting th whole user object here
            else
                throw new AuthenticationException();
        }

        await _next(context);
    }
}

