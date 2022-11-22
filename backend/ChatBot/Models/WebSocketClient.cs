using ChatBot.Models.DTOs;
using ChatBot.Models.DTOs.WebSocket;
using ChatBot.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ChatBot.Models
{
    public class WebSocketClient
    {
        private const int RECEIVE_BUFFER_SIZE = 1024;

        public WebSocketClient(User user)
        {
            User = user;
        }

        public event EventHandler<WebSocketRequest>? MessageReceived;
        public User User { get; }

        public bool IsPrivyToConversation(Conversation conversation)
        {
            return User.Role == Role.ADMIN || conversation.Participants.Contains(User);
        }

        private readonly ICollection<WebSocket> _webSockets = new List<WebSocket>();
        public async Task<int> Listen(WebSocket socket)
        {
            WebSocketReceiveResult result;
            var buffer = new byte[RECEIVE_BUFFER_SIZE];

            _webSockets.Add(socket);

            do
            {
                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                    try
                    {
                        var messageJSON = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var message = JsonSerializer.Deserialize<WebSocketRequest>(messageJSON, new JsonSerializerOptions {
                            PropertyNameCaseInsensitive = true
                        })!;
                        MessageReceived?.Invoke(this, message);
                    }
                    catch (JsonException ex)
                    {
                        Debug.WriteLine("WebSockets JSON conversion failed: " + ex.Message);
                    }
            }
            while (!result.CloseStatus.HasValue);

            _webSockets.Remove(socket);

            return _webSockets.Count;
        }

        private HashSet<string> alreadyKnownUsers = new();
        public Task SendMessageAsync(WebSocketResponse message)
        {
            if (message is WebSocketResponseUser userMessage && !alreadyKnownUsers.Add(userMessage.ID))
                return Task.CompletedTask;

            byte[] messageText = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message, message.GetType(), new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
            Task[] sendTasks = new Task[_webSockets.Count];
            int i = 0;

            foreach (var webSocket in _webSockets)
                sendTasks[i++] = webSocket.SendAsync(
                    new ArraySegment<byte>(messageText),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);

            return Task.WhenAll(sendTasks);
        }
    }
}
