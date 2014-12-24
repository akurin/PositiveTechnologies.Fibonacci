using System;
using System.Web.Http.ExceptionHandling;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using MassTransit;
using Microsoft.Owin.Hosting;
using PositiveTechnologies.Fibonacci.CalculationService.ArgumentsParsing;
using StructureMap;

namespace PositiveTechnologies.Fibonacci.CalculationService
{
    /// <summary>
    /// Represents self-hosting Web Application that calculates Fibonacci numbers and publishes them to the Message Queue.
    /// </summary>
    internal sealed class Program
    {
        private readonly string _calculationServiceWebApiUri;
        private readonly Startup _startup;

        public Program(string calculationServiceWebApiUri, Startup startup)
        {
            if (calculationServiceWebApiUri == null) throw new ArgumentNullException("calculationServiceWebApiUri");
            if (startup == null) throw new ArgumentNullException("startup");

            _calculationServiceWebApiUri = calculationServiceWebApiUri;
            _startup = startup;
        }

        public void Start()
        {
            using (WebApp.Start(url: _calculationServiceWebApiUri, startup: _startup.Configuration))
            {
                Console.WriteLine("Web-server started: {0}", _calculationServiceWebApiUri);
                Console.WriteLine("Press Enter to stop web-server");
                Console.ReadLine();
            }
        }

        public static void Main(string[] args)
        {
            Arguments arguments;
            try
            {
                arguments = Arguments.Parse(args);
            }
            catch (ArgumentsParseException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine("Expected 2 arguments:");
                Console.WriteLine("  Calculation Service Web API URI;");
                Console.WriteLine("  Calculation Service Message Queue URI.");
                Console.WriteLine("An example:");
                Console.WriteLine("PositiveTechnologies.Fibonacci.CalculationService.exe http://localhost:9000 rabbitmq://localhost/fibonacci");
                return;
            }

            ConfigureLog4Net();

            var container = Bootstrap(arguments);
            var program = container.GetInstance<Program>();
            
            program.Start();
        }

        private static void ConfigureLog4Net()
        {
            var consoleAppender = new ConsoleAppender { Layout = new SimpleLayout() };
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.AddAppender(consoleAppender);
            hierarchy.Configured = true;
        }

        private static IContainer Bootstrap(Arguments arguments)
        {
            var container = new Container(x =>
            {
                x.For<ILog>().Use(context => LogManager.GetLogger("Logger"));
                x.For<IFibonacciSequenceRepository>().Use<InMemoryFibonacciSequenceRepository>().Singleton();
                x.For<IServiceBus>().Use(context => CreateServiceBus(arguments)).Singleton();
                x.For<IExceptionLogger>().Use<UnhandledExceptionLogger>();
                x.For<Program>()
                    .Use<Program>()
                    .Ctor<string>("calculationServiceWebApiUri")
                    .Is(arguments.CalculationServiceWebApiUri);
            });

            return container;
        }

        private static IServiceBus CreateServiceBus(Arguments arguments)
        {
            return ServiceBusFactory.New(sbc =>
            {
                sbc.DisablePerformanceCounters();
                sbc.UseRabbitMq();
                sbc.ReceiveFrom(arguments.CalculationServiceMessageQueueUri);
                sbc.SetPurgeOnStartup(true);
            });
        }
    }
}
