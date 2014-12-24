using System;
using System.Linq;
using NUnit.Framework;
using PositiveTechnologies.Fibonacci.Domain;

namespace PositiveTechnologies.Fibonacci.ConsoleClient.Tests
{
    [TestFixture]
    public sealed class FibonacciNumberCalculatorTests
    {
        [Test]
        [TestCase(new int[] { }, 0)]
        [TestCase(new[] { 0, 1 }, 1)]
        [TestCase(new[] { 0, 1, 1, 2 }, 3)]
        [TestCase(new[] { 0, 1, 1, 2, 3, 5 }, 8)]
        public void ShouldCalculateEvenFibonacciNumbersLocally(int[] fibonacciSequenceNumbers, int expectedNumber)
        {
            // Arrange
            var fibonacciSequence = fibonacciSequenceNumbers
                .Aggregate(FibonacciSequence.Empty("someSequenceId"), (current, number) => current.Add(number));
            var fibonacciNumberCalculator = new FibonacciNumberCalculator();

            // Act
            var nextNumber = fibonacciNumberCalculator.CalculateNext(fibonacciSequence);

            // Assert
            Assert.AreEqual(expectedNumber, nextNumber);
        }
    }
}