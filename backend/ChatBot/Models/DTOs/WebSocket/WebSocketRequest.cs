using System.Text.Json.Serialization;

namespace ChatBot.Models.DTOs.WebSocket;

[JsonConverter(typeof(WebSocketRequestConverter))]
public class WebSocketRequest
{
    public string? Action { get; set; }
}
