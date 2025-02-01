using Lab5.Application.Contracts.Users;
using Lab5.Application.Models.Users;
using Spectre.Console;

namespace Lab5.Presentation.Scenarios;

public class BalanceScenario : IScenario
{
    private readonly IUserService _userService;

    public BalanceScenario(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Balance";

    public void Run()
    {
        AnsiConsole.Clear();
        User? currUser = _userService.ShowCurrentUser();
        if (currUser is not null)
        {
            User? finalUser = _userService.FindUser(currUser.Username, currUser.Pin);
            if (finalUser is not null)
            {
                AnsiConsole.WriteLine("Your balance is:");
                AnsiConsole.WriteLine(finalUser.Balance);
                AnsiConsole.WriteLine("Press any button to exit");
                System.Console.ReadKey();
            }
        }
    }
}