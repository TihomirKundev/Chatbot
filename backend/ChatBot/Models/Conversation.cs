using ChatBot.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatBot.Models;

public class Conversation
{
    public Conversation(Guid id)
    {
        ID = id;
        Status = ConversationStatus.ONGOING;
    }

    public Conversation(Guid id, ConversationStatus status, SortedSet<Message> messages, ISet<IParticipant> participants)
    {
        ID = id;
        Status = status;
        Messages = messages;
        Participants = participants;
    }

    public Guid ID { get; } = Guid.NewGuid();

    // Sorted by the timestamp of the messages
    // See Message.CompareTo() method for more info
    public SortedSet<Message> Messages
    {
        get => new(_messages);
        set => _messages = value;
    }

    public ConversationStatus Status { get; private set; }

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

    public ISet<IParticipant> Participants
    {
        get => new HashSet<IParticipant>(_participants);
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
    private ISet<IParticipant> _participants = new HashSet<IParticipant>();

    public void AddParticipant(IParticipant participant)
    {
        _participants.Add(participant);
    }

    public override bool Equals(object? obj) => obj is Conversation otherConversation && otherConversation.ID == ID;
    public override int GetHashCode() => ID.GetHashCode();
}
