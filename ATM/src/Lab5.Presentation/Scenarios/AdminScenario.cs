using Lab5.Application.Contracts.Users;
using Spectre.Console;

namespace Lab5.Presentation.Scenarios;

public class AdminScenario : IScenario
{
    private readonly IUserService _adminService;

    public AdminScenario(IUserService adminService)
    {
        _adminService = adminService;
    }

    public string Name => "Administrator";

    public void Run()
    {
        AnsiConsole.Clear();
        string pass = AnsiConsole.Ask<string>("Enter password:");
        _adminService.ValidateAdmin(pass);
        AnsiConsole.WriteLine("Password is correct");
        AnsiConsole.WriteLine("Press any button to exit");
        System.Console.ReadKey();
    }
}