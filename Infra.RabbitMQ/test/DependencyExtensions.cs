//using Autofac;
//using Microsoft.Extensions.Configuration;
//using RabbitMQ.Client;
//using Sodexo.BackOffice.Abstraction.Bus;
//using Sodexo.BackOffice.Abstraction.Events.RabbitMq;
//using System;

//namespace Sodexo.BackOffice.Bus.RabbitMq
//{
//    public static class DependencyExtensions
//    {
//        public static ContainerBuilder AddRabbitMq<TQueueMessage>(this ContainerBuilder builder)
//        {
//            builder.Register(p =>
//            {
//                var configuration = p.Resolve<IConfiguration>();
//                var rmqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();
//                return new ConnectionFactory()
//                {
//                    HostName = rmqSettings.HostName,
//                    Port = rmqSettings.Port,
//                    UserName = rmqSettings.UserName,
//                    Password = rmqSettings.Password,
//                    VirtualHost = rmqSettings.VirtualHost,
//                    DispatchConsumersAsync = true, // this is mandatory to have Async Subscribers
//                };
//            }).As<IConnectionFactory>().SingleInstance();
            
//            builder.RegisterType<RabbitPersistentConnection>().As<IRabbitConnection>().SingleInstance();
//            builder.RegisterType<RabbitMqBusQueue<TQueueMessage>>().AsImplementedInterfaces().SingleInstance();

//            return builder;
//        }
//    }
//}
