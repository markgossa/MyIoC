using MyIoC.Tests.Unit.Contracts;
using MyIoC.Tests.Unit.Models;
using System;
using Xunit;

namespace MyIoC.Tests.Unit
{
    public class MyIoCContainerTests
    {
        private readonly MyIoCContainer _sut = new MyIoCContainer();

        [Fact]
        public void ReturnsInstanceOfType()
        {
            var actual = _sut.GetInstance(typeof(Cylinder));
            Assert.IsType<Cylinder>(actual);
        }

        [Fact]
        public void ReturnsInstanceOfTypeWithParametersWhichHaveParameters()
        {
            _sut.AddSingleton<IEngine, VEngine>();
            _sut.AddSingleton<ICylinder, Cylinder>();
            _sut.AddSingleton<ISparkPlug, SparkPlug>();
            var actual = (VEngine)_sut.GetInstance(typeof(IEngine));
            Assert.IsType<VEngine>(actual);
            Assert.IsType<Cylinder>(actual.Cylinder);
            Assert.IsType<SparkPlug>(actual.SparkPlug);
        }

        [Fact]
        public void ReturnsInstanceOfTypeWithSingleParameterUsingGenerics()
        {
            _sut.AddSingleton<ICylinder, Cylinder>();
            _sut.AddSingleton<ISparkPlug, SparkPlug>();
            _sut.AddSingleton<IEngine, VEngine>();
            var actual = _sut.GetInstance<SparkPlug>(typeof(SparkPlug));
            Assert.IsType<SparkPlug>(actual);
            Assert.Equal(10, actual.Size);
        }

        [Fact]
        public void ThrowsIfTypeNotRegistered() => Assert.Throws<ApplicationException>(() => _sut.GetService<IEngine>());

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
    }
}
