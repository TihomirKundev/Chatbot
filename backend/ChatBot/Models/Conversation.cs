﻿using ChatBot.Models.Enums;
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

    public Conversation(Guid id, ConversationStatus status, SortedSet<Message> messages, List<Participant> participants)
    {
        ID = id;
        Status = status;
        Messages = messages;
        Participants = participants;
    }

    public Conversation()
    {
        
    }

    public Guid ID { get; set; } = Guid.NewGuid();

    // Sorted by the timestamp of the messages
    // See Message.CompareTo() method for more info
    public SortedSet<Message> Messages
    {
        get => _messages;
        set => _messages = value;
    }

    public ConversationStatus Status { get;  set; }

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

    public List<Participant> Participants
    {
        get => _participants;
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
    private List<Participant> _participants = new List<Participant>();

    public void AddParticipant(Participant participant)
    {
        _participants.Add(participant);
    }

    public override bool Equals(object? obj) => obj is Conversation otherConversation && otherConversation.ID == ID;
    public override int GetHashCode() => ID.GetHashCode();
}
