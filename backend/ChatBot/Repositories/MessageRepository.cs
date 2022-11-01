using ChatBot.Extensions;
using ChatBot.Models;
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
public class MessageRepository : IMessageRepository
{
    private readonly string _connString;
    private readonly IUserService _userService;

    public MessageRepository(IConfiguration config, IUserService userService)
    {
        _connString = config.GetConnectionString("DefaultConnection");
        _userService = userService;
    }

    public SortedSet<Message> GetAllMessagesByConversationID(Guid conversationID)
    {
        var messages = new SortedSet<Message>();

        using var connection = new SqlConnection(_connString);
        connection.Open();

        // Get all participants
        var participants = _userService.GetParticipantsByConversationID(conversationID);

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
