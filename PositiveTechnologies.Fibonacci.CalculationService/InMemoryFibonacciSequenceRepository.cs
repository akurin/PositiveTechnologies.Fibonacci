using System;
using System.Collections.Concurrent;
using PositiveTechnologies.Fibonacci.Domain;

namespace PositiveTechnologies.Fibonacci.CalculationService
{
    /// <summary>
    /// Represents in-memory storage of calculated Fibonacci sequences.
    /// </summary>
    /// <remarks>Thread-safe.</remarks>
    public sealed class InMemoryFibonacciSequenceRepository : IFibonacciSequenceRepository
    {
        private readonly ConcurrentDictionary<string, FibonacciSequence> _sequences =
            new ConcurrentDictionary<string, FibonacciSequence>();

        /// <summary>
        /// Gets Fibonacci sequence by <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public bool TryGet(string id, out FibonacciSequence sequence)
        {
            if (id == null) throw new ArgumentNullException("id");

            return _sequences.TryGetValue(id, out sequence);
        }

        /// <summary>
        /// Saves Fibonacci sequence.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sequence"></param>
        public void Set(string id, FibonacciSequence sequence)
        {
            if (id == null) throw new ArgumentNullException("id");

            _sequences[id] = sequence;
        }
    }
}