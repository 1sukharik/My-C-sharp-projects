using Lab5.Application.Contracts.Users;
using Lab5.Application.Models.Operations;
using Lab5.Application.Models.Users;
using Spectre.Console;

namespace Lab5.Presentation.Scenarios;

public class WatchHistoryScenario : IScenario
{
    private readonly IUserService _userService;

    public WatchHistoryScenario(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Watch history";

    public void Run()
    {
        AnsiConsole.Clear();
        try
        {
            User? currUser = _userService.ShowCurrentUser();
            if (currUser is not null)
            {
                AnsiConsole.WriteLine("Your account history:");
                var table = new Table();
                table.AddColumn(new TableColumn("Id").Centered());
                table.AddColumn(new TableColumn("Time").Centered());
                table.AddColumn(new TableColumn("Action").Centered());
                table.AddColumn(new TableColumn("Amount").Centered());

                foreach (Operation operation in _userService.WatchHistory(currUser.Username, currUser.Pin))
                {
                    var id = new Text(operation.OperationId.ToString());
                    var time = new Text(operation.Time);
                    var type = new Text(operation.Type);
                    var amount = new Text(operation.Amount.ToString());
                    table.AddRow(id, time, type, amount);
                }

                AnsiConsole.Write(table);
                AnsiConsole.WriteLine("Press any button to exit");
                System.Console.ReadKey();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}