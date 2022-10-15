using System;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Services;
using Microsoft.AspNetCore.Http;

namespace ChatBot.Auth;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, UserService userService, JwtUtils jwtUtils)
    {
        //get token, split bearer
        var token = context.Request.Headers["Authentication"]
            .FirstOrDefault()?
            .Split(" ")
            .Last();
        
        //validate token    
        Guid? userId = jwtUtils.ValidateToken(token);
        
        //attach user to context on successful jwt validation
        //might break due to guid
        if(userId is not null)
            context.Items["User"] = userService.GetUserById(userId.Value); 
        
        await _next(context);
    }
}   

