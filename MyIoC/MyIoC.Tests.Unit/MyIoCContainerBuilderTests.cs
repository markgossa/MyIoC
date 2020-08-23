using Microsoft.Extensions.DependencyInjection;
using MyIoC.Tests.Unit.Contracts;
using MyIoC.Tests.Unit.Models;
using System;
using Xunit;

namespace MyIoC.Tests.Unit
{
    public class MyIoCContainerBuilderTests
    {
        private readonly MyIoCContainerBuilder _sut = new MyIoCContainerBuilder(new ServiceCollection());

        [Fact]
        public void CreatesInstance()
        {
            Assert.IsType<MyIoCContainerBuilder>(_sut);
        }

        [Fact]
        public void ReturnsIServiceProvider()
        {
            var actual = _sut.Build();
            Assert.IsAssignableFrom<IServiceProvider>(actual);
        }

        //[Fact]
        //public void AddsServiceCollectionToContainer_Singleton()
        //{
        //    var serviceCollection = new ServiceCollection();
        //    serviceCollection.AddSingleton<IClassInterface, ClassSingleParam>();
        //    var sut = new MyIoCContainerBuilder(serviceCollection);
        //    var container = sut.Build();
        //    var actual = container.GetService(typeof(IClassInterface));

        //    Assert.IsType<ClassSingleParam>(actual);
        //}
    }
}
