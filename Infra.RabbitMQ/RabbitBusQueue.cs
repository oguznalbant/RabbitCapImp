using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Infra.RabbitMQ
{
    public class RabbitMqBusQueue<TQueueMessage> : IQueuePublisher<TQueueMessage>, IQueueSubscriber<TQueueMessage>
    {
        private readonly string queueName = $"{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}_{typeof(TQueueMessage).Name}";
        private readonly IRabbitConnection connection;
        private ILogger logger;
        private IModel channel;
        private QueueDeclareOk queueDeclare;

        public RabbitMqBusQueue(IRabbitConnection connection, ILogger<RabbitMqBusQueue<TQueueMessage>> logger)
        {
            this.connection = connection;
            this.logger = logger;
            try
            {
                this.channel = connection.CreateChannel();
                this.InitChannel();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RabbitMQ connection could not be initialized!");
            }
        }

        public void Publish(TQueueMessage obj)
        {
            if (obj != null)
            {
                var queueMsg = JsonSerializer.Serialize(obj);
                var body = Encoding.UTF8.GetBytes(queueMsg);
                this.channel.BasicPublish(string.Empty, this.queueName, null, body);
            }

        }

        private void InitChannel()
        {
            this.channel?.Dispose();

            this.channel = this.connection.CreateChannel();

            this.queueDeclare = this.channel.QueueDeclare(queue: this.queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            this.channel.CallbackException += (sender, ea) =>
            {
                Debug.WriteLine(ea.Exception.Message);
                //this.InitChannel();
            };
        }

        public void Subscribe(Action<TQueueMessage> action)
        {
            if (this.channel == null)
            {
                return;
            }

            var consumer = new AsyncEventingBasicConsumer(this.channel);
            consumer.Received += async (model, args) =>
            {
                TQueueMessage body = JsonSerializer.Deserialize<TQueueMessage>(args.Body.ToArray());
                action(body);
                this.channel.BasicAck(args.DeliveryTag, false);
            };

            this.channel.BasicConsume(queue: this.queueDeclare.QueueName, autoAck: false, consumer: consumer);
        }
    }
}
