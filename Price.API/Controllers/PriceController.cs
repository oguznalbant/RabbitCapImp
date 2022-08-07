using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Price.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PriceController : ControllerBase
    {

        public PriceController()
        {
        }

        [HttpPost("calcPrice")]
        public Task<int> CalculatePrice(int price)
        {
            for (int i = 1; i < 100; i++)
            {
                price = i + price;
            }

            return Task.FromResult(price);
        }

        [HttpPost("calcDiscountPrice")]
        public Task<int> CalculateDiscount(int price)
        {
            for (int i = 1; i < 100; i++)
            {
                Thread.Sleep(200);
                price = i + price;
            }

            return Task.FromResult(price);
        }

        [HttpPost("calcTotalPrice")]
        public Task<int> CalculateTotalPrice(int price)
        {
            for (int i = 1; i < 100; i++)
            {
                Thread.Sleep(250);
                price = i + price;
            }

            return Task.FromResult(price);
        }

        [CapSubscribe("test.queue")]
        [HttpPost("calcTotalPriceWithCap")]
        public async Task CapTotalPriceCalc(Infra.RabbitMQ.IntegrationEvent @event)
        {
            Debug.WriteLine($"{@event.Message}");

            //for (int i = 1; i < 100; i++)
            //{
            //    Thread.Sleep(250);
            //    price = i + price;
            //}

            return;
        }
    }
}