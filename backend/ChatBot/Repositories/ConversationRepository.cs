using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Models.Enums;
using ChatBot.Repositories.Interfaces;
using ChatBot.Repositories.Utils;
using ChatBot.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ChatBot.Repositories;

[Repository]
public class ConversationRepository : IConversationRepository
{
    private readonly string _connString;
    private readonly IUserService _userService;

    public ConversationRepository(IConfiguration config, IUserService userService)
    {
        _connString = config.GetConnectionString("DefaultConnection");
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
        var messages = GetAllMessagesByConversationID(id);
        var participants = GetParticipantsByConversationID(id);

        return new Conversation(id, status, messages, participants);
    }

    public ISet<IParticipant> GetParticipantsByConversationID(Guid id)
    {
        var participants = new HashSet<IParticipant>();
        
        using var reader = SqlHelper.ExecuteReader(
            _connString,
            "SELECT user_id FROM conversations_users WHERE conversation_id = @id;",
            new SqlParameter("@id", id));

        if (!reader.HasRows)
            return participants;

        // TODO: refactor to get only required users
        var users = _userService.GetAllUsers().ToDictionary(u => u.ID, u => (IParticipant)u);
        users.Add(Bot.GetChatBotID(), new Bot());

        while(reader.Read())
        {
            if (!users.TryGetValue(reader.GetGuid("user_id"), out var user))
                continue;
            participants.Add(user);
        }
        
        return participants;
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

    public SortedSet<Message> GetAllMessagesByConversationID(Guid conversationID)
    {
        var messages = new SortedSet<Message>();

        using var connection = new SqlConnection(_connString);
        connection.Open();

        // Get all participants
        var participants = GetParticipantsByConversationID(conversationID);

        // Get all messages
        string getMessagesQuery = "SELECT id, author_id, content, timestamp FROM messages WHERE conversation_id = @id";
        using var reader2 = SqlHelper.ExecuteReader(
            connection,
            getMessagesQuery,
            new SqlParameter("@id", conversationID));
        while (reader2.Read())
        {
            var id = reader2.GetInt32("id");
            var authorID = reader2.GetGuid("author_id");
            var content = reader2.GetString("content");
            var timestamp = reader2.GetDateTime("timestamp");

            IParticipant? author = participants.FirstOrDefault(p => p.ID == authorID);
            if (author is null)
                continue;

            var message = new Message(id, author, content, timestamp);
            messages.Add(message);
        }
        reader2.Close();

        connection.Close();
        return messages;
    }

    public bool DeleteMessageById(long id)
    {
        return SqlHelper.ExecuteNonQuery(_connString,
            "DELETE FROM users WHERE id = @id",
            new SqlParameter("@id", id))
            > 0;
    }
}
