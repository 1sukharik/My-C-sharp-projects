using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Lab5.Application.Abstractions.Repositories;
using Lab5.Application.Models.Operations;
using Lab5.Application.Models.Users;
using Npgsql;
using System.Globalization;

namespace Lab5.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public UserRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public User? ValidateAdmin(string password)
    {
        return new User(
            Id: -1,
            Username: "admin",
            Balance: 0,
            Pin: password);
    }

    public User? FindUser(string username, string pin)
    {
        const string sql = $"""
                            select user_id, user_name, user_balance, user_pin
                            from users
                            where user_name = @username and user_pin = @pin
                            """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("username", username)
            .AddParameter("pin", pin);

        using NpgsqlDataReader reader = command.ExecuteReader();

        if (reader.Read() is false)
            throw new Exception("User not found");

        return new User(
            Id: reader.GetInt64(0),
            Username: reader.GetString(1),
            Balance: reader.GetInt64(2),
            Pin: reader.GetString(3));
    }

    public User? ValidateUser(string username, string pin)
    {
        const string sql = $"""
                            select user_id, user_name, user_balance, user_pin
                            from users
                            where user_name = @username and user_pin = @pin
                            """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("username", username)
            .AddParameter("pin", pin);

        using NpgsqlDataReader reader = command.ExecuteReader();

        if (reader.Read() is false)
            return null;

        return new User(
            Id: reader.GetInt64(0),
            Username: reader.GetString(1),
            Balance: reader.GetInt64(2),
            Pin: reader.GetString(3));
    }

    public void LoginUser(string username, string pin)
    {
        const string sql = """
                                   INSERT INTO users (user_name, user_balance, user_pin)
                                   VALUES (@username, 0, @pin);
                           """;
        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();
        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("username", username)
            .AddParameter("pin", pin);
        using NpgsqlDataReader reader = command.ExecuteReader();
    }

    public User GetMoney(User user, string? username, string pin, long amountOfMoney)
    {
        const string sql2 = """
                                UPDATE users
                                SET user_balance = :newBalance
                                WHERE user_name = :username AND user_pin = :pin;
                                """;

        NpgsqlConnection connection2 = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlCommand command2 = new NpgsqlCommand(sql2, connection2)
            .AddParameter("username", username)
            .AddParameter("newBalance", amountOfMoney)
            .AddParameter("pin", pin);
        using NpgsqlDataReader reader2 = command2.ExecuteReader();

        const string sql3 = """
                                    INSERT INTO user_operations (user_id, user_type_of_operation, user_operation_amount_of_money)
                                    VALUES (:idUser, :withdrawal, :amountOfMoney);
                            """;
        NpgsqlConnection connection3 = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();
        using NpgsqlCommand command3 = new NpgsqlCommand(sql3, connection3)
            .AddParameter("idUser", user.Id)
            .AddParameter("withdrawal", "withdrawal")
            .AddParameter("amountOfMoney", amountOfMoney);
        using NpgsqlDataReader reader3 = command3.ExecuteReader();

        return new User(
            Id: user.Id,
            Username: user.Username,
            Balance: amountOfMoney,
            Pin: user.Pin);
    }

    public User SetMoney(User user, string username, string pin, long amountOfMoney)
    {
        const string sql2 = """
                                UPDATE users
                                SET user_balance = :newBalance
                                WHERE user_name = :username AND user_pin = :pin;
                                """;

        NpgsqlConnection connection2 = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlCommand command2 = new NpgsqlCommand(sql2, connection2)
            .AddParameter("username", username)
            .AddParameter("newBalance", amountOfMoney)
            .AddParameter("pin", pin);
        using NpgsqlDataReader reader2 = command2.ExecuteReader();

        const string sql3 = """
                                    INSERT INTO user_operations (user_id, user_type_of_operation, user_operation_amount_of_money)
                                    VALUES (:idUser, :deposit, :amountOfMoney);
                            """;
        NpgsqlConnection connection3 = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();
        using NpgsqlCommand command3 = new NpgsqlCommand(sql3, connection3)
            .AddParameter("idUser", user.Id)
            .AddParameter("deposit", "deposit")
            .AddParameter("amountOfMoney", amountOfMoney);
        using NpgsqlDataReader reader3 = command3.ExecuteReader();

        return new User(
            Id: user.Id,
            Username: user.Username,
            Balance: amountOfMoney,
            Pin: user.Pin);
    }

    public IEnumerable<Operation> WatchHistory(User user, string username, string pin)
    {
        const string sql2 = """
                                   select *
                                   from user_operations
                                   where user_id = :user_id
                           """;
        NpgsqlConnection connection2 = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();
        using NpgsqlCommand command2 = new NpgsqlCommand(sql2, connection2)
            .AddParameter("user_id", user.Id);
        using NpgsqlDataReader reader2 = command2.ExecuteReader();
        var operations = new List<Operation>();
        while (reader2.Read())
        {
            var operation = new Operation(
                OperationId: reader2.GetInt64(0),
                UserId: reader2.GetInt64(1),
                Type: reader2.GetString(2),
                Amount: reader2.GetInt64(3),
                Time: reader2.GetDateTime(4).ToString(CultureInfo.CurrentCulture));
            operations.Add(operation);
        }

        return operations;
    }
}