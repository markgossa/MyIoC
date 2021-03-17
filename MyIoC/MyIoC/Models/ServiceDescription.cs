using Microsoft.Extensions.DependencyInjection;
using System;

namespace MyIoC.Models
{
    internal class ServiceDescription
    {
        public Type ServiceType { get; }
        public Type ImplementationType { get; }
        public ServiceLifetime ServiceLifetime { get;}
        public object ServiceInstance { get; set; }
        public Func<IServiceProvider, object> ImplementationFactory { get; }

        public ServiceDescription(Type serviceType, Type implementationType, ServiceLifetime serviceLifetime,
            Func<IServiceProvider, object> implementationFactory = null, object serviceInstance = null)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            ServiceLifetime = serviceLifetime;
            ServiceInstance = serviceInstance;
            ImplementationFactory = implementationFactory;
        }
    }
}
