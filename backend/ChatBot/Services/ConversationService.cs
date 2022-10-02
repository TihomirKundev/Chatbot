using ChatBot.Models;

namespace ChatBot.Services
{
    public class ConversationService
    {
        public Conversation CreateNew()
        {
            return new Conversation();
        }
        public bool AddMessage(Conversation c, Message m)
        {
            return c.AddMessage(m);
        }
        public bool RemoveMessage(Conversation c, Message m)
        {
            return c.RemoveMessage(m);
        }
    }
}
