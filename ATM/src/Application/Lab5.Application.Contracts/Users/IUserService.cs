using Lab5.Application.Models.Operations;
using Lab5.Application.Models.Users;

namespace Lab5.Application.Contracts.Users;

public interface IUserService
{
    void ValidateUser(string username, string pin);

    User? FindUser(string username, string pin);

    void LoginUser(string username, string pin);

    User GetMoney(string username, string pin, long amountOfMoney);

    User SetMoney(string username, string pin, long amountOfMoney);

    IEnumerable<Operation> WatchHistory(string username, string pin);

    User? ShowCurrentUser();

    void ValidateAdmin(string password);
}