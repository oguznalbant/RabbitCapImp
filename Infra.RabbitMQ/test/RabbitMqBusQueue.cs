//using Microsoft.Extensions.Logging;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using Sodexo.BackOffice.Abstraction.Bus;
//using Sodexo.BackOffice.Abstraction.Extentions;
//using System;
//using System.Threading.Tasks;

//namespace Sodexo.BackOffice.Bus.RabbitMq
//{
//    public class RabbitMqBusQueue<TQueueMessage> : IQueuePublisher<TQueueMessage>, IQueueSubscriber<TQueueMessage>
//    {
//        private readonly string queueName = $"{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}_{typeof(TQueueMessage).Name}";
//        private readonly IRabbitConnection connection;
//        private ILogger logger;
//        private IModel channel;
//        private QueueDeclareOk queueDeclare;
//        public RabbitMqBusQueue(IRabbitConnection connection, ILogger<RabbitMqBusQueue<TQueueMessage>> logger)
//        {
//            this.connection = connection;
//            this.logger = logger;
//            try
//            {
//                this.channel = connection.CreateChannel();
//                this.InitChannel();
//            }
//#pragma warning disable CA1031 // Do not catch general exception types
//            catch (Exception ex)
//#pragma warning restore CA1031 // Do not catch general exception types
//            {
//                logger.LogError(ex, "RabbitMQ connection could not be initialized!");
//            }
//        }

//        public void Publish(TQueueMessage obj)
//        {
//            this.channel.BasicPublish(string.Empty, this.queueName, null, obj.ToBytes());
//        }

//        private void InitChannel()
//        {
//            this.channel?.Dispose();

//            this.channel = this.connection.CreateChannel();

//            this.queueDeclare = this.channel.QueueDeclare(queue: this.queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

//            this.channel.CallbackException += (sender, ea) =>
//            {
//                this.InitChannel();
//            };
//        }

//        public void Subscribe(Action<TQueueMessage> action)
//        {
//            if (this.channel == null)
//            {
//                return;
//            }

//            var consumer = new AsyncEventingBasicConsumer(this.channel);
//            consumer.Received += async (model, args) =>
//            {
//                var body = args.Body.ToArray().ToObject<TQueueMessage>();
//                action(body);
//                this.channel.BasicAck(args.DeliveryTag, false);
//            };

//            this.channel.BasicConsume(queue: this.queueDeclare.QueueName, autoAck: false, consumer: consumer);
//        }
//    }
//}
