using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MyIoC;

namespace WebApi1
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new MyIoCServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
