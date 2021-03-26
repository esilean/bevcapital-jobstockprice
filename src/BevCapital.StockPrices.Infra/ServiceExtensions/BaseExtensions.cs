using BevCapital.StockPrices.Application.UseCases;
using BevCapital.StockPrices.Application.UseCases.Gateways;
using BevCapital.StockPrices.Background.Services;
using BevCapital.StockPrices.Infra.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BevCapital.StockPrices.Infra.ServiceExtensions
{
    public static class BaseExtensions
    {
        public static IServiceCollection ConfigureCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));
            services.Configure<UpdateStockPriceBackgroundServiceSettings>(configuration.GetSection("UpdateStockPriceBackgroundServiceSettings"));

            services.AddSingleton<IUpdateStockPriceUseCaseAsync, UpdateStockPriceUseCaseAsync>();
            services.AddHostedService<UpdateStockPriceBackgroundService>();

            return services;
        }
    }
}
