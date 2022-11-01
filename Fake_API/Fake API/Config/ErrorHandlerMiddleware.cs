﻿using Fake_API.Exception;
using System.Net;

namespace Fake_API.Config;

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
        catch (System.Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (ex)
            {
                case FormatException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case ArgumentNullException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case UserNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case KeyNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
        }
    }


}