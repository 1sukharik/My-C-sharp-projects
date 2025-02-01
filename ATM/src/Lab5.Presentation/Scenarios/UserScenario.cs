using Lab5.Application.Contracts.Users;
using Spectre.Console;

namespace Lab5.Presentation.Scenarios;

public class UserScenario : IScenario
{
    private readonly IUserService _userService;

    public UserScenario(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "User";

    public void Run()
    {
        AnsiConsole.Clear();
        string name = AnsiConsole.Ask<string>("Enter name:");
        string pin = AnsiConsole.Ask<string>("Enter pin:");
        try
        {
            _userService.ValidateUser(name, pin);
            AnsiConsole.WriteLine("Logged in");
            AnsiConsole.WriteLine("Press any button to exit");
            System.Console.ReadKey();
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine(ex.Message);
            AnsiConsole.WriteLine("Press any button to exit");
            System.Console.ReadKey();
        }
    }
}