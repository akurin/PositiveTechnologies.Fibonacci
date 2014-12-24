using System;

namespace PositiveTechnologies.Fibonacci.Domain
{
    /// <summary>
    /// Represents class that calculates next Fibonacci number in sequence.
    /// </summary>
    public sealed class FibonacciNumberCalculator
    {
        /// <summary>
        /// Calculates next Fibonacci number in <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public long CalculateNext(FibonacciSequence sequence)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");

            switch (sequence.Length)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                default:
                    return checked(sequence.Previous + sequence.Current);
            }
        }
    }
}