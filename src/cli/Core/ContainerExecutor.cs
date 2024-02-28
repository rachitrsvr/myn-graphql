using Oakton;

namespace Myn.GraphQL.Cli.Core;

public class ContainerExecutor : ICommandCreator
{
    readonly IServiceProvider _container;

    public ContainerExecutor(IServiceProvider container)
    {
        _container = container;
    }

    public IOaktonCommand CreateCommand(Type commandType)
    {
        return (IOaktonCommand)_container.GetService(commandType);
    }

    public object CreateModel(Type modelType)
    {
        return Activator.CreateInstance(modelType);
    }
}