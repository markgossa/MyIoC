using MyIoC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyIoC
{
    public class MyIoCContainer : IServiceProvider
    {
        private readonly Dictionary<Type, ServiceDescription> _registeredServices = new Dictionary<Type, ServiceDescription>();

        public T GetInstance<T>(Type type) => (T)GetInstance(type);

        public object GetInstance(Type type)
        {
            if (type.IsInterface)
            {
                return GetService(type);
            }

            return Activator.CreateInstance(type, GetConstructorParameters(type)?.ToArray());
        }

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

        private void AddRegisteredService(ServiceDescription serviceDescription)
        {
            if (_registeredServices.ContainsKey(serviceDescription.ServiceType))
            {
                _registeredServices[serviceDescription.ServiceType] = serviceDescription;
            }
            else
            {
                _registeredServices.Add(serviceDescription.ServiceType, serviceDescription);
            }
        }

        public void AddTransient<TService, TImplementation>() where TImplementation : TService
        {
            var serviceDescription = new ServiceDescription(typeof(TService), typeof(TImplementation),
                MyIoCServiceLifetime.Transient);

            _registeredServices.Add(typeof(TService), serviceDescription);
        }

        public T GetService<T>()
        {
            var serviceDescription = _registeredServices.GetValueOrDefault(typeof(T));
            if (serviceDescription is null)
            {
                throw new ApplicationException($"{typeof(T).FullName} could not be resolved to an implementation");
            }

            if (serviceDescription.ServiceLifetime == MyIoCServiceLifetime.Singleton)
            {
                serviceDescription.ServiceInstance ??= GetInstance<T>(serviceDescription.ImplementationType);
                return (T)serviceDescription.ServiceInstance;
            }

            return GetInstance<T>(serviceDescription.ImplementationType);
        }

        private object[] GetConstructorParameters(Type type)
        {
            if (type is null)
            {
                return null;
            }

            var constructors = type.GetConstructors();


            // don't create a new instance of the constructor parameters. Get the service instead. This will make sure it uses the container to resolve the services.
            //return constructors.First().GetParameters().Select(x => GetInstance(x.ParameterType)).ToArray(); 
            return constructors.First().GetParameters().Select(x => GetService(x.ParameterType)).ToArray(); 
        }

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
    }
}
