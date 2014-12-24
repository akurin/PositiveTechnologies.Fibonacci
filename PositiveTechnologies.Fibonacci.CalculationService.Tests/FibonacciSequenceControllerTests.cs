using System;
using System.Linq.Expressions;
using FakeItEasy;
using MassTransit;
using NUnit.Framework;
using PositiveTechnologies.Fibonacci.CalculationService.Messages;
using PositiveTechnologies.Fibonacci.Domain;

namespace PositiveTechnologies.Fibonacci.CalculationService.Tests
{
    [TestFixture]
    public sealed class FibonacciSequenceControllerTests
    {
        [Test]
        public void ShouldCalculateAndPublishFibonacciNumbersToMessageQueue()
        {
            // Arrange
            var postedNumbers = new[] { 0, 1, 3, 8, 21, 55 };
            var expectedToBePublishedToMessageQueueNumbers = new[] { 1, 2, 5, 13, 34, 89 };

            var fakeServiceBus = A.Fake<IServiceBus>();

            var controller = new FibonacciSequenceController(
                new InMemoryFibonacciSequenceRepository(),
                new FibonacciNumberCalculator(),
                fakeServiceBus);

            // Act
            foreach (var number in postedNumbers)
            {
                controller.Post("someSequenceId", number);
            }

            // Assert
            foreach (var expectedNumber in expectedToBePublishedToMessageQueueNumbers)
            {
                var expectedNumberCopy = expectedNumber;

                Expression<Func<FibonacciNumberCalculated, bool>> messageConstraint =
                    message => message.Number == expectedNumberCopy;

                A.CallTo(() => fakeServiceBus.Publish(
                    A<FibonacciNumberCalculated>.That.Matches(messageConstraint)))
                    .MustHaveHappened();
            }
        }
    }
}
