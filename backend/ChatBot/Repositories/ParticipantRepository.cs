using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Repositories.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ChatBot.Repositories;

[Repository]
public class ParticipantRepository : IParticipantRepository
{
    private readonly string _connString;
    private readonly IAccountRepository _accountRepo;
    private readonly IUserRepository _userRepo;

    public ParticipantRepository(IDbConnection dbc, IAccountRepository accountRepo, IUserRepository userRepo)
    {
        _accountRepo = accountRepo;
        _userRepo = userRepo;
        _connString = dbc.GetConnectionString();
    }

    public IParticipant? GetParticipantByID(Guid id)
    {
        if (id == Bot.GetChatBotID())
            return new Bot();

        var account = _accountRepo.GetAccountByID(id);
        if (account is not null)
            return account;

        if (_userRepo.UserExists(id))
            return new User(id);

        return null;
    }

    public ISet<IParticipant> GetParticipantsByConversationID(Guid id)
    {
        var participants = new HashSet<IParticipant>();

        string getParticipantIDsQuery = "SELECT user_id FROM conversations_users WHERE conversation_id = @id";
        using var reader = MySqlHelper.ExecuteReader(
            _connString,
            getParticipantIDsQuery,
            new MySqlParameter("@id", id));

        while (reader.Read())
        {
            var participantID = reader.GetGuid("user_id");
            var participant = GetParticipantByID(participantID);
            if (participant is null)
                continue;
            participants.Add(participant);
        }
        reader.Close();

        return participants;
    }
}
