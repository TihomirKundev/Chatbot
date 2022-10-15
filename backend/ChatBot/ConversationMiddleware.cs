using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using ChatBot.Models;
using System.Collections.Generic;
using ChatBot.Models.DTOs;
using System.Linq;

namespace ChatBot
{
    public class ConversationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        
        public ConversationMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ConversationMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path != "/ws")
            {
                await _next(context);
                return;
            }
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = 404;
                return;
            }
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var wsClient = new WebSocketClient(new Anonymous(), webSocket);
            try
            {
                await HandleClient(wsClient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Echo websocket client {wsClient.Participant.ID} error.");
                await context.Response.WriteAsync("closed");
            }
        }

        private async Task HandleClient(WebSocketClient webSocket)
        {
            WebSocketClientCollection.Add(webSocket);
            _logger.LogInformation($"Websocket client added.");

            WebSocketReceiveResult result;
            do
            {
                var buffer = new byte[1024 * 1];
                result = await webSocket.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text && !result.CloseStatus.HasValue)
                {
                    var msgString = Encoding.UTF8.GetString(buffer);
                    //_logger.LogInformation($"Websocket client ReceiveAsync message {msgString}.");
                    var message = JsonConvert.DeserializeObject<MessageDTO>(msgString);
                    message.AuthorID = webSocket.Participant.ID;
                    HandleMessage(message);
                }
            }
            while (!result.CloseStatus.HasValue);
            WebSocketClientCollection.Remove(webSocket);
            _logger.LogInformation($"Websocket client disconnected.");
        }

        private void HandleMessage(MessageDTO message)
        {
            WebSocketClient? author = WebSocketClientCollection.Get(message.AuthorID);
            if (author is null)
            {
                _logger.LogError($"Author {message.AuthorID} not found.");
                return;
            }
            switch (message.Action)
            {
                case MessageAction.JOIN:
                    if (!Guid.TryParse(message.Content, out Guid convID))
                    {
                        _logger.LogError($"Invalid conversation ID '{message.Content}'.");
                        return;
                    }
                    if(author.ConversationID is not null)
                    {
                        if(author.ConversationID == convID)
                        {
                            _logger.LogWarning($"Client ID: '{author.Participant.ID}' is already in conversation ID: '{convID}'");
                            return;
                        }
                        _logger.LogInformation($"Client ID: '{author.Participant.ID}' switched to conversation ID: '{convID}'");
                    }
                    author.ConversationID = convID;
                    _logger.LogInformation($"Client '{author.Participant.ID} joined conversation with ID '{convID}'");
                    break;
                case MessageAction.SEND:
                    if(author.ConversationID is null)
                    {
                        _logger.LogError($"Client '{message.AuthorID}' is not in a conversation.");
                        return;
                    }
                    var clients = WebSocketClientCollection.GetByConversationID(author.ConversationID);
                    clients.ForEach(c => c.SendMessageAsync(message.Content));
                    _logger.LogInformation($"Websocket client ID: '{message.AuthorID}' sent a message: '{message.Content}'.");
                    break;
                case MessageAction.LEAVE:
                    _logger.LogInformation($"Websocket client '{message.AuthorID}' left the room '{author.ConversationID}'.");
                    author.ConversationID = null;
                    break;
                default:
                    return;
            }
        }
    }
}
