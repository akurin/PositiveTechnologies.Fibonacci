using System;

namespace PositiveTechnologies.Fibonacci.Domain
{
    /// <summary>
    /// Represents Fibonacci sequence.
    /// </summary>
    public sealed class FibonacciSequence
    {
        private readonly string _id;
        private readonly int _length;
        private readonly long _previous;
        private readonly long _current;

        public static FibonacciSequence Empty(string id)
        {
            return new FibonacciSequence(id, 0, 0, 0);
        }

        private FibonacciSequence(string id, int length, long previous, long current)
        {
            _id = id;
            _length = length;
            _previous = previous;
            _current = current;
        }

        public string Id
        {
            get { return _id; }
        }

        public int Length
        {
            get { return _length; }
        }

        public long Previous
        {
            get
            {
                if (Length < 2)
                    throw new InvalidOperationException("There is no previous element in sequence");

                return _previous;
            }
        }

        public long Current
        {
            get
            {
                if (Length < 1)
                    throw new InvalidOperationException("There is no current element in sequence");

                return _current;
            }
        }

        public FibonacciSequence Add(long number)
        {
            return new FibonacciSequence(_id, _length + 1, _current, number);
        }
    }
}