using Confluent.Kafka;
using Myn.GraphQL.Cli.Core;
using Oakton;
using System.Timers;

namespace Myn.GraphQL.Cli.Commands.Kafka;

[Description("Migrate The Database", Name = "kafka-produce-messages")]
public class KafkaProduceMessagesCommand : OaktonCommand<EmptyInput>
{
    const string Topic = "cn-accounts";
    const string BootstrapServers = "172.18.0.3:9092";
    
    public override bool Execute(EmptyInput input)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9094"
        };


        using var producer = new ProducerBuilder<Null, string>(config).Build();

        // Create a timer with a 2 second interval.
        var timer = new System.Timers.Timer(2000);
        timer.Elapsed += (sender, e) => SendMessage(sender, e, producer);
        timer.AutoReset = true;
        timer.Enabled = true;

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
        return true;
    }
    private static void SendMessage(object sender, ElapsedEventArgs e, IProducer<Null, string> producer)
    {
        try
        {
            var message = $"Message sent at {DateTime.Now}";
            producer.ProduceAsync("my-topic", new Message<Null, string> { Value = message }).Wait();
            Console.WriteLine($"Message sent successfully: {message}");
        }
        catch (ProduceException<Null, string> ex)
        {
            Console.WriteLine($"Delivery failed: {ex.Error.Reason}");
        }
    }
}