using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyIoC
{
    public class MyIoCContainer : IServiceProvider
    {
        private readonly Dictionary<Type, Func<object>> _registeredServices = new Dictionary<Type, Func<object>>();
        private readonly List<object> _singletons = new List<object>();

        public T GetInstance<T>(Type type) => (T)GetInstance(type);

        public object GetInstance(Type type)
        {
            return Activator.CreateInstance(type, GetConstructorParameters(type)?.ToArray());
        }

        //public object GetInstance(Type type)
        //{
        //    var constructorParameters = new List<object>();

        //    foreach (var parameter in GetConstructorParameters(type)?.ToArray())
        //    {
        //        if (parap.)
        //    }

        //    return Activator.CreateInstance(type, );
        //}

        public void AddSingleton<TService, TImplementation>() where TImplementation : TService
        {
            object action() => _singletons.LastOrDefault(s => s is TImplementation);
            if (_registeredServices.ContainsKey(typeof(TService)))
            {
                _registeredServices[typeof(TService)] = action;
                _singletons.RemoveAll(s => s is TService);
            }
            else
            {
                _registeredServices.Add(typeof(TService), action);
            }

            _singletons.Add(GetInstance(typeof(TImplementation)));
        }

        public void AddSingleton(Type service, Type implementation)
        {
            if (_registeredServices.ContainsKey(service))
            {
                _registeredServices[service] = action;
                _singletons.RemoveAll(s => s.GetType() == service);
            }
            else
            {
                _registeredServices.Add(service, action);
            }

            if (implementation == null)
            {
                return;
            }

            object action() => _singletons.LastOrDefault(s => s.GetType() == implementation);
            _singletons.Add(GetInstance(implementation));
        }

        public void AddTransient<T1, T2>() where T2 : T1 => _registeredServices.Add(typeof(T1), () => GetInstance(typeof(T2)));

        public T GetService<T>() => (T)_registeredServices.GetValueOrDefault(typeof(T))();

        private object[] GetConstructorParameters(Type type)
        {
            if (type is null)
            {
                return null;
            }

            var constructors = type.GetConstructors();

            return constructors.First().GetParameters().Select(x => GetInstance(x.ParameterType)).ToArray();
        }

        public object GetService(Type serviceType)
        {
            _registeredServices.TryGetValue(serviceType, out var value);
            return value() ?? null;
        }

        //public object GetService(Type serviceType)
        //{
        //    return _registeredServices.GetValueOrDefault(serviceType)();
        //}
    }
}
