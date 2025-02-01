using Itmo.Dev.Platform.Common.Extensions;
using Itmo.Dev.Platform.Postgres.Extensions;
using Itmo.Dev.Platform.Postgres.Models;
using Lab5.Application.Abstractions.Repositories;
using Lab5.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Lab5.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDataAccess(
        this IServiceCollection collection,
        Action<PostgresConnectionConfiguration> configuration)
    {
        collection.AddPlatform();
        collection.AddPlatformPostgres(builder => builder.Configure(configuration));
        collection.AddPlatformMigrations(typeof(ServiceCollectionExtensions).Assembly);

        collection.AddScoped<IUserRepository, UserRepository>();

        return collection;
    }
}