using System;

namespace MyIoC.Models
{
    internal class ServiceDescription
    {
        public Type ServiceType { get; }
        public Type ImplementationType { get; }
        public MyIoCServiceLifetime ServiceLifetime { get;}
        public object ServiceInstance { get; set; }

        public ServiceDescription(Type serviceType, Type implementationType, MyIoCServiceLifetime serviceLifetime, 
             object serviceInstance = null)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            ServiceLifetime = serviceLifetime;
            ServiceInstance = serviceInstance;
        }
    }
}
