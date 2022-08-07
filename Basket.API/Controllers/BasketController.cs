using DotNetCore.CAP;
using Infra.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IQueuePublisher<IntegrationEvent> _rabitMQProducer;
        private readonly ICapPublisher _capPublisher;

        public BasketController(IQueuePublisher<IntegrationEvent> rabbitMQProducer, ICapPublisher capPublisher)
        {
            _rabitMQProducer = rabbitMQProducer;
            _capPublisher = capPublisher;
        }

        [HttpPost("addproduct")]
        public Task AddProduct(string message)
        {
            Parallel.For(0, 100, new ParallelOptions { MaxDegreeOfParallelism = 5 }, a =>
            {
                _rabitMQProducer.Publish(new IntegrationEvent { CreatedDate = DateTime.Now, EventId = Guid.NewGuid(), Message = message });
            });


            return Task.CompletedTask;
        }

        [HttpPost("addproductWithCap")]
        public Task AddProductWithCap(string message)
        {
            Parallel.For(0, 1, new ParallelOptions { MaxDegreeOfParallelism = 1 }, a =>
            {
                _capPublisher.Publish("test.queue", new IntegrationEvent { CreatedDate = DateTime.Now, EventId = Guid.NewGuid(), Message = message });
            });


            return Task.CompletedTask;
        }

        //[CapSubscribe("test.queue")]
        //[HttpPost("calcTotalPriceWithCap")]
        //public Task CapTotalPriceCalc(Infra.RabbitMQ.IntegrationEvent @event)
        //{
        //    Debug.WriteLine($"{@event.Message}");

        //    //for (int i = 1; i < 100; i++)
        //    //{
        //    //    Thread.Sleep(250);
        //    //    price = i + price;
        //    //}

        //    return Task.CompletedTask;
        //}
    }
}