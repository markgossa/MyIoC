using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyIoC.Tests.Unit.Contracts;
using MyIoC.Tests.Unit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace MyIoC.Tests.Unit
{
    public class MyIoCContainerTests
    {
        private readonly MyIoCContainer _sut = new MyIoCContainer();

        [Fact]
        public void ThrowsIfTypeNotRegistered() => Assert.Throws<ApplicationException>(() => _sut.GetService<IEngine>());

        [Fact]
        public void RegistersItselfAsIServiceProvider()
        {
            _sut.AddSingleton<ISparkPlug, SparkPlug>();
            _sut.AddSingleton<ICylinder, Cylinder>();
            var serviceProvider = _sut.GetService<IServiceProvider>();
            Assert.Equal(_sut, serviceProvider);
            Assert.IsAssignableFrom<IServiceProvider>(serviceProvider);
        }

        [Fact]
        public void RegistersATypeAsSingletonUsingInterface()
        {
            _sut.AddSingleton<ISparkPlug, SparkPlug>();
            _sut.AddSingleton<ICylinder, Cylinder>();
            var object1 = _sut.GetService<ISparkPlug>();
            var object2 = _sut.GetService<ISparkPlug>();
            Assert.Equal(object1, object2);
            Assert.IsType<SparkPlug>(object1);
            Assert.IsType<SparkPlug>(object2);
        }

        [Fact]
        public void RegistersATypeAsSingletonTwiceUsingInterfaceAndReturnsLastOneRegistered()
        {
            _sut.AddSingleton<ICylinder, Cylinder>();
            _sut.AddSingleton<IEngine, VEngine>();
            _sut.AddSingleton<ISparkPlug, SparkPlug>();
            _sut.AddSingleton<ISparkPlug, SparkPlug>();
            var object1 = _sut.GetService<IEngine>();
            var object2 = _sut.GetService<IEngine>();
            Assert.Equal(object1, object2);
            Assert.IsType<VEngine>(object1);
            Assert.IsType<VEngine>(object2);
        }

        [Fact]
        public void RegistersATypeAsTransientUsingInterface()
        {
            _sut.AddSingleton<ICylinder, Cylinder>();
            _sut.AddTransient<ISparkPlug, SparkPlug>();
            var object1 = _sut.GetService<ISparkPlug>();
            var object2 = _sut.GetService<ISparkPlug>();
            Assert.NotEqual(object1, object2);
        }

        [Fact]
        public void RegistersATypeAsSingletonUsingInterface_Overload1()
        {
            _sut.AddSingleton<ICylinder, Cylinder>();
            _sut.AddSingleton(typeof(ISparkPlug), typeof(SparkPlug));
            var object1 = _sut.GetService<ISparkPlug>();
            var object2 = _sut.GetService<ISparkPlug>();
            Assert.Equal(object1, object2);
            Assert.IsType<SparkPlug>(object1);
            Assert.IsType<SparkPlug>(object2);
        }

        [Fact]
        public void RegistersATypeTwiceAsSingletonUsingInterface_Overload1()
        {
            _sut.AddSingleton(typeof(ICylinder), typeof(Cylinder));
            _sut.AddSingleton(typeof(ISparkPlug), typeof(SparkPlug));
            _sut.AddSingleton(typeof(IEngine), typeof(SparkPlug));
            _sut.AddSingleton(typeof(IEngine), typeof(VEngine));
            var object1 = _sut.GetService<IEngine>();
            var object2 = _sut.GetService<IEngine>();
            Assert.Equal(object1, object2);
            Assert.IsType<VEngine>(object1);
            Assert.IsType<VEngine>(object2);
        }

        [Fact]
        public void RegistersASingletonWithoutImplementationAndDoesNotThrow() => _sut.AddSingleton(typeof(IEngine), null);

        [Fact]
        public void RegistersAComplexDependencyAsSingletonUsingInterface()
        {
            _sut.AddSingleton<ICar, Hatchback>();
            _sut.AddSingleton<IEngine, VEngine>();
            _sut.AddSingleton<ISparkPlug, SparkPlug>();
            _sut.AddSingleton<ICylinder, Cylinder>();
            var car1 = _sut.GetService<ICar>();
            var car2 = _sut.GetService<ICar>();
            Assert.Equal(car1, car2);
            Assert.Equal(10, car1.GetEngine().SparkPlug.Size);
            Assert.IsType<VEngine>(car1.GetEngine());
        }

        [Fact]
        public void ResolvesConcreteDependencies()
        {
            _sut.AddSingleton<IBuilding, House>();
            _sut.AddSingleton<Room, Room>();
            var actual = _sut.GetService<IBuilding>();
            Assert.Equal(1, actual.Room.Beds);
        }

        [Fact]
        public void PopulateAndGetServiceUsingImplentationFactory()
        {
            const string baseAddress = "https://thing.com/";
            var services = new ServiceCollection();
            services.AddSingleton(typeof(HttpClient), s =>
            {
                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(baseAddress)
                };
                return httpClient;
            });

            _sut.Populate(services);
            var actual = _sut.GetService<HttpClient>();

            Assert.Equal(baseAddress, actual.BaseAddress.ToString());
        }

        [Fact]
        public void PopulateAndGetServiceUsingImplentationInstance()
        {
            const string baseAddress = "https://thing.com/";
            var services = new ServiceCollection();
            services.AddSingleton(new HttpClient
                {
                    BaseAddress = new Uri(baseAddress)
                });

            _sut.Populate(services);
            var actual = _sut.GetService<HttpClient>();

            Assert.Equal(baseAddress, actual.BaseAddress.ToString());
        }

        [Fact]
        public void PopulateAndGetServiceUsingImplentationtype()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ISparkPlug, SparkPlug>();
            services.AddSingleton<ICylinder, Cylinder>();

            _sut.Populate(services);
            var actual = _sut.GetService<ISparkPlug>();

            Assert.Equal(10, actual.Size);
        }

        [Fact]
        public void PopulateAndGetServiceThatImplementsGenericInterface()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IMapper<IEngine, EngineInfo>, EngineMapper>();

            _sut.Populate(services);
            var actual = _sut.GetService<IMapper<IEngine, EngineInfo>>();

            Assert.IsType<EngineMapper>(actual);
        }
        
        [Fact]
        public void PopulateAndGetServiceTypeWithGenericParameters()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IBuilder<ICar>, ComponentBuilder<ICar>>();

            _sut.Populate(services);
            var actual = _sut.GetService<IBuilder<ICar>>();

            Assert.IsType<ComponentBuilder<ICar>>(actual);
        }

        [Fact]
        public void PopulateAndGetServiceWhereConstructorParametersContainGenericType()
        {
            var services = new ServiceCollection();
            services.AddSingleton <IAuditor, Auditor>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddSingleton(typeof(ILoggerFactory), typeof(LoggerFactory));

            _sut.Populate(services);
            var actual = _sut.GetService<IAuditor>();

            Assert.IsType<Auditor>(actual);
        }

        [Fact]
        public void PopulateAndGetServiceWhereConstructorParametersContainIEnumerable()
        {
            var services = new ServiceCollection();
            services.AddSingleton<Street, Street>();
            services.AddSingleton(typeof(IEnumerable<House>), new List<House> { new House(new Room()) });

            _sut.Populate(services);
            var actual = _sut.GetService<Street>();

            Assert.IsType<Street>(actual);
            Assert.Equal(1, actual.Houses.First().Room.Beds);
        }
    }
}
