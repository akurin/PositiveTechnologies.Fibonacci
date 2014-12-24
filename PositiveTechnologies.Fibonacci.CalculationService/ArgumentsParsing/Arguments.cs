using System;

namespace PositiveTechnologies.Fibonacci.CalculationService.ArgumentsParsing
{
    /// <summary>
    /// Represents command-line arguments.
    /// </summary>
    internal sealed class Arguments
    {
        private readonly string _calculationServiceWebApiUri;
        private readonly string _calculationServiceMessageQueueUri;

        private Arguments(
            string calculationServiceWebApiUri,
            string calculationServiceMessageQueueUri)
        {
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

            if (args.Length != 2)
                throw new ArgumentsParseException("Wrong number of command-line arguments");

            var calculationServiceWebApiUri = ParseCalculationServiceWebApiUri(args[0]);
            var calculationServiceMessageQueueUri = ParseCalculationServiceMessageQueueUri(args[1]);

            return new Arguments(
                calculationServiceWebApiUri,
                calculationServiceMessageQueueUri);
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