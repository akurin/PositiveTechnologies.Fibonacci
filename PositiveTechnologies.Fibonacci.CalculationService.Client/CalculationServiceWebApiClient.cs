using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp;

namespace PositiveTechnologies.Fibonacci.CalculationService.Client
{
    /// <summary>
    /// Represents Calculation Service Web API client.
    /// </summary>
    internal sealed class CalculationServiceWebApiClient
    {
        private readonly RestClient _restClient;

        public CalculationServiceWebApiClient(string uri)
        {
            if (uri == null) throw new ArgumentNullException("uri");

            _restClient = new RestClient(uri);
        }

        /// <summary>
        /// Performs POST-request to Calculation Service to start calculation of next Fibonacci number.
        /// </summary>
        /// <param name="fibonacciSequenceId"></param>
        /// <param name="currentNumber"></param>
        /// <returns></returns>
        public async Task CalculateNextAsync(string fibonacciSequenceId, long currentNumber)
        {
            var request = new RestRequest("fibonacciSequence/{id}", Method.POST);
            request.AddParameter("id", fibonacciSequenceId, ParameterType.UrlSegment);
            request.AddParameter("currentNumber", currentNumber, ParameterType.QueryString);

            var response = await _restClient.ExecuteTaskAsync(request);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                var message = string.Format("Error retrieving response, status: {0}", response.StatusDescription);
                throw new CalculationServiceRequestException(message);
            }
        }
    }
}