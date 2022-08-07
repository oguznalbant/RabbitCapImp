// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost", UserName = "admin", Password = "123456" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "queueoz",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    

    for (int i = 0; i < 100000; i++)
    {
        string message = "Message "+ i;
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "",
                         routingKey: "queueoz",
                         basicProperties: null,
                         body: body);

        Console.WriteLine(" [x] Sent {0}", message);
    }
    

    
    Console.ReadLine();
}