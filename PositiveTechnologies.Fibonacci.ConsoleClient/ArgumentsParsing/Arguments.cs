using System;

namespace PositiveTechnologies.Fibonacci.ConsoleClient.ArgumentsParsing
{
    /// <summary>
    /// Represents command-line arguments.
    /// </summary>
    internal sealed class Arguments
    {
        private readonly int _numberOfParallelCalculations;
        private readonly string _calculationServiceWebApiUri;
        private readonly string _calculationServiceMessageQueueUri;

        private Arguments(
            int numberOfParallelCalculations,
            string calculationServiceWebApiUri,
            string calculationServiceMessageQueueUri)
        {
            _numberOfParallelCalculations = numberOfParallelCalculations;
            _calculationServiceWebApiUri = calculationServiceWebApiUri;
            _calculationServiceMessageQueueUri = calculationServiceMessageQueueUri;
        }

        /// <summary>
        /// Parses command-line arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Arguments Parse(string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");

            if (args.Length != 3)
                throw new ArgumentsParseException("Wrong number of command-line arguments");

            var numberOfParallelCalculations = ParseNumberOfParallelCalculations(args[0]);
            var calculationServiceWebApiUri = ParseCalculationServiceWebApiUri(args[1]);
            var calculationServiceMessageQueueUri = ParseCalculationServiceMessageQueueUri(args[2]);

            return new Arguments(
                numberOfParallelCalculations,
                calculationServiceWebApiUri,
                calculationServiceMessageQueueUri);
        }

        private static int ParseNumberOfParallelCalculations(string value)
        {
            int numberOfParallelCalculations;
            if (!int.TryParse(value, out numberOfParallelCalculations))
                throw new ArgumentsParseException("Number of parallel calculations should be of Int32");

            if (numberOfParallelCalculations <= 0)
                throw new ArgumentsParseException("Number of parallel calculations should be greater than zero");

            return numberOfParallelCalculations;
        }

        private static string ParseCalculationServiceWebApiUri(string value)
        {
            if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
                throw new ArgumentsParseException("Calculation Service Web API URI should be absolute URI");

            return value;
        }

        private static string ParseCalculationServiceMessageQueueUri(string value)
        {
            if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
                throw new ArgumentsParseException("Calculation Service Message Queue URI should be absolute URI");

            return value;
        }

        /// <summary>
        /// Number of parallel calculations.
        /// </summary>
        public int NumberOfParallelCalculations
        {
            get { return _numberOfParallelCalculations; }
        }

        /// <summary>
        /// Calculation Service Web API URI.
        /// </summary>
        public string CalculationServiceWebApiUri
        {
            get { return _calculationServiceWebApiUri; }
        }

        /// <summary>
        /// Calculation Service Message Queue URI.
        /// </summary>
        public string CalculationServiceMessageQueueUri
        {
            get { return _calculationServiceMessageQueueUri; }
        }
    }
}   