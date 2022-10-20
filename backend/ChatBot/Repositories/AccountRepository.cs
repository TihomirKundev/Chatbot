using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Models.Enums;
using ChatBot.Repositories.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace ChatBot.Repositories;

[Repository]
public class AccountRepository : IAccountRepository
{
    private readonly string _connString;

    public AccountRepository(IDbConnection dbc)
    {
        _connString = dbc.GetConnectionString();
    }

    public void SaveAccount(Account account)
    {
        try
        {
            string query =
                "IF EXISTS (SELECT 1 FROM accounts WHERE id = @id) " +
                "BEGIN " +
                    "UPDATE accounts SET " +
                        "firstname = @firstname " +
                        "lastname = @lastname " +
                        "email = @email " +
                        "phone = @phone " +
                        "password = @password" +
                        "role = @role " +
                    "WHERE id = @id " +
                "END " +
                "ELSE " +
                    "INSERT INTO accounts(id, firstname, lastname, email, phone, password, role) " +
                    "VALUES (@id, @firstname, @lastname, @email, @phone, @password, @role) " +
                "END";

            MySqlHelper.ExecuteNonQuery(
                _connString,
                query,
                new MySqlParameter("@id", account.ID),
                new MySqlParameter("@firstname", account.FirstName),
                new MySqlParameter("@lastname", account.LastName),
                new MySqlParameter("@email", account.Email),
                new MySqlParameter("@phone", account.Phone),
                new MySqlParameter("@password", account.Password),
                new MySqlParameter("@role", account.Role));
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            throw new DuplicateNameException();
        }
    }

    public Account GetByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public List<Account> GetAllAccounts()
    {
        var accounts = new List<Account>();

        using var reader = MySqlHelper.ExecuteReader(
            _connString,
            "SELECT id, firstname, lastname, email, phone, password, role FROM accounts");

        if (!reader.HasRows)
            return accounts;

        while (reader.Read())
        {
            accounts.Add(new Account(
                reader.GetGuid("id"),
                reader.GetString("firstname"),
                reader.GetString("lastname"),
                reader.GetString("email"),
                reader.GetString("phone"),
                reader.GetString("password"),
                Enum.Parse<Role>(reader.GetString("role"), true)
            ));
        }
        return accounts;
    }

    public Account? GetAccountByID(Guid id)
    {
        using var reader = MySqlHelper.ExecuteReader(
            _connString,
            "SELECT firstname, lastname, email, phone, password, role FROM accounts WHERE id = @id",
            new MySqlParameter("@id", id));

        if (!reader.Read())
            return null;

        return new Account(
            reader.GetGuid("id"),
            reader.GetString("firstname"),
            reader.GetString("lastname"),
            reader.GetString("email"),
            reader.GetString("phone"),
            reader.GetString("password"),
            Enum.Parse<Role>(reader.GetString("role"), true)
        );
    }
}
