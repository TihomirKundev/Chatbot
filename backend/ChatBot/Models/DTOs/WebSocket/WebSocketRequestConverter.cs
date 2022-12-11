using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatBot.Models.DTOs.WebSocket
{
    /// <summary>
    /// The client on the websocket can send multiple kinds of messages.
    /// To deal with this using polymorphism, this class will check the "action" property to construct the correct subtype.
    /// </summary>
    public class WebSocketRequestConverter : JsonConverter<WebSocketRequest>
    {
        private static readonly JsonSerializerOptions DEFAULT_OPTIONS = new() {
            PropertyNameCaseInsensitive = true
        };

        // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?pivots=dotnet-6-0#an-alternative-way-to-do-polymorphic-deserialization
        public override WebSocketRequest? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader readerClone = reader;

            if (readerClone.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            readerClone.Read();
            if (readerClone.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string? propertyName = readerClone.GetString();
            if (propertyName != "action")
                throw new JsonException();

            readerClone.Read();
            if (readerClone.TokenType != JsonTokenType.String)
                throw new JsonException();

            string actionName = readerClone.GetString()!;
            WebSocketRequest message = actionName switch {
                "sendChatMessage" => JsonSerializer.Deserialize<WebSocketRequestSendMessage>(ref reader, DEFAULT_OPTIONS)!,
                "newConversation" => JsonSerializer.Deserialize<WebSocketRequestNewConversation>(ref reader, DEFAULT_OPTIONS)!,
                "getMessageHistory" => JsonSerializer.Deserialize<WebSocketRequestGetMessageHistory>(ref reader, DEFAULT_OPTIONS)!,
                "getConversations" => JsonSerializer.Deserialize<WebSocketRequestGetConversations>(ref reader, DEFAULT_OPTIONS)!,
                "getUserInfo" => JsonSerializer.Deserialize<WebSocketRequestGetUserInfo>(ref reader, DEFAULT_OPTIONS)!,
                _ => throw new JsonException()
            };

            return message;
        }

        public override void Write(Utf8JsonWriter writer, WebSocketRequest value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
