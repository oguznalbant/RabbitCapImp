using DotNetCore.CAP;
using System.Diagnostics;

namespace Price.API
{
    public class PriceCalculateService : ICapSubscribe
    {
        private static readonly HttpClient client = new HttpClient();

        public PriceCalculateService()
        {
            if (client.Timeout == null)
            {
                client.Timeout = TimeSpan.FromSeconds(60);
            }
        }
        [CapSubscribe("test.queue")]
        public async void CalculatePrice(Infra.RabbitMQ.IntegrationEvent @event)
        {
            var values = new Dictionary<string, string>
               {
                    { "price", "1200" }
               };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("http://localhost:5157/Price/calcTotalPrice", content).ConfigureAwait(false);
            var response2 = await client.PostAsync("http://localhost:5157/Price/calcDiscountPrice", content).ConfigureAwait(false);
            var response3 = await client.PostAsync("http://localhost:5157/Price/calcPrice", content).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var responseString3 = await response3.Content.ReadAsStringAsync();

            Debug.WriteLine(DateTime.Now + " - " + responseString + " - " + responseString2 + " - " + responseString3);
        }
    }
}
