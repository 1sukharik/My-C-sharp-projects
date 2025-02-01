using Lab5.Application.Contracts.Users;
using Lab5.Application.Models.Users;
using Spectre.Console;

namespace Lab5.Presentation.Scenarios;

public class AddMoneyScenario : IScenario
{
    private readonly IUserService _userService;

    public AddMoneyScenario(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Add money";

    public void Run()
    {
        AnsiConsole.Clear();
        string money = AnsiConsole.Ask<string>("Enter amount:");
        try
        {
            User? currUser = _userService.ShowCurrentUser();
            if (currUser is not null)
            {
                _userService.SetMoney(currUser.Username, currUser.Pin, long.Parse(money));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Thread.Sleep(2000);
        }
    }
}