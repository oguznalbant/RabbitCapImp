using Infra.RabbitMQ;
using System.Diagnostics;

namespace Price.API
{
    public class EventService : BackgroundService
    {
        private readonly IQueueSubscriber<IntegrationEvent> _rabitMQProducer;
        private static readonly HttpClient client = new HttpClient();

        public EventService(IQueueSubscriber<IntegrationEvent> rabitMQProducer)
        {
            _rabitMQProducer = rabitMQProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            client.Timeout = TimeSpan.FromMinutes(5);

            _rabitMQProducer.Subscribe(async (integrationEvent) =>
           {
               var values = new Dictionary<string, string>
               {
                    { "price", "1200" }
               };
               var content = new FormUrlEncodedContent(values);
               var response = client.PostAsync("http://localhost:5157/Price/calcTotalPrice", content);
               response.Wait();
               var response2 = client.PostAsync("http://localhost:5157/Price/calcDiscountPrice", content);
               response2.Wait();

               var response3 = client.PostAsync("http://localhost:5157/Price/calcPrice", content);
               response3.Wait();

               var responseString = await response.Result.Content.ReadAsStringAsync();
               var responseString2 = await response2.Result.Content.ReadAsStringAsync();
               var responseString3 = await response3.Result.Content.ReadAsStringAsync();

               Debug.WriteLine(DateTime.Now + " - " + responseString + " - " + responseString2 + " - " + responseString3);
           });
        }
    }
}
