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
            byte[] msg = Encoding.UTF8.GetBytes(message.Content);
            return WebSocket.SendAsync(
                new ArraySegment<byte>(msg, 0, msg.Length),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
    }
}
