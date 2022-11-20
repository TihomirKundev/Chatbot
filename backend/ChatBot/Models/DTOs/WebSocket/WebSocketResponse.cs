namespace ChatBot.Models.DTOs.WebSocket
{
    public class WebSocketResponse
    {
        protected WebSocketResponse(string type)
        {
            Type = type;
        }

        public string Type { get; }
    }
}
