using System;

namespace PositiveTechnologies.Fibonacci.CalculationService.Messages
{
    public sealed class FibonacciNumberCalculated
    {
        public long Number { get; set; }
        public string FibonacciSequenceId { get; set; }
    }
}