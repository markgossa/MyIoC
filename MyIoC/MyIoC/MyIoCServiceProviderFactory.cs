using Microsoft.Extensions.DependencyInjection;
using System;

namespace MyIoC
{
    public class MyIoCServiceProviderFactory : IServiceProviderFactory<MyIoCContainerBuilder>
    {
        public MyIoCContainerBuilder CreateBuilder(IServiceCollection services) => new MyIoCContainerBuilder(services);

        public IServiceProvider CreateServiceProvider(MyIoCContainerBuilder containerBuilder) => containerBuilder.Build();
    }
}
