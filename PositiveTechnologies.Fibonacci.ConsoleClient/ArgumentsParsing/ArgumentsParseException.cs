using System;

namespace PositiveTechnologies.Fibonacci.ConsoleClient.ArgumentsParsing
{
    /// <summary>
    /// Represents error that occurred during command-line arguments parsing.
    /// </summary>
    public sealed class ArgumentsParseException : Exception
    {
        public ArgumentsParseException(string message)
            : base(message)
        { }
    }
}