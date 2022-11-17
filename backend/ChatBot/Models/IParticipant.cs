using System;
using System.ComponentModel.DataAnnotations;

namespace ChatBot.Models
{
    public class Participant
    {
        [Key]
        public Guid ID { get; set; }

        public Participant()
        {

        }

        public Participant(Guid id)
        {
            ID = id;
        }
    }
}
