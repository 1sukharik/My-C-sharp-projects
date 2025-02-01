using Lab5.Application.Contracts.Users;
using Spectre.Console;

namespace Lab5.Presentation.Scenarios;

public class MakeBillScenario : IScenario
{
    private readonly IUserService _userService;

    public MakeBillScenario(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Make bill";

    public void Run()
    {
        AnsiConsole.Clear();
        string name = AnsiConsole.Ask<string>("Enter name:");
        string pin = AnsiConsole.Ask<string>("Enter pin:");
        try
        {
            _userService.FindUser(name, pin);
            AnsiConsole.WriteLine("You have an account");
            AnsiConsole.WriteLine("Press any button to exit");
            System.Console.ReadKey();
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine(ex.Message);
            _userService.LoginUser(name, pin);
            AnsiConsole.WriteLine("Congratulations you now have an account!");
            AnsiConsole.WriteLine("Press any button to exit");
            System.Console.ReadKey();
        }
    }
}