using System;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using MassTransit;
using PositiveTechnologies.Fibonacci.CalculationService.Client;
using PositiveTechnologies.Fibonacci.ConsoleClient.ArgumentsParsing;
using PositiveTechnologies.Fibonacci.ConsoleClient.Calculation;
using StructureMap;
using StructureMap.AutoFactory;

namespace PositiveTechnologies.Fibonacci.ConsoleClient
{
    /// <summary>
    /// Represents application that performs parallel calculations of Fibonacci numbers.
    /// </summary>
    internal sealed class Program
    {
        private readonly int _numberOfParallelCalculations;
        private readonly InfiniteFibonacciSequenceCalculationFactory _fibonacciSequenceCalculationFactory;

        public Program(int numberOfParallelCalculations, InfiniteFibonacciSequenceCalculationFactory fibonacciSequenceCalculationFactory)
        {
            if (numberOfParallelCalculations <= 0)
                throw new ArgumentException("Should be grater than zero", "numberOfParallelCalculations");
            if (fibonacciSequenceCalculationFactory == null) throw new ArgumentNullException("fibonacciSequenceCalculationFactory");

            _numberOfParallelCalculations = numberOfParallelCalculations;
            _fibonacciSequenceCalculationFactory = fibonacciSequenceCalculationFactory;
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
                Console.WriteLine("Expected 3 arguments:");
                Console.WriteLine("  numbers of parallel calculations;");
                Console.WriteLine("  Calculation Service Web API URI;");
                Console.WriteLine("  Calculation Service Message Queue URI.");
                Console.WriteLine("An example how to calculate 10 Fibonacci sequences in parallel:");
                Console.WriteLine("PositiveTechnologies.Fibonacci.ConsoleClient.exe 10 http://localhost:9000 rabbitmq://localhost/fibonacci");
                return;
            }

            ConfigureLog4Net();

            var container = Bootstrap(arguments);
            var program = container.GetInstance<Program>();

            program.StartAsync();

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
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
                x.For<IServiceBus>().Use(context => CreateServiceBus(arguments)).Singleton();

                x.For<ICalculationServiceClientFactory>().CreateFactory();
                x.For<CalculationServiceClient>()
                    .Use<CalculationServiceClient>()
                    .Ctor<string>("calculationServiceWebApiUri")
                    .Is(arguments.CalculationServiceWebApiUri)
                    .AlwaysUnique();
                
                x.For<Program>()
                    .Use<Program>()
                    .Ctor<int>("numberOfParallelCalculations")
                    .Is(arguments.NumberOfParallelCalculations);

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
            });
        }

        public void StartAsync()
        {
            for (var i = 0; i < _numberOfParallelCalculations; i++)
            {
                var calculationName = "#" + i;
                var calculation = _fibonacciSequenceCalculationFactory.Create(calculationName);
                calculation.StartAsync();
            }
        }
    }
}