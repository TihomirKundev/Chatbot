using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Repositories.Interfaces;
using ChatBot.Services.Interfaces;
using System;

namespace ChatBot.Services;

[TransientService]

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepo;

    private readonly IUserService _userService;

    public MessageService(
        IMessageRepository messageRepository,
        IUserService userService)
    {
        _messageRepo = messageRepository;
        _userService = userService;
    }


    public Message CreateMessage(MessageDTO dto)
    {
        if (dto.Timestamp is null)
            throw new InvalidMessageException("Timestamp cannot be null.");

        if (string.IsNullOrWhiteSpace(dto.Content))
            throw new InvalidMessageException("Message content cannot be empty.");

        IParticipant? author = _userService.GetById(dto.AuthorID);

        if (author is null)
            throw new InvalidMessageException($"No participant exists with such ID: '{dto.AuthorID}'");

        if (dto.Timestamp is null)
        {
            throw new InvalidMessageException("No timestamp provided.");
        }

        DateTime timestamp;
        timestamp = UnixTimeStampToDateTime(dto.Timestamp.Value);

        return new Message(author, dto.Content, timestamp);
    }

    public bool DeleteMessageById(long id)
    {
        return _messageRepo.DeleteMessageById(id);
    }

    private static DateTime UnixTimeStampToDateTime(long unixTimestamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddMilliseconds(unixTimestamp).ToLocalTime();
        return dateTime;
    }

    public class InvalidMessageException : Exception
    {
        public InvalidMessageException() : base() { }

        public InvalidMessageException(string message) : base(message) { }
    }
}