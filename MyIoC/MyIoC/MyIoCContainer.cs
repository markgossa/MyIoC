using MyIoC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyIoC
{
    public class MyIoCContainer : IServiceProvider
    {
        private readonly Dictionary<Type, ServiceDescription> _registeredServices = new Dictionary<Type, ServiceDescription>();

        public void AddSingleton<TService, TImplementation>() where TImplementation : TService
        {
            var serviceDescription = new ServiceDescription(typeof(TService), typeof(TImplementation),
                    MyIoCServiceLifetime.Singleton);
            AddRegisteredService(serviceDescription);
        }
        
        public void AddSingleton(Type serviceType, Type implementationType)
        {
            var serviceDescription = new ServiceDescription(serviceType, implementationType,
                    MyIoCServiceLifetime.Singleton);
            AddRegisteredService(serviceDescription);
        }

        public void AddTransient<TService, TImplementation>() where TImplementation : TService
        {
            var serviceDescription = new ServiceDescription(typeof(TService), typeof(TImplementation),
                MyIoCServiceLifetime.Transient);
            _registeredServices.Add(typeof(TService), serviceDescription);
        }

        public T GetService<T>() => (T)GetService(typeof(T));

        public object GetService(Type serviceType)
        {
            var serviceDescription = _registeredServices.GetValueOrDefault(serviceType);
            if (serviceDescription is null)
            {
                throw new ApplicationException($"{serviceType.FullName} could not be resolved to an implementation");
            }

            if (serviceDescription.ServiceLifetime == MyIoCServiceLifetime.Singleton)
            {
                serviceDescription.ServiceInstance ??= GetInstance(serviceDescription.ImplementationType);
                return serviceDescription.ServiceInstance;
            }

            return GetInstance(serviceDescription.ImplementationType);
        }

        private void AddRegisteredService(ServiceDescription serviceDescription)
        {
            _registeredServices[serviceDescription.ServiceType] = serviceDescription;
        }

        private object[] GetConstructorParameters(Type type) => type.GetConstructors().First().GetParameters().Select(x => GetService(x.ParameterType)).ToArray();

        private object GetInstance(Type type) => Activator.CreateInstance(type, GetConstructorParameters(type)?.ToArray());
    }
}
