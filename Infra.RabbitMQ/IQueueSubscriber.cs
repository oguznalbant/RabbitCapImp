namespace Infra.RabbitMQ
{
    public interface IQueueSubscriber<TQueueMessage>
    {
        void Subscribe(Action<TQueueMessage> action);
    }
}