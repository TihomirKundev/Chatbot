using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Models.Enums;
using ChatBot.Repositories.Interfaces;
using ChatBot.Repositories.Utils;
using ChatBot.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace ChatBot.Repositories;

[Repository]
public class ConversationRepository : IConversationRepository
{
    private readonly string _connString;
    private readonly IMessageRepository _messageRepo;
    private readonly IUserService _userService;

    public ConversationRepository(IDbConnection dbc, IMessageRepository messageRepo, IUserService userService)
    {
        _connString = dbc.GetConnectionString();
        _messageRepo = messageRepo;
        _userService = userService;
    }

    public void SaveMessageToConversation(Message message, Guid conversationId)
    {
        if (message.ID is null)
        {
            try
            {
                message.ID = SqlHelper.ExecuteNonQuery(
                    _connString,
                    "INSERT INTO messages(content, author_id, timestamp, conversation_id)" +
                    "VALUES (@content, @authorid, @timestamp, @conversation_id);",
                    new SqlParameter("@content", message.Content),
                    new SqlParameter("@authorid", message.Author.ID),
                    new SqlParameter("@timestamp", message.Timestamp),
                    new SqlParameter("@conversation_id", conversationId));
            }
            catch (SqlException ex) when (ex.Number == 1062)
            {
                throw new DuplicateNameException();
            }
        }
        else
        {
            SqlHelper.ExecuteNonQuery(
                _connString,
                "UPDATE messages SET  content = @content  author_id = @authorId timestamp = @timestamp WHERE id = @id;",
                new SqlParameter("@id", message.ID),
                new SqlParameter("@content", message.Content),
                new SqlParameter("@authorId", message.Author.ID),
                new SqlParameter("@timestamp", message.Timestamp));
        }
    }

    public void AddParticipantToConversation(Guid participantID, Guid conversationID)
    {
        SqlHelper.ExecuteNonQuery(
            _connString,
            "INSERT INTO conversations_users(conversation_id, user_id) VALUES (@conversation_id, @user_id)",
            new SqlParameter("@conversation_id", conversationID),
            new SqlParameter("@user_id", participantID));
    }

    public Conversation? GetConversationByID(Guid id)
    {
        using var reader = SqlHelper.ExecuteReader(
            _connString,
            "SELECT status FROM conversations WHERE id = @id;",
            new SqlParameter("@id", id));

        if (!reader.HasRows)
            return null;
        
        reader.Read();
        var status = (ConversationStatus)reader.GetInt32("status");
        var messages = _messageRepo.GetAllMessagesByConversationID(id);
        var participants = _userService.GetParticipantsByConversationID(id);

        return new Conversation(id, status, messages, participants);
    }

    public void CreateConversation(Conversation conversation)
    {
        try
        {
            SqlHelper.ExecuteNonQuery(
            _connString,
            "INSERT INTO conversations(id, status) VALUES (@id, @status) ",
            new SqlParameter("@id", conversation.ID),
            new SqlParameter("@status", conversation.Status));
        }
        catch (SqlException ex) when (ex.ErrorCode == 1062)
        {
            throw new DuplicateNameException($"Duplicate conversation ID: {conversation.ID}");
        }
    }

    public void SetConversationStatus(Guid id, ConversationStatus status)
    {
        SqlHelper.ExecuteNonQuery(
        _connString,
        "UPDATE conversations SET status = @status WHERE id = @id",
        new SqlParameter("@id", id),
        new SqlParameter("@status", status));
    }
}
