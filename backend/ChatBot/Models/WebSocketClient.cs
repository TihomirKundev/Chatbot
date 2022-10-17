using ChatBot.Models.DTOs;
using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ChatBot.Models
{
    public class WebSocketClient
    {
        public WebSocketClient(IParticipant participant, WebSocket webSocket)
        {
            Participant = participant;
            WebSocket = webSocket;
        }

        public IParticipant Participant { get; }

        public WebSocket WebSocket { get; }

        public Guid? ConversationID { get; set; }

        public Task SendMessageAsync(MessageDTO message)
        {
            byte[] msg = JsonSerializer.SerializeToUtf8Bytes(message);
            return WebSocket.SendAsync(
                new ArraySegment<byte>(msg, 0, msg.Length),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
    }
}
