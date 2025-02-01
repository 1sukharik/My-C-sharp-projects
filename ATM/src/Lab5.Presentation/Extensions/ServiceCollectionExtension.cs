using Lab5.Presentation.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Lab5.Presentation.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPresentationConsole(this IServiceCollection collection)
    {
        collection.AddScoped<AtmRunner>();

        collection.AddScoped<IScenarioProvider, AdminProvider>();

        collection.AddScoped<IScenarioProvider, UserProvider>();

        collection.AddScoped<IScenarioProvider, MakeBillProvider>();

        collection.AddScoped<IScenarioProvider, AddMoneyProvider>();

        collection.AddScoped<IScenarioProvider, GetMoneyProvider>();

        collection.AddScoped<IScenarioProvider, WatchHistoryProvider>();

        collection.AddScoped<IScenarioProvider, BalanceProvider>();

        return collection;
    }
}