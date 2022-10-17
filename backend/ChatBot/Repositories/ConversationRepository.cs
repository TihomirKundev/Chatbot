using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Models.Enums;
using ChatBot.Repositories.Interfaces;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace ChatBot.Repositories;

[Repository]
public class ConversationRepository : IConversationRepository
{
    private readonly string _connString;
    private readonly IMessageRepository _messageRepo;
    private readonly IParticipantRepository _participantRepo;

    public ConversationRepository(IDbConnection dbc, IMessageRepository messageRepo, IParticipantRepository participantRepo)
    {
        _connString = dbc.GetConnectionString();
        _messageRepo = messageRepo;
        _participantRepo = participantRepo;
    }

    public void SaveMessageToConversation(Message message, Guid conversationId)
    {
        if (message.ID is null)
        {
            try
            {
                message.ID = MySqlHelper.ExecuteNonQuery(
                    _connString,
                    "INSERT INTO messages(content, author_id, timestamp, conversation_id)" +
                    "VALUES (@content, @authorid, @timestamp, @conversation_id);",
                    new MySqlParameter("@content", message.Content),
                    new MySqlParameter("@authorid", message.Author.ID),
                    new MySqlParameter("@timestamp", message.Timestamp),
                    new MySqlParameter("@conversation_id", conversationId));
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                throw new DuplicateNameException();
            }
        }
        else
        {
            MySqlHelper.ExecuteNonQuery(
                _connString,
                "UPDATE messages SET " +
                    "content = @content " +
                    "author_id = @authorId " +
                    "timestamp = @timestamp " +
                "WHERE id = @id;",
                new MySqlParameter("@id", message.ID),
                new MySqlParameter("@content", message.Content),
                new MySqlParameter("@authorId", message.Author.ID),
                new MySqlParameter("@timestamp", message.Timestamp));
        }
    }

    public void AddParticipantToConversation(Guid participantID, Guid conversationID)
    {
        MySqlHelper.ExecuteNonQuery(
            _connString,
            "INSERT INTO conversations_users(conversation_id, user_id) VALUES (@conversation_id, @user_id)",
            new MySqlParameter("@conversation_id", conversationID),
            new MySqlParameter("@user_id", participantID));
    }

    public Conversation? GetConversationByID(Guid id)
    {
        using var reader = MySqlHelper.ExecuteReader(
            _connString,
            "SELECT status FROM conversations WHERE id = @id;",
            new MySqlParameter("@id", id));

        if (!reader.HasRows)
            return null;

        var status = Enum.Parse<ConversationStatus>(reader.GetString("status"), true);
        var messages = _messageRepo.GetAllMessagesByConversationID(id);
        var participants = _participantRepo.GetParticipantsByConversationID(id);

        return new Conversation(id, status, messages, participants);
    }

    public void CreateConversation(Conversation conversation)
    {
        try
        {
            MySqlHelper.ExecuteNonQuery(
            _connString,
            "INSERT INTO conversations(id, status) VALUES (@id, @status) ",
            new MySqlParameter("@id", conversation.ID),
            new MySqlParameter("@status", conversation.Status));
        }
        catch (MySqlException ex) when (ex.ErrorCode == 1062)
        {
            throw new DuplicateNameException($"Duplicate conversation ID: {conversation.ID}");
        }
    }

    public void SetConversationStatus(Guid id, ConversationStatus status)
    {
            MySqlHelper.ExecuteNonQuery(
            _connString,
            "UPDATE conversations SET status = @status WHERE id = @id",
            new MySqlParameter("@id", id),
            new MySqlParameter("@status", status));
    }
}
