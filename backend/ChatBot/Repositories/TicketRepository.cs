using System;
using System.Collections.Generic;
using ChatBot.Extensions;
using ChatBot.Models.DTOs;
using ChatBot.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Data;
using ChatBot.Repositories.Utils;

namespace ChatBot.Repositories;

[Repository]
public class TicketRepository : ITicketRepository
{
    private readonly string _connString;

    public TicketRepository(IDbConnection dbc)
    {
        _connString = dbc.GetConnectionString();
    }

    public void SaveTicket(TicketDTO ticket)
    {
        string query = "INSERT INTO tickets (id, email, name, status) VALUES (@id, @email, @name, @status)";

        SqlHelper.ExecuteNonQuery(_connString, query,
            new SqlParameter("@id", ticket.ticketnumber),
                new SqlParameter("@email", ticket.email),
                new SqlParameter("@name", ticket.name),
                new SqlParameter("@status", ticket.status.ToString()));
    }

    public TicketDTO[] GetAll()
    {
        var tickets = new List<TicketDTO>();

        string query = "SELECT id, name , email, status FROM tickets";

        using var reader = SqlHelper.ExecuteReader(_connString, query);

        if (!reader.HasRows)
            return tickets.ToArray();

        while (reader.Read())
        {
            TicketDTO ticket = new TicketDTO()
            {
                ticketnumber = reader.GetString("id"),
                name = reader.GetString("name"),
                email = reader.GetString("email"),
                status = Enum.Parse<Status>(reader.GetString("status"))
            };
            tickets.Add(ticket);
        }

        return tickets.ToArray();
    }
}