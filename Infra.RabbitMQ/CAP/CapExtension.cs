using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.RabbitMQ.CAP
{
    public static class CapExtension
    {
        public static void AddCap(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddCap(capOpt =>
            {
                try
                {
                    capOpt.UseRabbitMQ(o =>
                    {
                        o.HostName = "localhost";
                        o.ConnectionFactoryOptions = opt =>
                        {
                            opt.HostName = "localhost";
                            opt.UserName = "admin";
                            opt.Password = "123456";
                            //opt.DispatchConsumersAsync = true;
                        };
                    });

                    capOpt.UseDashboard();
                    capOpt.UseMongoDB(mongoOpt =>
                    {
                        mongoOpt.DatabaseName = "CapDb";
                        mongoOpt.PublishedCollection = "published";
                        mongoOpt.ReceivedCollection = "recevied";
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }
    }
}
