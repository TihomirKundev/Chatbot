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

    private readonly IUserService _userService;
    private readonly IConversationRepository _conversationRepo;

    public MessageService(
        IUserService userService, 
        IConversationRepository conversationRepo)
    {
        _userService = userService;
        _conversationRepo = conversationRepo;
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
            throw new InvalidMessageException("No timestamp provided.");

        DateTime timestamp;
        timestamp = DateExtensions.FromUnixTimestamp(dto.Timestamp.Value);

        return new Message(dto.ID ?? Guid.NewGuid(), author, dto.Content, timestamp);
    }

    public bool DeleteMessageById(long id)
    {
        return _conversationRepo.DeleteMessageById(id);
    }

    public class InvalidMessageException : Exception
    {
        public InvalidMessageException() : base() { }

        public InvalidMessageException(string message) : base(message) { }
    }
}