using RabbitMQ.Client;

namespace Infra.RabbitMQ
{
    public interface IRabbitConnection
    {
        bool IsConnected { get; }

        IModel CreateChannel();
    }
}
