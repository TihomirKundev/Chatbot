using ChatBot.Models.DTOs;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatBot.Models
{
    public class WebSocketClient
    {
        public WebSocketClient(Guid id, WebSocket webSocket)
        {
            ID = id;
            WebSocket = webSocket;
        }

        public Guid ID { get; }

        public WebSocket WebSocket { get; }

        public Guid? ConversationID { get; set; }

        public Task SendMessageAsync(MessageDTO message)
        {
            byte[] msg = Encoding.UTF8.GetBytes(message.Content);
            return WebSocket.SendAsync(
                new ArraySegment<byte>(msg, 0, msg.Length),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
    }
}
