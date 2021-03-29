using BevCapital.StockPrices.Data.Context;
using BevCapital.StockPrices.Data.Repositories;
using BevCapital.StockPrices.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BevCapital.StockPrices.Infra.ServiceExtensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddAppDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StockPriceContext>(opts =>
            {
                opts.UseMySql(configuration.GetConnectionString("SqlCNN"));
                opts.AddXRayInterceptor(true);
            });

            services.AddSingleton<IStockPriceRepositoryAsync, StockPriceRepositoryAsync>();

            return services;
        }
    }
}
