using System;

namespace ChatBot.Models
{
    public class Participant
    {
        public Guid ID { get; }

        public Participant()
        {

        }

        public Participant(Guid id)
        {
            ID = id;
        }
    }
}
