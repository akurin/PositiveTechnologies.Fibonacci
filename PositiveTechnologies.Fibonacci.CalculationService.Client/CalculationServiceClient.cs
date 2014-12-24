using System;
using System.Threading.Tasks;
using MassTransit;

namespace PositiveTechnologies.Fibonacci.CalculationService.Client
{
    /// <summary>
    /// Represents Calculation Service API.
    /// </summary>
    /// <remarks>Hides the complexity of underlying protocol and allows to work with Calculation Service in request-response style.</remarks>
    public class CalculationServiceClient
    {
        private readonly CalculationServiceWebApiClient _webApiClient;
        private readonly CalculationServiceMessageQueueClient _messageQueueClient;

        public CalculationServiceClient(string calculationServiceWebApiUri, IServiceBus serviceBus)
        {
            if (calculationServiceWebApiUri == null) throw new ArgumentNullException("calculationServiceWebApiUri");
            if (serviceBus == null) throw new ArgumentNullException("serviceBus");

            _webApiClient = new CalculationServiceWebApiClient(calculationServiceWebApiUri);
            _messageQueueClient = new CalculationServiceMessageQueueClient(serviceBus);
        }

        /// <summary>
        /// Establishes connection with Calculation Service.
        /// </summary>
        public void Connect()
        {
            _messageQueueClient.Subscribe();
        }

        /// <summary>
        /// Closes connection with Calculation Service.
        /// </summary>
        public void Disconnect()
        {
            _messageQueueClient.Unsubscribe();
        }

        /// <summary>
        /// Requests next Fibonacci number in sequence identified by <paramref name="fibonacciSequnceId"/>.
        /// </summary>
        /// <param name="fibonacciSequnceId"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public async Task<long> CalculateNext(string fibonacciSequnceId, long current)
        {
            if (fibonacciSequnceId == null) throw new ArgumentNullException("fibonacciSequnceId");

            _messageQueueClient.PrepareToReceiveMessage(
                m => m.FibonacciSequenceId == fibonacciSequnceId);

            await _webApiClient.CalculateNextAsync(fibonacciSequnceId, current);

            var fibonacciNumberCalculatedMessage = await _messageQueueClient.ReceiveMessageAsync();
            return fibonacciNumberCalculatedMessage.Number;
        }
    }
}