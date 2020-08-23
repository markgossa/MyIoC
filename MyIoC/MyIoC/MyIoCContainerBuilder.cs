using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MyIoC
{
    public class MyIoCContainerBuilder
    {
        private readonly IServiceCollection _services;

        public MyIoCContainerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IServiceProvider Build()
        {
            var container = new MyIoCContainer();

            //var serviceNames = _services.Where(s => s.Lifetime == ServiceLifetime.Singleton).Select(x => x.ServiceType.Name).OrderBy(x => x);

            foreach (var service in _services.Where(s => s.Lifetime == ServiceLifetime.Singleton))
            {
                //container.AddSingleton(service.ServiceType, service.ImplementationType);
            }

            return container;
        }
    }
}
