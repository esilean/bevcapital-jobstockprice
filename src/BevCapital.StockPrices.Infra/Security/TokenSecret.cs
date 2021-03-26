using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager.Model;
using BevCapital.StockPrices.Application.Gateways.Security;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BevCapital.StockPrices.Infra.Security
{
    public class TokenSecret : ITokenSecret
    {
        private readonly ILogger<TokenSecret> _logger;

        private readonly IAmazonSecretsManager _amazonSecretsManager;
        private SecretsManagerCache _secretsManagerCache;

        public TokenSecret(ILogger<TokenSecret> logger)
        {
            _logger = logger;

            _amazonSecretsManager = new AmazonSecretsManagerClient(RegionEndpoint.SAEast1);
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            try
            {
                string secret = "";
                if (_secretsManagerCache != null)
                    secret = await _secretsManagerCache.GetSecretString(secretName);

                if (string.IsNullOrEmpty(secret))
                {
                    var request = new GetSecretValueRequest
                    {
                        SecretId = secretName,
                        VersionStage = "AWSCURRENT"
                    };

                    var response = await _amazonSecretsManager.GetSecretValueAsync(request);
                    _secretsManagerCache = new SecretsManagerCache(_amazonSecretsManager);

                    secret = response.SecretString;
                }

                return secret;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, secretName);
                return "";
            }
        }

        public void Dispose()
        {
            _amazonSecretsManager.Dispose();
            _secretsManagerCache.Dispose();
        }
    }
}
