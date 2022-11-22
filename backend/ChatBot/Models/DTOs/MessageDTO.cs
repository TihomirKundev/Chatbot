using System;

namespace ChatBot.Models.DTOs;

public record MessageDTO
{
    public Guid? ID { get; set; }

    public Guid AuthorID { get; set; }

    public string? Content { get; set; }

    public string? Nickname { get; set; }

    public long? Timestamp { get; set; }

    public MessageAction Action { get; set; } = MessageAction.SEND;

    public QuickSelector QuickSelector { get; set; } = QuickSelector.CustomerSupport;
}
public enum MessageAction
{
    JOIN = 0,
    SEND = 1,
    LEAVE = 2
}

public enum QuickSelector
{
    CustomerSupport,
    Faq,
    Order
}