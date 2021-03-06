using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BevCapital.StockPrices.Infra.ServiceExtensions
{
    public static class DistributedCacheExtensions
    {
        public static IServiceCollection AddAppDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheCNN = configuration.GetConnectionString("CacheCNN");
            if (string.IsNullOrWhiteSpace(cacheCNN))
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = cacheCNN;
                    options.InstanceName = "Stocks:";
                });
            }

            return services;
        }
    }
}
