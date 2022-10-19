using System;

namespace ChatBot.Models
{
    public class User : IParticipant
    {
        public Guid ID { get; set; }

        public User()
        {
            ID = Guid.NewGuid();
        }

        public User(Guid guid)
        {
            ID = guid;
        }
    }
}
