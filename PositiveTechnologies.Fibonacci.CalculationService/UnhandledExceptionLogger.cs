using System;
using System.Web.Http.ExceptionHandling;
using log4net;

namespace PositiveTechnologies.Fibonacci.CalculationService
{
    /// <summary>
    /// Represents exception handler that logs exceptions and detailed information about request.
    /// </summary>
    internal sealed class UnhandledExceptionLogger : ExceptionLogger
    {
        private readonly ILog _log;

        public UnhandledExceptionLogger(ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");

            _log = log;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            var message = string.Format(
                "Unhandled exception processing {0} for {1}",
                context.Request.Method,
                context.Request.RequestUri);

            _log.Error(message, context.Exception);
        }
    }
}