using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatBot.Models;

public class Conversation
{
    public Conversation()
    {
        Status = ConversationStatus.ONGOING;
    }

    public Conversation(Guid id, ConversationStatus status)
    {
        ID = id;
        Status = status;
    }

    public Guid ID { get; }

    // Sorted by the timestamp of the messages
    // See Message.CompareTo() method for more info
    public SortedSet<Message> Messages => new(_messages);

    public ConversationStatus Status { get; private set; }

    public DateTime StartTime => _messages.First().Timestamp;

    // Returns null if the conversation is ongoing, otherwise returns last message's timestamp
    public DateTime? EndTime => Status == ConversationStatus.ONGOING ? null : _messages.Last().Timestamp;

    public HashSet<Participant> Participants => new(_participants);

    public bool AddMessage(Message message) => _messages.Add(message);

    public bool RemoveMessage(Message message) => _messages.Remove(message);

    public bool EndConversation(ConversationStatus status)
    {
        if (Status != ConversationStatus.ONGOING || status == ConversationStatus.ONGOING)
            return false;
        Status = status;
        return true;
    }

    private readonly SortedSet<Message> _messages = new();
    private readonly HashSet<Participant> _participants = new();
}
