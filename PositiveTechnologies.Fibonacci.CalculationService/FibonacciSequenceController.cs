using System;
using System.Web.Http;
using MassTransit;
using PositiveTechnologies.Fibonacci.CalculationService.Messages;
using PositiveTechnologies.Fibonacci.Domain;

namespace PositiveTechnologies.Fibonacci.CalculationService
{
    /// <summary>
    /// Represents controller that calculates Fibonacci numbers.
    /// </summary>
    public sealed class FibonacciSequenceController : ApiController
    {
        private readonly IFibonacciSequenceRepository _fibonacciSequenceRepository;
        private readonly FibonacciNumberCalculator _fibonacciNumberCalculator;
        private readonly IServiceBus _serviceBus;

        public FibonacciSequenceController(
            IFibonacciSequenceRepository fibonacciSequenceRepository,
            FibonacciNumberCalculator fibonacciNumberCalculator,
            IServiceBus serviceBus)
        {
            if (fibonacciSequenceRepository == null) throw new ArgumentNullException("fibonacciSequenceRepository");
            if (fibonacciNumberCalculator == null) throw new ArgumentNullException("fibonacciNumberCalculator");
            if (serviceBus == null) throw new ArgumentNullException("serviceBus");

            _fibonacciSequenceRepository = fibonacciSequenceRepository;
            _fibonacciNumberCalculator = fibonacciNumberCalculator;
            _serviceBus = serviceBus;
        }

        /// <summary>
        /// Handles POST-request like this: fibonacciSequence/cd171f7c-560d-4a62-8d65-16b87419a58c?currentNumber=13 and starts calculation of next Fibonacci number.
        /// </summary>
        /// <param name="id">Fibonacci sequence id.</param>
        /// <param name="currentNumber"></param>
        public void Post(string id, long currentNumber)
        {
            if (id == null) throw new ArgumentNullException("id");

            FibonacciSequence fibonacciSequence;

            var newCalculationStarted = !_fibonacciSequenceRepository.TryGet(id, out fibonacciSequence);
            if (newCalculationStarted)
                fibonacciSequence = FibonacciSequence.Empty(id);

            fibonacciSequence = fibonacciSequence.Add(currentNumber);
            fibonacciSequence = fibonacciSequence.Add(_fibonacciNumberCalculator.CalculateNext(fibonacciSequence));

            PublishMessage(id, fibonacciSequence);

            _fibonacciSequenceRepository.Set(id, fibonacciSequence);
        }

        private void PublishMessage(string id, FibonacciSequence fibonacciSequence)
        {
            _serviceBus.Publish(
                new FibonacciNumberCalculated
                {
                    Number = fibonacciSequence.Current,
                    FibonacciSequenceId = id
                });
        }
    }
}