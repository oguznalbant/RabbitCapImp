using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Infra.RabbitMQ
{
    public static class DependencyExtension
    {
        public static void AddRabbitConnection(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSingleton(p =>
            {
                return serviceDescriptors.GetRabbitConnection();
            });
        }

        public static IConnectionFactory GetRabbitConnection(this IServiceCollection serviceDescriptors)
        {
            return new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "123456",
                DispatchConsumersAsync = true,
                //HostName = rmqSettings.HostName,
                //Port = rmqSettings.Port,
                //UserName = rmqSettings.UserName,
                //Password = rmqSettings.Password,
                //VirtualHost = rmqSettings.VirtualHost,
                //DispatchConsumersAsync = true, // this is mandatory to have Async Subscribers
            };
        }
    }
}