using Lab5.Application.Extensions;
using Lab5.Infrastructure.Extensions;
using Lab5.Presentation;
using Lab5.Presentation.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace Itmo.ObjectOrientedProgramming.Lab5;

public class Program
{
    public static void Main(string[] args)
    {
        var collection = new ServiceCollection();

        collection
            .AddApplication()
            .AddInfrastructureDataAccess(configuration =>
            {
                configuration.Host = "localhost";
                configuration.Port = 6432;
                configuration.Username = "postgres";
                configuration.Password = "postgres";
                configuration.Database = "postgres";
                configuration.SslMode = "Prefer";
            })
            .AddPresentationConsole();

        ServiceProvider provider = collection.BuildServiceProvider();
        using IServiceScope scope = provider.CreateScope();

        scope.UseInfrastructureDataAccess();

        AtmRunner scenarioAtmRunner = scope.ServiceProvider
            .GetRequiredService<AtmRunner>();

        while (true)
        {
            scenarioAtmRunner.Run();
            AnsiConsole.Clear();
        }
    }
}