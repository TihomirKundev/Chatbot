using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace ChatBot.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (SocketException ex)
        {
            _logger.LogError(ex, $"Could not establish connection to the database: {ex}");
            await HandleDbExceptionAsync(httpContext);
        }
    }
    private async Task HandleDbExceptionAsync(HttpContext context)
    {
        if (!context.Response.HasStarted)
        {
            await context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Internal Server Error."
            }.ToString());
        }
        else
        {
            await context.Response.WriteAsync(string.Empty);
        }
    }
    
    private record ErrorDetails
    {
        public int StatusCode { get; set; } = 500;

        public string Message { get; set; } = "";
    }
}