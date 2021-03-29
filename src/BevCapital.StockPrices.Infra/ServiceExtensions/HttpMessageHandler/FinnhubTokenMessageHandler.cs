using BevCapital.StockPrices.Application.Gateways.Security;
using BevCapital.StockPrices.Infra.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.StockPrices.Infra.ServiceExtensions.HttpMessageHandler
{
    public class FinnhubTokenMessageHandler : DelegatingHandler
    {

        private readonly ILogger<FinnhubTokenMessageHandler> _logger;
        private readonly ITokenSecret _tokenSecret;
        private readonly string _finnhubToken;

        public FinnhubTokenMessageHandler(ILogger<FinnhubTokenMessageHandler> logger,
                                          ITokenSecret tokenSecret,
                                          IOptions<TokenSettings> options)
        {
            _logger = logger;
            _tokenSecret = tokenSecret;
            _finnhubToken = options.Value.FinnhubToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var awsTokenSecret = await _tokenSecret.GetSecretAsync(_finnhubToken);
            if (!string.IsNullOrWhiteSpace(awsTokenSecret))
            {
                var finnhubAwsModel = JsonConvert.DeserializeObject<FinnhubAwsModel>(awsTokenSecret);
                request.Headers.Add("X-Finnhub-Token", finnhubAwsModel.FinnhubToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }

    }
}
