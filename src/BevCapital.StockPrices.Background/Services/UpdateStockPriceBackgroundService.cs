using BevCapital.StockPrices.Application.UseCases.Gateways;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.StockPrices.Background.Services
{
    public class UpdateStockPriceBackgroundService : BackgroundService
    {
        private readonly ILogger<UpdateStockPriceBackgroundService> _logger;
        private readonly IUpdateStockPriceUseCaseAsync _updateStockPriceUseCaseAsync;
        private readonly UpdateStockPriceBackgroundServiceSettings _updateStockPriceBackgroundServiceSettings;

        public UpdateStockPriceBackgroundService(IOptions<UpdateStockPriceBackgroundServiceSettings> options,
                                                 ILogger<UpdateStockPriceBackgroundService> logger,
                                                 IUpdateStockPriceUseCaseAsync updateStockPriceUseCaseAsync)
        {
            _updateStockPriceUseCaseAsync = updateStockPriceUseCaseAsync;
            _logger = logger;
            _updateStockPriceBackgroundServiceSettings = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting UpdateStockPriceBackgroundService...");

            //COLD START
            await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await _updateStockPriceUseCaseAsync.UpdateStockPrices(stoppingToken);

                TimeSpan timespan = TimeSpan.FromSeconds(_updateStockPriceBackgroundServiceSettings.IntervalInSeconds);
                await Task.Delay(timespan, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping UpdateStockPriceBackgroundService...");
            return base.StopAsync(cancellationToken);
        }
    }
}
