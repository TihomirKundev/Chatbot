using System.Collections.Generic;

namespace ChatBot.Models.DTOs.WebSocket
{
    public class WebSocketResponseConversation : WebSocketResponse
    {
        public WebSocketResponseConversation(Conversation conversation) : base("conversation")
        {
            ID = conversation.ID.ToString();
            Status = conversation.Status.ToString().ToLowerInvariant();

            var participants = conversation.Participants;
            var i = 0;
            Participants = new string[participants.Count];

            foreach (var participant in participants)
                Participants[i++] = participant.ID.ToString();
        }

        public string ID { get; }
        public string Status { get; }
        public string[] Participants { get; }
    }
}
