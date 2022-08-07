using Infra.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IQueuePublisher<IntegrationEvent> _rabitMQProducer;

        public BasketController(IQueuePublisher<IntegrationEvent> rabbitMQProducer)
        {
            this._rabitMQProducer = rabbitMQProducer;
        }

        [HttpPost("addproduct")]
        public Task AddProduct(string message)
        {
           _rabitMQProducer.Publish(new IntegrationEvent { CreatedDate = DateTime.Now, EventId = Guid.NewGuid(), Message = message });

            return Task.CompletedTask;
        }
    }
}