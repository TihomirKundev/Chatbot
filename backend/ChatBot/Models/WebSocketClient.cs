using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ChatBot.Models
{
    public class WebSocketClient
    {
        public WebSocketClient(Participant participant, WebSocket webSocket)
        {
            Participant = participant;
            WebSocket = webSocket;
        }
        
        public Participant Participant { get; }
        
        public WebSocket WebSocket { get; }
        
        public Guid? ConversationID { get; set; }
        
        public Task SendMessageAsync(string message)
        {
            byte[] msg = Encoding.UTF8.GetBytes(message);
            return WebSocket.SendAsync(
                new ArraySegment<byte>(msg, 0, msg.Length),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
    }
}
