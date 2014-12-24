using System;
using log4net;
using MassTransit;
using PositiveTechnologies.Fibonacci.Domain;

namespace PositiveTechnologies.Fibonacci.ConsoleClient.Calculation
{
    /// <summary>
    /// Represents factory for the <see cref="InfiniteFibonacciSequenceCalculation"/> class.
    /// </summary>
    internal sealed class InfiniteFibonacciSequenceCalculationFactory
    {
        private readonly ILog _log;
        private readonly IServiceBus _serviceBus;
        private readonly FibonacciNumberCalculator _fibonacciNumberCalculator;
        private readonly ICalculationServiceClientFactory _calculationServiceClientFactory;

        public InfiniteFibonacciSequenceCalculationFactory(
            ILog log,
            IServiceBus serviceBus,
            FibonacciNumberCalculator fibonacciNumberCalculator,
            ICalculationServiceClientFactory calculationServiceClientFactory)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (serviceBus == null) throw new ArgumentNullException("serviceBus");
            if (fibonacciNumberCalculator == null) throw new ArgumentNullException("fibonacciNumberCalculator");
            if (calculationServiceClientFactory == null)
                throw new ArgumentNullException("calculationServiceClientFactory");

            _log = log;
            _serviceBus = serviceBus;
            _fibonacciNumberCalculator = fibonacciNumberCalculator;
            _calculationServiceClientFactory = calculationServiceClientFactory;
        }

        /// <summary>
        /// Creates named <see cref="InfiniteFibonacciSequenceCalculation"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public InfiniteFibonacciSequenceCalculation Create(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            var calculationServiceClient = _calculationServiceClientFactory.Create();
            return new InfiniteFibonacciSequenceCalculation(name, _log, _fibonacciNumberCalculator, calculationServiceClient);
        }
    }
}