using FluentMigrator.Runner;
using Myn.GraphQL.Cli.Core;
using Oakton;

namespace Myn.GraphQL.Cli.Commands.Database;

[Description("Migrate The Database", Name = "migrate-database")]
public class KafkaMessageCommand(IMigrationRunner runner) : OaktonCommand<EmptyInput>
{
    public override bool Execute(EmptyInput input)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            
            .Build();
        return true;
    }
}