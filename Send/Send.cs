using System.Text;
using RabbitMQ.Client;
using System.Diagnostics;


var factory = new ConnectionFactory { HostName = "rabbitmq" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "LargeFiles",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
var watch = Stopwatch.StartNew();
for (int i = 0; i < 1000; i++)
{
    string message = new('a', 1_800_000);
    var body = Encoding.UTF8.GetBytes(message);
    var properties = channel.CreateBasicProperties();
    properties.Persistent = true;
    channel.BasicPublish(exchange: string.Empty,
                        routingKey: "LargeFiles",
                        basicProperties: properties,
                        body: body);
    Console.WriteLine($" [x] Sent #{i}");
}
watch.Stop();
Console.WriteLine($"elapsed {watch.Elapsed.TotalSeconds}");
