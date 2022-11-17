using ChatBot.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ChatBot.Models;

public class Conversation
{
    public Conversation() { }
    public Conversation(Guid id)
    {
        ID = id;
        Status = ConversationStatus.ONGOING;
    }

    public Conversation(Guid id, ConversationStatus status, SortedSet<Message> messages, ISet<Participant> participants)
    {
        ID = id;
        Status = status;
        Messages = messages;
        Participants = participants;
    }
    [Key]
    public Guid ID { get; set; } = Guid.NewGuid();

    // Sorted by the timestamp of the messages
    // See Message.CompareTo() method for more info
    public SortedSet<Message> Messages
    {
        get => new(_messages);
        set => _messages = value;
    }

    public ConversationStatus Status { get;  set; }  //entity framework needs a setter, idk we can convert to records and crete model motators later

    public DateTime StartTime => _messages.First().Timestamp;

    public DateTime? EndTime
    {
        get
        {
            if (Status == ConversationStatus.ONGOING)
                return null;
            else
                return _messages.Last().Timestamp;
        }
    }

    public ISet<Participant> Participants
    {
        get => new HashSet<Participant>(_participants);
        set => _participants = value;
    }
    public bool AddMessage(Message message) => _messages.Add(message);

    public bool RemoveMessage(Message message) => _messages.Remove(message);

    public bool EndConversation(ConversationStatus status)
    {
        if (Status != ConversationStatus.ONGOING || status == ConversationStatus.ONGOING)
            return false;

        Status = status;
        return true;
    }

    private SortedSet<Message> _messages = new();
    private ISet<Participant> _participants = new HashSet<Participant>();
}
    