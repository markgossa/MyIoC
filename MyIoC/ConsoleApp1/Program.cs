using ConsoleApp1.Services;
using MyIoC;

namespace ConsoleApp1
{
    internal class Program
    {
        private static MyIoCContainer _container;

        private static void Main(string[] args)
        {
            RegisterServices();
            var greetingService = _container.GetService<IGreetingService>();
            greetingService.DisplayGreeting("Mark");
        }

        private static void RegisterServices()
        {
            _container = new MyIoCContainer();
            _container.AddSingleton<IGreetingService, HelloWorldService>();
        }
    }
}
