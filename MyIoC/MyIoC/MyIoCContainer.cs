using Microsoft.Extensions.DependencyInjection;
using MyIoC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var serviceDescription = FindServiceType(serviceType);
            IsNullServiceDescription(serviceType, serviceDescription);

            if (serviceDescription.ServiceLifetime == ServiceLifetime.Singleton)
            {
                serviceDescription.ServiceInstance ??= serviceDescription switch
                {
                    { ImplementationFactory: { } } => serviceDescription.ImplementationFactory(this),
                    { ImplementationType: { } } => CreateImplementationType(serviceType, serviceDescription),
                    _ => serviceDescription.ServiceInstance
                };

                return serviceDescription.ServiceInstance;
            }

            return CreateInstance(serviceDescription.ImplementationType);
        }

        public void Populate(IServiceCollection services)
        {
            foreach (var service in services)
            {
                var serviceDescription = service switch
                {
                    { ImplementationInstance: { } } => new ServiceDescription(service.ServiceType, service.ImplementationType,
                            service.Lifetime, null, service.ImplementationInstance),
                    { ImplementationFactory: { } } => new ServiceDescription(service.ServiceType, service.ImplementationType,
                        service.Lifetime, service.ImplementationFactory),
                    _ => new ServiceDescription(service.ServiceType, service.ImplementationType,
                        service.Lifetime)
                };

                RegisterService(serviceDescription);
            }
        }

        private void RegisterIServiceProvider() => RegisterService(new ServiceDescription(typeof(IServiceProvider), null,
                ServiceLifetime.Singleton, null, this));

        private void RegisterService(ServiceDescription serviceDescription) => _registeredServices[serviceDescription.ServiceType.GUID] = serviceDescription;

        private ServiceDescription FindServiceType(Type serviceType) => _registeredServices.GetValueOrDefault(serviceType.GUID);

        private static void IsNullServiceDescription(Type serviceType, ServiceDescription serviceDescription)
        {
            if (serviceDescription is null)
            {
                throw new ApplicationException($"{serviceType.FullName} could not be resolved to an implementation");
            }
        }

        private object CreateImplementationType(Type serviceType, ServiceDescription serviceDescription)
        {
            if (serviceDescription.ImplementationType.ContainsGenericParameters)
            {
                var withTypeArguments = serviceDescription.ImplementationType.MakeGenericType(serviceType.GenericTypeArguments);
                return CreateInstance(withTypeArguments);
            }
            else
            {
                return CreateInstance(serviceDescription.ImplementationType);
            }
        }

        private object CreateInstance(Type type)
        {
            var constructorParameters = type.GetConstructors().First().GetParameters();
            var constructorParameterObjects = constructorParameters.Select(x => GetService(x.ParameterType)).ToArray();

            return Activator.CreateInstance(type, constructorParameterObjects?.ToArray());
        }
    }
}
