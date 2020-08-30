using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyIoC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace MyIoC
{
    public class MyIoCContainer : IServiceProvider
    {
        private readonly Dictionary<Guid, ServiceDescription> _registeredServices = new Dictionary<Guid, ServiceDescription>();

        public MyIoCContainer()
        {
            RegisterIServiceProvider();
        }

        public void AddSingleton<TService, TImplementation>() where TImplementation : TService
        {
            var serviceDescription = new ServiceDescription(typeof(TService), typeof(TImplementation),
                    ServiceLifetime.Singleton);
            RegisterService(serviceDescription);
        }
        
        public void AddSingleton(Type serviceType, Type implementationType)
        {
            var serviceDescription = new ServiceDescription(serviceType, implementationType,
                    ServiceLifetime.Singleton);
            RegisterService(serviceDescription);
        }

        public void AddTransient<TService, TImplementation>() where TImplementation : TService
        {
            var serviceDescription = new ServiceDescription(typeof(TService), typeof(TImplementation),
                ServiceLifetime.Transient);
            RegisterService(serviceDescription);
        }

        public T GetService<T>() => (T)GetService(typeof(T));

        public object GetService(Type serviceType)
        {
            var serviceDescription = _registeredServices.GetValueOrDefault(serviceType.GUID);
            if (serviceDescription is null)
            {
                throw new ApplicationException($"{serviceType.FullName} could not be resolved to an implementation");
            }

            if (serviceDescription.ServiceLifetime == ServiceLifetime.Singleton)
            {
                if (serviceDescription.ImplementationFactory is { })
                {
                    serviceDescription.ServiceInstance = serviceDescription.ImplementationFactory(this);
                }
                else if (serviceDescription.ServiceInstance is { })
                {
                    return serviceDescription.ServiceInstance;
                }
                else if (serviceDescription.ImplementationType is { })
                {
                    if (serviceDescription.ImplementationType.ContainsGenericParameters)
                    {
                        var withTypeArguments = serviceDescription.ImplementationType.MakeGenericType(serviceType.GenericTypeArguments);
                        serviceDescription.ServiceInstance = CreateInstance(withTypeArguments);
                    }
                    else
                    {
                        serviceDescription.ServiceInstance = CreateInstance(serviceDescription.ImplementationType);
                    }
                }
                
                return serviceDescription.ServiceInstance;
            }

            return CreateInstance(serviceDescription.ImplementationType);
        }

        public void Populate(IServiceCollection services)
        {
            foreach (var service in services)
            {
                ServiceDescription serviceDescription = null;
                if (service.ImplementationInstance is { })
                {
                    serviceDescription = new ServiceDescription(service.ServiceType, service.ImplementationType,
                            service.Lifetime, null, service.ImplementationInstance);
                }
                else if (service.ImplementationFactory is { })
                {
                    serviceDescription = new ServiceDescription(service.ServiceType, service.ImplementationType,
                        service.Lifetime, service.ImplementationFactory);
                }
                else
                {
                    serviceDescription = new ServiceDescription(service.ServiceType, service.ImplementationType,
                        service.Lifetime);
                }

                RegisterService(serviceDescription);
            }
        }

        private void RegisterIServiceProvider() => RegisterService(new ServiceDescription(typeof(IServiceProvider), null,
                ServiceLifetime.Singleton, null, this));

        private void RegisterService(ServiceDescription serviceDescription) => _registeredServices[serviceDescription.ServiceType.GUID] = serviceDescription;

        private object CreateInstance(Type type)
        {
            var constructorParameters = type.GetConstructors().First().GetParameters();

            var constructorParameterObjects = constructorParameters.Select(x =>
            {
                return GetService(x.ParameterType);
            }).ToArray();

            return Activator.CreateInstance(type, constructorParameterObjects?.ToArray());
        }
    }
}
