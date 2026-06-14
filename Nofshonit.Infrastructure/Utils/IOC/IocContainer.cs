using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Autofac;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Http;

namespace Nofshonit.Infrastructure.Utils.IOC
{
    public class IocContainer : ICustomContainer
    {
        public Autofac.IContainer Container { get; set; }
        public IocContainer()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var appRoot = Path.GetDirectoryName(location);

            var blConfigBuilder = new ConfigurationBuilder();
            blConfigBuilder.AddJsonFile("ContainerRegistration.json");
            var modules = new Autofac.Configuration.ConfigurationModule(blConfigBuilder.Build());

            var builder = new ContainerBuilder();
            builder.RegisterModule(modules);
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.Register(c =>
            {
                var httpClientFactory = c.Resolve<IHttpClientFactory>();
                return httpClientFactory.CreateClient();
            }).As<HttpClient>().InstancePerLifetimeScope();

            Container = builder.Build();
        }

        public TService Resolve<TService>()
        {
            return Container.Resolve<TService>();
        }

        public TService ResolveByOrganization<TService>(string organizationKey)
        {
            var c = Container.ResolveKeyed<TService>(organizationKey);
            return c;
        }

        public bool IsRegistered<TService>()
        {
            return Container.IsRegistered<TService>();
        }

    }
}
