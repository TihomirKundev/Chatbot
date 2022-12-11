namespace ChatBot.Models.DTOs.WebSocket
{
    public class WebSocketRequestSendMessage : WebSocketRequest
    {
        public string? Content { get; set; }
        public string? Conversation { get; set; }
        public string? QuickSelector { get; set; }
    }
}
