namespace Infra.RabbitMQ
{
    public interface IQueuePublisher<TQueueMessage>
    {
        void Publish(TQueueMessage obj);
    }
}