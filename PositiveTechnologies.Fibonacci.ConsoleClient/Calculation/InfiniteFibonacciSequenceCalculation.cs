using System;
using System.Threading.Tasks;
using log4net;
using PositiveTechnologies.Fibonacci.CalculationService.Client;
using PositiveTechnologies.Fibonacci.Domain;

namespace PositiveTechnologies.Fibonacci.ConsoleClient.Calculation
{
    /// <summary>
    /// Represents infinite Fibonacci sequence calculation.
    /// </summary>
    public sealed class InfiniteFibonacciSequenceCalculation
    {
        private readonly string _name;
        private readonly ILog _log;
        private readonly CalculationServiceClient _calculationServiceClient;
        private readonly FibonacciNumberCalculator _fibonacciNumberCalculator;

        public InfiniteFibonacciSequenceCalculation(
            string name,
            ILog log,
            FibonacciNumberCalculator fibonacciNumberCalculator,
            CalculationServiceClient calculationServiceClient)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (log == null) throw new ArgumentNullException("log");
            if (fibonacciNumberCalculator == null) throw new ArgumentNullException("fibonacciNumberCalculator");
            if (calculationServiceClient == null) throw new ArgumentNullException("calculationServiceClient");

            _name = name;
            _log = log;
            _fibonacciNumberCalculator = fibonacciNumberCalculator;
            _calculationServiceClient = calculationServiceClient;
        }

        /// <summary>
        /// Starts calculation.
        /// </summary>
        public async void StartAsync()
        {
            _calculationServiceClient.Connect();

            try
            {
                await DoInfiniteCalculationLoopAsync();
            }
            catch (Exception ex)
            {
                var logMessage = string.Format("Calculation {0} stopped", _name);
                _log.Error(logMessage, ex);
            }
            finally
            {
                _calculationServiceClient.Disconnect();
            }
        }

        private async Task DoInfiniteCalculationLoopAsync()
        {
            var fibonacciSequenceId = Guid.NewGuid().ToString();
            var fibonacciSequence = FibonacciSequence.Empty(fibonacciSequenceId);

            while (true)
            {
                var fibonacciNumber = await CalculateNext(fibonacciSequence);
                fibonacciSequence = fibonacciSequence.Add(fibonacciNumber);

                var logMessage = string.Format(
                    "Calculation {0} produced Fibonacci number #{1}: {2}",
                    _name,
                    fibonacciSequence.Length - 1,
                    fibonacciNumber);

                _log.Info(logMessage);
            }
        }

        /// <summary>
        /// Calculates next Fibonacci number locally or remotely.
        /// </summary>
        /// <param name="fibonacciSequence"></param>
        /// <returns></returns>
        private async Task<long> CalculateNext(FibonacciSequence fibonacciSequence)
        {
            if (fibonacciSequence == null) throw new ArgumentNullException("fibonacciSequence");

            return fibonacciSequence.Length % 2 == 0
                ? CalculateNextFibonaccyNumberLocally(fibonacciSequence)
                : await CalculateNextFibonaccyNumberRemotelyAsync(fibonacciSequence);
        }

        private Task<long> CalculateNextFibonaccyNumberRemotelyAsync(FibonacciSequence fibonacciSequence)
        {
            return _calculationServiceClient.CalculateNext(
                fibonacciSequence.Id,
                fibonacciSequence.Current);
        }

        private long CalculateNextFibonaccyNumberLocally(FibonacciSequence fibonacciSequence)
        {
            return _fibonacciNumberCalculator.CalculateNext(fibonacciSequence);
        }
    }
}