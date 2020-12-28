namespace DemoApp.Implementation.Loggers
{
    using System;
    using DemoApp.Interfaces;

    // testing class without constructors
    public class ConsoleLogger : ILogger
    {
        public void Log(string text)
        {
            Console.WriteLine($"Logging: {text}");
        }
    }
}
