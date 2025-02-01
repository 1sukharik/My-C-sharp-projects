using Lab5.Application.Abstractions.Repositories;
using Lab5.Application.Contracts.Users;
using Lab5.Application.Models.Operations;
using Lab5.Application.Models.Users;

namespace Lab5.Application.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly CurrentUserManager _currentUserManager;

    public UserService(IUserRepository repository, CurrentUserManager currentUserManager)
    {
        _repository = repository;
        _currentUserManager = currentUserManager;
    }

    public void ValidateAdmin(string password)
    {
        var file = new StreamReader("../../../../Lab5.Infrastructure/Repositories/config.txt");
        string? line = file.ReadLine();
        if (password != line)
        {
            throw new InvalidOperationException("Password mismatch");
        }

        User? user = _repository.ValidateAdmin(password);
    }

    public User? ShowCurrentUser()
    {
        return _currentUserManager.User;
    }

    public void ValidateUser(string username, string pin)
    {
        User? user = _repository.ValidateUser(username, pin);

        if (user is null)
        {
            throw new Exception("You don't have an account");
        }

        _currentUserManager.User = user;
    }

    public void LoginUser(string username, string pin)
    {
        _repository.LoginUser(username, pin);
    }

    public User GetMoney(string username, string pin, long amountOfMoney)
    {
        User? user = _repository.FindUser(username, pin);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        long newBalance = user.Balance - amountOfMoney;
        if (newBalance < 0)
        {
            throw new Exception("You can't get more than your balance.");
        }

        if (amountOfMoney < 0)
        {
            throw new Exception("You can't get negative amount of money.");
        }

        return _repository.GetMoney(user, username, pin, newBalance);
    }

    public User SetMoney(string username, string pin, long amountOfMoney)
    {
        User? user = _repository.FindUser(username, pin);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (amountOfMoney < 0)
        {
            throw new Exception("You can't put negative amount of money.");
        }

        long newBalance = user.Balance + amountOfMoney;
        User ans = _repository.SetMoney(user, username, pin, newBalance);
        return ans;
    }

    public IEnumerable<Operation> WatchHistory(string username, string pin)
    {
        User? user = _repository.FindUser(username, pin);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return _repository.WatchHistory(user, username, pin);
    }

    public User? FindUser(string username, string pin)
    {
        return _repository.FindUser(username, pin);
    }
}