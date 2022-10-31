﻿using ChatBot.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Auth.Jwt;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, AccountService accountService, JwtUtils jwtUtils)
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
        if (userId is not null)
            context.Items["User"] = accountService.GetById(userId.Value);

        await _next(context);
    }
}

