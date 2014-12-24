using System;
using PositiveTechnologies.Fibonacci.Domain;

namespace PositiveTechnologies.Fibonacci.CalculationService
{
    /// <summary>
    /// Represents storage of calculated Fibonacci sequences.
    /// </summary>
    public interface IFibonacciSequenceRepository
    {
        /// <summary>
        /// Gets Fibonacci sequence by <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        bool TryGet(string id, out FibonacciSequence sequence);

        /// <summary>
        /// Saves Fibonacci sequence.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sequence"></param>
        void Set(string id, FibonacciSequence sequence);
    }
}