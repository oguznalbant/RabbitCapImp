using RabbitMQ.Client;

namespace Infra.RabbitMQ
{
    public class RabbitPersistentConnection : IDisposable, IRabbitConnection
    {
        private readonly IConnectionFactory connectionFactory;
        private IConnection connection;
        private bool disposed;

        private readonly object semaphore = new object();

        public RabbitPersistentConnection(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public bool IsConnected => this.connection != null && this.connection.IsOpen && !this.disposed;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.connection.Dispose();
            this.disposed = true;
        }

        private void TryConnect()
        {
            lock (this.semaphore)
            {
                if (this.IsConnected)
                {
                    return;
                }

                this.connection = this.connectionFactory.CreateConnection();
                this.connection.ConnectionShutdown += (s, e) => this.TryConnect();
                this.connection.CallbackException += (s, e) => this.TryConnect();
                this.connection.ConnectionBlocked += (s, e) => this.TryConnect();
            }
        }

        public IModel CreateChannel()
        {
            this.TryConnect();

            if (!this.IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return this.connection.CreateModel();
        }
    }
}
