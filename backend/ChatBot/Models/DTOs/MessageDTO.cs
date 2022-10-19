using System;

namespace ChatBot.Models.DTOs;

public record MessageDTO
{
    public Guid AuthorID { get; set; }

    public string? Content { get; set; }

    public string? Nickname { get; set; }

    public long? Timestamp { get; set; }
    
    public MessageAction? Action { get; set; }

    public QuickSelector? QuickSelector { get; set; }
}
public enum MessageAction
{
    JOIN = 0,
    SEND = 1,
    LEAVE = 2
}

public enum QuickSelector
{
    faq = 0,
    ts = 1,
    order = 2 
}