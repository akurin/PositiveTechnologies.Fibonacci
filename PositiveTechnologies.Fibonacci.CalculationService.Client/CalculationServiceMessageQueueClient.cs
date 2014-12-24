using System;
using System.Threading.Tasks;
using MassTransit;
using PositiveTechnologies.Fibonacci.CalculationService.Messages;

namespace PositiveTechnologies.Fibonacci.CalculationService.Client
{
    /// <summary>
    /// Represents Calculation Service Message Queue listener that allows to receive message in async-await style.
    /// </summary>
    internal sealed class CalculationServiceMessageQueueClient
    {
        private readonly IServiceBus _serviceBus;
        private TaskCompletionSource<FibonacciNumberCalculated> _taskCompletionSource;
        private Predicate<FibonacciNumberCalculated> _messagePredicate = message => false;
        private UnsubscribeAction _unsubscribeAction;

        public CalculationServiceMessageQueueClient(IServiceBus serviceBus)
        {
            if (serviceBus == null) throw new ArgumentNullException("serviceBus");

            _serviceBus = serviceBus;
        }

        /// <summary>
        /// Starts listening to Calculation Service Message Queue.
        /// </summary>
        public void Subscribe()
        {
            if (_unsubscribeAction != null)
                throw new InvalidOperationException("Unsubscribe should be called at first");

            _unsubscribeAction = _serviceBus.SubscribeHandler<FibonacciNumberCalculated>(message =>
            {
                if (_messagePredicate(message))
                    SetResult(message);
            });
        }

        /// <summary>
        /// Stops listening to Calculation Service Message Queue.
        /// </summary>
        public void Unsubscribe()
        {
            if (_unsubscribeAction == null)
                throw new InvalidOperationException("Subscribe should be called at first");
        }

        private void SetResult(FibonacciNumberCalculated message)
        {
            if (_taskCompletionSource == null)
                throw new InvalidOperationException("PrepareToReceiveMessage should be called at first");

            _taskCompletionSource.SetResult(message);
        }

        /// <summary>
        /// Prepares to receive message that satisfies <paramref name="messagePredicate"/>.
        /// </summary>
        /// <param name="messagePredicate"></param>
        public void PrepareToReceiveMessage(Predicate<FibonacciNumberCalculated> messagePredicate)
        {
            if (messagePredicate == null) throw new ArgumentNullException("messagePredicate");

            _messagePredicate = messagePredicate;
            _taskCompletionSource = new TaskCompletionSource<FibonacciNumberCalculated>();
        }

        /// <summary>
        /// Receives message.
        /// </summary>
        /// <returns></returns>
        public async Task<FibonacciNumberCalculated> ReceiveMessageAsync()
        {
            return await _taskCompletionSource.Task;
        }
    }
}