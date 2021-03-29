using BevCapital.StockPrices.Application.UseCases;
using BevCapital.StockPrices.Application.UseCases.Gateways;
using BevCapital.StockPrices.Background.Services;
using BevCapital.StockPrices.Infra.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BevCapital.StockPrices.Infra.ServiceExtensions
{
    public static class CoreExtensions
    {
        public static void AddSecrets(this IServiceCollection _, IConfiguration configuration)
        {
            var secretsJson = File.ReadAllText(@"appsecrets.json");
            var secrets = JsonConvert.DeserializeObject<IDictionary<string, string>>(secretsJson);

            foreach (var secret in secrets)
            {
                var values = secret.Value.Split("::");
                foreach (var value in values)
                {
                    configuration[secret.Key] = configuration[secret.Key]?.Replace(value, Environment.GetEnvironmentVariable(value));
                }
            }
        }

        public static IServiceCollection AddAppCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));
            services.Configure<UpdateStockPriceBackgroundServiceSettings>(configuration.GetSection("UpdateStockPriceBackgroundServiceSettings"));

            services.AddSingleton<IUpdateStockPriceUseCaseAsync, UpdateStockPriceUseCaseAsync>();
            services.AddHostedService<UpdateStockPriceBackgroundService>();

            return services;
        }
    }
}
