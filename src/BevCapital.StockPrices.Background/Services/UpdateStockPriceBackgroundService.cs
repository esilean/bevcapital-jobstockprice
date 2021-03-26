using BevCapital.StockPrices.Application.UseCases.Gateways;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.StockPrices.Background.Services
{
    public class UpdateStockPriceBackgroundService : BackgroundService
    {
        private readonly IUpdateStockPriceUseCaseAsync _updateStockPriceUseCaseAsync;
        private readonly UpdateStockPriceBackgroundServiceSettings _updateStockPriceBackgroundServiceSettings;

        public UpdateStockPriceBackgroundService(IOptions<UpdateStockPriceBackgroundServiceSettings> options,
                                                 IUpdateStockPriceUseCaseAsync updateStockPriceUseCaseAsync)
        {
            _updateStockPriceUseCaseAsync = updateStockPriceUseCaseAsync;
            _updateStockPriceBackgroundServiceSettings = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _updateStockPriceUseCaseAsync.UpdateStockPrices(stoppingToken);

                TimeSpan timespan = TimeSpan.FromSeconds(_updateStockPriceBackgroundServiceSettings.IntervalInSeconds);
                await Task.Delay(timespan, stoppingToken);
            }
        }
    }
}
