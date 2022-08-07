using Infra.RabbitMQ;
using System.Diagnostics;

namespace Price.API
{
    public class EventService : BackgroundService
    {
        private readonly Infra.RabbitMQ.IQueueSubscriber<IntegrationEvent> _rabitMQProducer;

        public EventService(Infra.RabbitMQ.IQueueSubscriber<IntegrationEvent> rabitMQProducer)
        {
            _rabitMQProducer = rabitMQProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _rabitMQProducer.Subscribe((integrationEvent) =>
            {
                Thread.Sleep(5000);
                Debug.WriteLine(integrationEvent.Message);
            });
        }
    }
}
