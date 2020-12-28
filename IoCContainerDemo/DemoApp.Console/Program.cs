namespace DemoApp.Console
{
    using DemoApp.Implementation;
    using DemoApp.Implementation.Loggers;
    using DemoApp.Implementation.Models;
    using DemoApp.Implementation.Repositories;
    using DemoApp.Implementation.Services;
    using DemoApp.Interfaces;
    using IoCContainer.Demo;

#pragma warning disable SA1600 // Elements should be documented
    internal class Program
#pragma warning restore SA1600 // Elements should be documented
    {
        private static void Main(string[] args)
        {
            var container = new SimpleContainer();
            container.Register<ILogger, ConsoleLogger>();
            container.Register(typeof(IRepository<>), typeof(SqlRepository<>));

            var service = container.Resolve<InvoiceService>();

            var invoice = new Invoice
            {
                Note = "custom text",
                Price = 1,
            };

            service.Create(invoice);

            System.Console.WriteLine("press enter...");
            System.Console.ReadLine();
        }
    }
}
