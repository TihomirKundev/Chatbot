using ChatBot.Extensions;
using System;

namespace ChatBot.Models.DTOs.WebSocket
{
    public class WebSocketResponseChatMessage : WebSocketResponse
    {
        public WebSocketResponseChatMessage(Conversation conversation, Message message) : base("chatMessage")
        {
            if (message.ID is not { } id)
                throw new ArgumentException("The given message must have an ID");

            ID = id.ToString();
            Conversation = conversation.ID.ToString();
            Author = message.Author.ID.ToString();
            Content = message.Content;
            Timestamp = message.Timestamp;
        }

        public WebSocketResponseChatMessage(Conversation conversation, MessageDTO message) : base("chatMessage")
        {
            if (message.ID is not { } id)
                throw new ArgumentException("The given message must have an ID");

            ID = id.ToString();
            Conversation = conversation.ID.ToString();
            Author = message.AuthorID.ToString();
            Content = message.Content ?? "";
            Timestamp = message.Timestamp is { } timestamp ? DateExtensions.FromUnixTimestamp(timestamp) : DateTime.Now;
        }

        public string ID { get; }
        public string Conversation { get; }
        public string Author { get; }
        public string Content { get; }
        public DateTime Timestamp { get; }
    }
}
