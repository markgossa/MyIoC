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
            var actual = _sut.GetInstance(typeof(ClassNoParams));
            Assert.IsType<ClassNoParams>(actual);
        }

        [Fact]
        public void ReturnsInstanceOfTypeWithSingleParameter()
        {
            var actual = (ClassSingleParam)_sut.GetInstance(typeof(ClassSingleParam));
            Assert.IsType<ClassSingleParam>(actual);
            Assert.IsType<ClassNoParams>(actual.Value);
        }

        [Fact]
        public void ReturnsInstanceOfTypeWithMultipleParameters()
        {
            var actual = (ClassMultipleParams)_sut.GetInstance(typeof(ClassMultipleParams));
            Assert.IsType<ClassMultipleParams>(actual);
            Assert.IsType<ClassNoParams>(actual.Value1);
            Assert.IsType<ClassNoParams2>(actual.Value2);
        }

        [Fact]
        public void ReturnsInstanceOfTypeWithParametersWhichHaveParameters()
        {
            var actual = (ClassNestedParams)_sut.GetInstance(typeof(ClassNestedParams));
            Assert.IsType<ClassNestedParams>(actual);
            Assert.IsType<ClassNoParams>(actual.Value1);
            Assert.IsType<ClassSingleParam>(actual.Value2);
        }

        [Fact]
        public void ReturnsInstanceOfTypeWithSingleParameterUsingGenerics()
        {
            var actual = _sut.GetInstance<ClassSingleParam>(typeof(ClassSingleParam));
            Assert.IsType<ClassSingleParam>(actual);
            Assert.IsType<ClassNoParams>(actual.Value);
        }
        
        [Fact]
        public void RegistersATypeUsingInterface()
        {
            _sut.AddSingleton<IClassInterface, ClassSingleParam>();
            var actual = _sut.GetService<IClassInterface>();
            Assert.IsType<ClassSingleParam>(actual);
        }

        [Fact]
        public void RegistersATypeWithDependenciesAsSingletonUsingInterface()
        {
            _sut.AddSingleton<IClassInterface, ClassNestedParams>();
            var actual = _sut.GetService<IClassInterface>();
            Assert.IsType<ClassNestedParams>(actual);
        }

        [Fact]
        public void ThrowsIfTypeNotRegistered()
        {
            Assert.Throws<NullReferenceException>(() => _sut.GetService<IClassInterface>());
        }

        [Fact]
        public void RegistersATypeAsSingletonUsingInterface()
        {
            _sut.AddSingleton<IClassInterface, ClassSingleParam>();
            var object1 = _sut.GetService<IClassInterface>();
            var object2 = _sut.GetService<IClassInterface>();
            Assert.Equal(object1, object2);
            Assert.IsType<ClassSingleParam>(object1);
            Assert.IsType<ClassSingleParam>(object2);
        }

        [Fact]
        public void RegistersMultipleTypesAsSingletonUsingInterface()
        {
            _sut.AddSingleton<IClassInterface2, ClassNoParams2>();
            _sut.AddSingleton<IClassInterface, ClassSingleParam>();
            var object1 = _sut.GetService<IClassInterface2>();
            var object2 = _sut.GetService<IClassInterface2>();
            Assert.Equal(object1, object2);
            Assert.IsType<ClassNoParams2>(object1);
        }

        [Fact]
        public void RegistersATypeAsSingletonTwiceUsingInterfaceAndReturnsLastOneRegistered()
        {
            _sut.AddSingleton<IClassInterface, ClassSingleParam>();
            _sut.AddSingleton<IClassInterface, ClassNestedParams>();
            var object1 = _sut.GetService<IClassInterface>();
            var object2 = _sut.GetService<IClassInterface>();
            Assert.Equal(object1, object2);
            Assert.IsType<ClassNestedParams>(object1);
            Assert.IsType<ClassNestedParams>(object2);
        }

        [Fact]
        public void RegistersATypeAsTransientUsingInterface()
        {
            _sut.AddTransient<IClassInterface, ClassSingleParam>();
            var object1 = _sut.GetService<IClassInterface>();
            var object2 = _sut.GetService<IClassInterface>();
            Assert.NotEqual(object1, object2);
        }

        [Fact]
        public void RegistersATypeAsSingletonUsingInterface_Overload1()
        {
            _sut.AddSingleton(typeof(IClassInterface), typeof(ClassSingleParam));
            var object1 = _sut.GetService<IClassInterface>();
            var object2 = _sut.GetService<IClassInterface>();
            Assert.Equal(object1, object2);
            Assert.IsType<ClassSingleParam>(object1);
            Assert.IsType<ClassSingleParam>(object2);
        }

        [Fact]
        public void RegistersATypeTwiceAsSingletonUsingInterface_Overload1()
        {
            _sut.AddSingleton(typeof(IClassInterface), typeof(ClassSingleParam));
            _sut.AddSingleton(typeof(IClassInterface), typeof(ClassNestedParams));
            var object1 = _sut.GetService<IClassInterface>();
            var object2 = _sut.GetService<IClassInterface>();
            Assert.Equal(object1, object2);
            Assert.IsType<ClassNestedParams>(object1);
            Assert.IsType<ClassNestedParams>(object2);
        }

        [Fact]
        public void RegistersASingletonWithoutImplementationAndDoesNotThrow()
        {
            _sut.AddSingleton(typeof(IClassInterface), null);
        }

        //[Fact]
        //public void RegistersAComplexDependencyAsSingletonUsingInterface()
        //{
        //    _sut.AddSingleton<IClassInterface, ClassSingleParam>();
        //    _sut.AddSingleton<IComplexDependency, ComplexDependency>();
        //    var object1 = _sut.GetService<IComplexDependency>();
        //    var object2 = _sut.GetService<IComplexDependency>();
        //    Assert.NotEqual(object1, object2);
        //}
    }
}
