namespace Infra.RabbitMQ
{
    public class IntegrationEvent
    {
        public Guid EventId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Message { get; set; }
    }
}