using Confluent.Kafka;
using Myn.GraphQL.Cli.Core;
using Oakton;

namespace Myn.GraphQL.Cli.Commands.Kafka
{
    [Description("Migrate The Database", Name = "kafka-consume-messages")]
    public class KafkaConsumerMessagesCommand : OaktonCommand<EmptyInput>
    {

        public override bool Execute(EmptyInput input)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using (var adminClient = new AdminClientBuilder(config).Build())
            {
                try
                {
                    var topicMetadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
                    foreach (var topic in topicMetadata.Topics)
                    {
                        Console.WriteLine($"Topic: {topic.Topic}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("postgres.public.student");

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
