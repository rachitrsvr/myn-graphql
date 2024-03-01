using Confluent.Kafka;
using Myn.GraphQL.Cli.Core;
using Oakton;

namespace Myn.GraphQL.Cli.Commands.Kafka;

[Description("Migrate The Database", Name = "kafka-produce-messages")]
public class KafkaProduceMessagesCommand : OaktonCommand<EmptyInput>
{
    const string Topic = "cn-accounts";
    const string BootstrapServers = "localhost:9092";
    
    public override bool Execute(EmptyInput input)
    {
        var producerConfig = new ProducerConfig { BootstrapServers = BootstrapServers };

        using var producer = new ProducerBuilder<string, byte[]>(producerConfig).Build();

        string[] users = { "eabara", "jsmith", "sgarcia", "jbernard", "htanaka", "awalther" };
        string[] items = { "book", "alarm clock", "t-shirts", "gift card", "batteries" };

        
        return true;
    }
}