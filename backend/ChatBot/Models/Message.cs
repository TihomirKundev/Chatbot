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

    public Message(long id, Participant author, string content, DateTime timestamp)
        : this(author, content, timestamp)
    {
        ID = id;
    }

    public long ID { get; }

    public Participant Author { get; }

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

    public int CompareTo(Message other)
    {
        return Timestamp.CompareTo(other.Timestamp);
    }

    private string _content = null!;
}
