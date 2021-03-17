using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace MyIoC.Tests.Unit
{
    public class MyIoCServiceProviderFactoryTests
    {
        private MyIoCServiceProviderFactory _sut = new MyIoCServiceProviderFactory();

        [Fact]
        public void ReturnsBuilder()
        {
            Assert.IsType<MyIoCContainerBuilder>(_sut.CreateBuilder(new ServiceCollection()));
        }

        [Fact]
        public void ReturnsIServiceProvider()
        {
            var builder = _sut.CreateBuilder(new ServiceCollection());
            var serviceProvider = _sut.CreateServiceProvider(builder);

            Assert.IsAssignableFrom<IServiceProvider>(serviceProvider);
        }
    }
}
