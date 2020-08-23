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

        public object GetInstance(Type type) => Activator.CreateInstance(type, GetConstructorParameters(type)?.ToArray());

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

            return constructors.First().GetParameters().Select(x => GetInstance(x.ParameterType)).ToArray();
        }

        public object GetService(Type serviceType) => throw new NotImplementedException();
    }
}
