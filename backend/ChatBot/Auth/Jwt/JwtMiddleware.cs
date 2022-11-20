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
    private readonly IAuthService _authService;

    public JwtMiddleware(RequestDelegate next, IAuthService authService)
    {
        _next = next;
        _authService = authService;
    }

    public async Task Invoke(HttpContext context) //attaches userId to context
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var authorization))
        {
            if (authorization[0].StartsWith("Bearer ") && _authService.CheckToken(authorization[0][7..]) is {} user)
                context.Items["User"] = user;
            else throw new AuthenticationException();
        }

        await _next(context);
    }
}

