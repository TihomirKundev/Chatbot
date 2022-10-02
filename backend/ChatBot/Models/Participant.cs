using System;

namespace ChatBot.Models
{
    public abstract class Participant
    {
        public Guid ID { get; protected set; }
    }
}
