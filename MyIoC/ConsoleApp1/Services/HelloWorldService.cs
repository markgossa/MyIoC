using System;

namespace ConsoleApp1.Services
{
    internal class HelloWorldService : IGreetingService
    {
        public void DisplayGreeting(string name) => Console.WriteLine($"Hello {name}");
    }
}
