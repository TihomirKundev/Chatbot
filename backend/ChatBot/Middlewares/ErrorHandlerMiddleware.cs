using ChatBot.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ChatBot.Middlewares;
public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = ex switch
            {
                AggregateException or 
                KeyNotFoundException or 
                TokenNotFoundException or 
                UserNotFoundException 
                    => (int)HttpStatusCode.NotFound,
                    
                FormatException or
                ArgumentNullException or 
                DuplicateEmailException or 
                InvalidCredentialsException 
                    => (int)HttpStatusCode.BadRequest,
                    
                _ => (int)HttpStatusCode.InternalServerError,
            };

            var result = ex.Message;
            await response.WriteAsync(result);
        }
    }


}