using AvaBot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AvaBot.Application.Common.Interfaces;
using AvaBot.Infrastructure.Services;

namespace AvaBot.Infrastructure;
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(opt =>
        opt.UseNpgsql(connectionString: configuration.GetConnectionString("PostgresDB"),
        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName).MigrationsHistoryTable("__EFMigrationsHistory", "postgres"))
        .ReplaceService<IHistoryRepository, CamelCaseHistoryContext>());

        services.AddScoped<IChatService, ChatService>();

        return services;
    }
}
