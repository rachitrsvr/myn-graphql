using Confluent.Kafka;
using Myn.GraphQL.Cli.Core;
using Oakton;

namespace Myn.GraphQL.Cli.Commands.Kafka
{
    [Description("Migrate The Database", Name = "kafka-consume-messages")]
    public class KafkaConsumerMessagesCommand : OaktonCommand<EmptyInput>
    {
        const string Topic = "cn-accounts";
        const string BootstrapServers = "172.18.0.3:9092";

        public override bool Execute(EmptyInput input)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("my-topic");

            try
            {
                while (true)
                {
                    var consumeResult = consumer.Consume();
                    Console.WriteLine($"Consumed message: {consumeResult.Message.Value}");
                }
            }
            catch (OperationCanceledException)
            {
                // Ctrl-C was pressed.
            }
            finally
            {
                consumer.Close();
            }
            return true;
        }
    }
}
