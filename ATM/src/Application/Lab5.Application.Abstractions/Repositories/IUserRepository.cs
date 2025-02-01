using Lab5.Application.Models.Operations;
using Lab5.Application.Models.Users;

namespace Lab5.Application.Abstractions.Repositories;

public interface IUserRepository
{
    User? ValidateUser(string username, string pin);

    void LoginUser(string username, string pin);

    User? FindUser(string username, string pin);

    User GetMoney(User user, string username, string pin, long amountOfMoney);

    User SetMoney(User user, string username, string pin, long amountOfMoney);

    IEnumerable<Operation> WatchHistory(User user, string username, string pin);

    User? ValidateAdmin(string password);
}