using Amazon.XRay.Recorder.Handlers.System.Net;
using BevCapital.StockPrices.Application.Gateways.PricesServices;
using BevCapital.StockPrices.Domain.Constants;
using BevCapital.StockPrices.Infra.PricesServices.Finnhub;
using BevCapital.StockPrices.Infra.ServiceExtensions.HttpMessageHandler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace BevCapital.StockPrices.Infra.ServiceExtensions
{

    public static class HttpFactoriesExtensions
    {
        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var finnhubService = configuration.GetSection(HttpServices.FinnhubServiceName);

            services.AddTransient<FinnhubTokenMessageHandler>();
            services.AddHttpClient(HttpServices.FinnhubServiceName, client =>
            {
                client.BaseAddress = new Uri($"{finnhubService["BaseAddress"]}/");
                client.Timeout = TimeSpan.FromSeconds(20);
            })
            .AddHttpMessageHandler<FinnhubTokenMessageHandler>()
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientXRayTracingHandler(new HttpClientHandler());
            });

            services.AddSingleton<IFinnhubService, FinnhubService>();

            return services;
        }
    }
}
