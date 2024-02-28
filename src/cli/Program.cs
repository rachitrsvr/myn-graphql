using FluentMigrator.Runner;
using Myn.GraphQL.Cli.Core;

var assembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEntityFrameworkNpgsql()
    .AddCommands(assembly)
    .AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        // Add SQLite support to FluentMigrator
        .AddPostgres()
        // Set the connection string
        .WithGlobalConnectionString("Host=localhost;Database=postgres;Username=postgres;Password=start;Port=5432")
        // Define the assembly containing the migrations
        .ScanIn(typeof(Program).Assembly).For.Migrations())
    // Enable logging to console in the FluentMigrator way
    .AddLogging(lb => lb.AddFluentMigratorConsole())
    .BuildServiceProvider();

var app = builder.Build();

var executor = CommandRegistration.Create(assembly, app.Services);

return await executor.ExecuteAsync(args);

namespace Myn.GraphQL.Cli
{
    public partial class Program { }
}
