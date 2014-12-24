using System;
using PositiveTechnologies.Fibonacci.CalculationService.Client;

namespace PositiveTechnologies.Fibonacci.ConsoleClient.Calculation
{
    public interface ICalculationServiceClientFactory
    {
        CalculationServiceClient Create();
    }
}
