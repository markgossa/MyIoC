using System;

namespace ConsoleApp1.Services
{
    internal class GoodMorningWorldService : IGreetingService
    {
        public void DisplayGreeting(string name) => Console.WriteLine($"Good morning {name}");
    }
}
