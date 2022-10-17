using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Repositories.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace ChatBot.Repositories;

[Repository]
public class UserRepository : IUserRepository
{
    private readonly string _connString;

    public UserRepository(IDbConnection dbc)
    {
        _connString = dbc.GetConnectionString();
    }

    public void CreateUser(User user)
    {
        try
        {
            MySqlHelper.ExecuteNonQuery(
                _connString,
                "INSERT INTO users(id) " +
                "VALUES (@id);",
                new MySqlParameter("@id", user.ID));
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            throw new DuplicateNameException();
        }
    }


    public List<User> GetAllUsers()
    {
        var users = new List<User>();

        using var reader = MySqlHelper.ExecuteReader(
            _connString,
            "SELECT id FROM users WHERE id NOT IN (SELECT id FROM accounts)");

        if (!reader.HasRows)
            return users;

        while (reader.Read())
        {
            users.Add(new User(reader.GetGuid("id")));
        }
        return users;
    }

    public bool UserExists(Guid id)
    {
        object result = MySqlHelper.ExecuteScalar(
            _connString,
            "SELECT 1 FROM users WHERE id = @id",
            new MySqlParameter("@id", id));

        int status = Convert.ToInt32(result);

        return status == 1;
        
    }

}
