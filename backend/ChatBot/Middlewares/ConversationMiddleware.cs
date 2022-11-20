using ChatBot.Models;
using ChatBot.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatBot.Middlewares
{
    public class ConversationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketService _webSocketService;
        private readonly IAuthService _authService;

        public ConversationMiddleware(
            RequestDelegate next,
            IWebSocketService webSocketService,
            IAuthService authService)
        {
            _next = next;
            _webSocketService = webSocketService;
            _authService = authService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path != "/ws")
            {
                await _next(context);
                return;
            }
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = StatusCodes.Status426UpgradeRequired;
                return;
            }

            var webSocket = await context.WebSockets.AcceptWebSocketAsync();

            var tokenBuffer = new byte[256];
            var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(tokenBuffer), CancellationToken.None);
            if (receiveResult.MessageType == WebSocketMessageType.Text && _authService.CheckToken(Encoding.UTF8.GetString(tokenBuffer, 0, receiveResult.Count)) is User user)
                await _webSocketService.HandleConnection(user, webSocket);
            else
                await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Authentication failed", CancellationToken.None);
        }
    }
}
