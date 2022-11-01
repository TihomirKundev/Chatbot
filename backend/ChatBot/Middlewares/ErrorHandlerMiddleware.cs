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
    private readonly ILogger _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<ErrorHandlerMiddleware>(); 
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (System.Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            
            switch (ex)
            {
                
                case AggregateException:
                case KeyNotFoundException:
                case TokenNotFoundException:
                case UserNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case FormatException:
                case ArgumentNullException:
                case DuplicateEmailException:
                case InvalidCredentialsException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            
            // var result = ex.Message;
            // await response.WriteAsync(result);
        }
    }


}