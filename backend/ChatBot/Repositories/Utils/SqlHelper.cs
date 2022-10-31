using Microsoft.Data.SqlClient;
using System.Data;

namespace ChatBot.Repositories.Utils;

// NOTICE: This class does NOT support other command types except for CommandType.Text
internal static class SqlHelper
{
    public static int ExecuteNonQuery(string connectionString, string commandText, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(connectionString);

        return ExecuteNonQuery(conn, commandText, parameters);
    }

    public static int ExecuteNonQuery(SqlConnection connection, string commandText, params SqlParameter[] parameters)
    {
        using var cmd = new SqlCommand(commandText, connection);
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddRange(parameters);

        if (connection.State != ConnectionState.Open)
            connection.Open();

        return cmd.ExecuteNonQuery();
    }

    public static object ExecuteScalar(string connectionString, string commandText, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(connectionString);

        return ExecuteScalar(conn, commandText, parameters);
    }

    public static object ExecuteScalar(SqlConnection connection, string commandText, params SqlParameter[] parameters)
    {
        using var cmd = new SqlCommand(commandText, connection);

        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddRange(parameters);

        if (connection.State != ConnectionState.Open)
            connection.Open();

        return cmd.ExecuteScalar();
    }

    public static SqlDataReader ExecuteReader(string connectionString, string commandText, params SqlParameter[] parameters)
    {
        var conn = new SqlConnection(connectionString);
        return ExecuteReader(conn, commandText, parameters);
    }

    public static SqlDataReader ExecuteReader(SqlConnection connection, string commandText, params SqlParameter[] parameters)
    {
        using var cmd = new SqlCommand(commandText, connection);
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddRange(parameters);

        if (connection.State != ConnectionState.Open)
            connection.Open();

        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        return reader;
    }
}