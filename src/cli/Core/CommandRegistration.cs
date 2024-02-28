using System.Reflection;
using Oakton;

namespace Myn.GraphQL.Cli.Core;

public static class CommandRegistration
{
    public static IServiceCollection AddCommands
    (
        this IServiceCollection container,
        Assembly assembly
    )
    {
        foreach (var type in assembly.GetExportedTypes().Where(CommandFactory.IsOaktonCommandType))
        {
            container.AddTransient(type);
        }

        return container;
    }
    
    public static CommandExecutor Create
    (
        Assembly assembly,
        IServiceProvider container    
    )
    {
        return CommandExecutor.For(f =>
        {
            // do the other configuration of the CommandFactory
            f.RegisterCommands(assembly);
    
        }, new ContainerExecutor(container));
    }
}