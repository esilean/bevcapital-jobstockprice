using BevCapital.StockPrices.Background.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BevCapital.StockPrices.Infra.ServiceExtensions
{
    public static class BackgoundServicesExtensions
    {
        public static IServiceCollection AddAppBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<UpdateStockPriceBackgroundService>();

            return services;
        }
    }
}
