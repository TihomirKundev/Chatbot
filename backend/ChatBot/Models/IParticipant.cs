using System;
using System.ComponentModel.DataAnnotations;

namespace ChatBot.Models
{
    public class Participant
    {
        
        public  Guid ID { get; set; }
        
        [Key]
        public Guid EFID { get; set; }
        
        public Participant()
        {

        }

        public Participant(Guid id)
        {
            ID = id;
        }
    }
}
