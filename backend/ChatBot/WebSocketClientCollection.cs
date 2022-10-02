using ChatBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatBot
{
    public static class WebSocketClientCollection
    {
        private static readonly Dictionary<Guid, WebSocketClient> _clients = new();

        public static List<WebSocketClient> GetAll() => new(_clients.Values);

        public static WebSocketClient? Get(Guid id) => _clients.TryGetValue(id, out var client) ? client : null;
        
        public static List<WebSocketClient> GetByConversationID(Guid? conversationId)
        {
            if (conversationId is null)
                return new();
            return _clients
                .Values
                .Where(c => c.ConversationID == conversationId)
                .ToList();
        }
        
        public static bool Add(WebSocketClient client)
        {
            if (!_clients.TryGetValue(client.Participant.ID, out _))
            {
                _clients.Add(client.Participant.ID, client);
                return true;
            }
            return false;
        }
        public static bool Remove(WebSocketClient client)
        {
            return _clients.Remove(client.Participant.ID);
        }
    }
}
