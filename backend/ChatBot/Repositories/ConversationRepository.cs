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
        try
        {
            SqlHelper.ExecuteNonQuery(
                _connString,
                "INSERT INTO messages(id, content, author_id, timestamp, conversation_id)" +
                "VALUES (@id, @content, @authorid, @timestamp, @conversation_id);",
                new SqlParameter("@id", message.ID),
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

        while(reader.Read())
        {
            if (!users.TryGetValue(Guid.Parse(reader.GetString("user_id")), out var user))
                continue;
            participants.Add(user);
        }

        participants.Add(new Bot());
        
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
            var id = Guid.Parse(reader2.GetString("id"));
            var authorID = Guid.Parse(reader2.GetString("author_id"));
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

    public IEnumerable<Conversation> GetConversationsByUser(User user)
    {
        using var reader = SqlHelper.ExecuteReader(
            _connString,
            "SELECT id FROM conversations INNER JOIN conversations_users ON id = conversation_id WHERE user_id = @user_id",
            new SqlParameter("@user_id", user.ID));

        while (reader.Read())
            yield return new Conversation(Guid.Parse(reader.GetString("id")));
    }

    public IEnumerable<Conversation> GetConversations()
    {
        var conversations = new Dictionary<Guid, Conversation>();

        using (var reader = SqlHelper.ExecuteReader(_connString, "SELECT id, status FROM conversations"))
            while (reader.Read())
                if (Guid.TryParse(reader.GetString("id"), out var conversationID))
                    conversations.Add(conversationID, new Conversation(conversationID, (ConversationStatus) reader.GetInt32("status"), new(), new HashSet<IParticipant>()));

        var users = _userService.GetAllUsers().ToDictionary(u => u.ID, u => u);

        using (var reader = SqlHelper.ExecuteReader(_connString, "SELECT conversation_id, user_id FROM conversations_users"))
            while (reader.Read())
                if (Guid.TryParse(reader.GetString("conversation_id"), out var conversationID) && Guid.TryParse(reader.GetString("user_id"), out var userID) && conversations.TryGetValue(conversationID, out var conversation) && users.TryGetValue(userID, out var user))
                    conversation.AddParticipant(user);

        using (var reader = SqlHelper.ExecuteReader(_connString, "SELECT * FROM (SELECT id, author_id, content, timestamp, conversation_id, ROW_NUMBER() OVER (PARTITION BY conversation_id ORDER BY timestamp DESC) timestamp_rank FROM messages) _ WHERE timestamp_rank = 1"))
            while (reader.Read())
                if (Guid.TryParse(reader.GetString("id"), out var ID) && Guid.TryParse(reader.GetString("conversation_id"), out var conversationID) && Guid.TryParse(reader.GetString("author_id"), out var userID) && conversations.TryGetValue(conversationID, out var conversation) && users.TryGetValue(userID, out var user))
                    conversation.AddMessage(new Message(ID, user, reader.GetString("content"), reader.GetDateTime("timestamp")));

        return conversations.Values;
    }
}
