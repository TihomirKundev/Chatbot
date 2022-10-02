using System;

namespace ChatBot.Models
{
    public class Anonymous : Participant
    {
        public Anonymous()
        {
            ID = Guid.NewGuid();
        }

        public Anonymous(Guid guid)
        {
            ID = guid;
        }
    }
}
