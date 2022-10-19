using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Repositories.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatBot.Repositories;

[Repository]

public class MessageRepository : IMessageRepository
{
    private readonly string _connString;
    private readonly IParticipantRepository _participantRepo;

    public MessageRepository(IDbConnection dbc, IParticipantRepository participantRepo)
    {
        _connString = dbc.GetConnectionString();
        _participantRepo = participantRepo;
    }

    public SortedSet<Message> GetAllMessagesByConversationID(Guid conversationID)
    {
        var messages = new SortedSet<Message>();

        using var connection = new MySqlConnection(_connString);
        connection.Open();

        // Get all participants
        var participants = _participantRepo.GetParticipantsByConversationID(conversationID);

        // Get all messages
        string getMessagesQuery = "SELECT id, author_id, content, timestamp FROM messages WHERE conversation_id = @id";
        using var reader2 = MySqlHelper.ExecuteReader(
            connection,
            getMessagesQuery,
            new MySqlParameter("@id", conversationID));
        while (reader2.Read())
        {
            var id = reader2.GetInt64("id");
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
        return MySqlHelper.ExecuteNonQuery(_connString,
            "DELETE FROM users WHERE id = @id",
            new MySqlParameter("@id", id))
            > 0;
    }
}
