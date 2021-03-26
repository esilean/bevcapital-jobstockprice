using Amazon.XRay.Recorder.Core;
using BevCapital.StockPrices.Application.Gateways.PricesServices;
using BevCapital.StockPrices.Application.Gateways.PricesServices.Model;
using BevCapital.StockPrices.Domain.Constants;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BevCapital.StockPrices.Infra.PricesServices.Finnhub
{
    public class FinnhubService : IFinnhubService
    {
        private readonly AsyncRetryPolicy<HttpResponseMessage> TransientErrorRetryPolicy;
        private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));

        private readonly ILogger<FinnhubService> _logger;
        private readonly HttpClient _client;

        public FinnhubService(ILogger<FinnhubService> logger,
                              IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient(HttpServices.FinnhubServiceName);

            TransientErrorRetryPolicy = Policy
                .HandleResult<HttpResponseMessage>
                (message => ((int)message.StatusCode == 400 || (int)message.StatusCode >= 500))
                .WaitAndRetryAsync(1, retryAttempt =>
                {
                    _logger.LogWarning(1, "Trying to request again... RetryAttempt {retryAttempt}", retryAttempt);
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                });
        }


        public async Task<FinnhubModel> GetQuote(string symbol)
        {
            try
            {
                AWSXRayRecorder.Instance.BeginSegment("FinnhubService");

                if (CircuitBreakerPolicy.CircuitState == CircuitState.Open)
                    throw new BrokenCircuitException("Finnhub API is down!");

                var response = await CircuitBreakerPolicy
                                                    .ExecuteAsync(() =>
                                                        TransientErrorRetryPolicy
                                                        .ExecuteAsync(() => _client.GetAsync($"quote?symbol={symbol}")));

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error retrieving quote from Finnhub");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var finnhubModel = JsonConvert.DeserializeObject<FinnhubModel>(content);

                return finnhubModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                AWSXRayRecorder.Instance.AddException(ex);
                return null;
            }
            finally
            {
                AWSXRayRecorder.Instance.EndSegment();
            }
        }
    }
}
