using System;

namespace ChatBot.Models;

public class Message : IComparable<Message>
{
    public Message(Participant author, string content, DateTime timestamp)
    {
        Author = author;
        Content = content;
        Timestamp = timestamp;
    }

    public Message(Guid id, Participant author, string content, DateTime timestamp)
        : this(author, content, timestamp)
    {
        ID = id;
    }

    public Message()
    {
        
    }
    
    public Guid? ID { get; set; } = null;

    public Participant Author { get; set; }

    public string Content
    {
        get => _content;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(Content));
            _content = value;
        }
    }

    public DateTime Timestamp { get; }

    public int CompareTo(Message? other)
    {
        if (other is null)
            return 0;
        return Timestamp.CompareTo(other.Timestamp);
    }

    private string _content = default!;

	public override bool Equals(object? obj) => obj is Message otherMessage && otherMessage.ID == ID;
	public override int GetHashCode() => ID.GetHashCode();
}
