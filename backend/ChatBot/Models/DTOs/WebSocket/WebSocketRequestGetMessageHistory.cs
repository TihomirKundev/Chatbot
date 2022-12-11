namespace ChatBot.Models.DTOs.WebSocket
{
    public class WebSocketRequestGetMessageHistory : WebSocketRequest
    {
        public string? Conversation { get; set; }
    }
}
