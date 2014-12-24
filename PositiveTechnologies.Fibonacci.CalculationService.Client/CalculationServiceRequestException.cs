using System;

namespace PositiveTechnologies.Fibonacci.CalculationService.Client
{
    /// <summary>
    /// Represents exception that occurred during request to Calculation Service.
    /// </summary>
    public sealed class CalculationServiceRequestException : Exception
    {
        public CalculationServiceRequestException(string message)
            : base(message)
        { }
    }
}