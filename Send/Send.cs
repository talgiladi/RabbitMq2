using System.Text;
using RabbitMQ.Client;


var factory = new ConnectionFactory { HostName = "rabbitmq" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
int index = 0;
while(true){
    string message = $"#{index++} {DateTime.UtcNow}";
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: string.Empty,
                        routingKey: "hello",
                        basicProperties: null,
                        body: body);
    Console.WriteLine($" [x] Sent {message}");
    System.Threading.Thread.Sleep(1000);
}
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
