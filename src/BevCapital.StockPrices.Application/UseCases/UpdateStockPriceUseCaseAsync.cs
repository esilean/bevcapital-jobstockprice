using Amazon.XRay.Recorder.Core;
using BevCapital.StockPrices.Application.Gateways.PricesServices;
using BevCapital.StockPrices.Application.UseCases.Gateways;
using BevCapital.StockPrices.Domain.Constants;
using BevCapital.StockPrices.Domain.Entities;
using BevCapital.StockPrices.Domain.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.StockPrices.Application.UseCases
{
    public class UpdateStockPriceUseCaseAsync : IUpdateStockPriceUseCaseAsync
    {
        private readonly ILogger<UpdateStockPriceUseCaseAsync> _logger;
        private readonly IStockPriceRepositoryAsync _stockPriceRepositoryAsync;
        private readonly IFinnhubService _finnhubService;
        private readonly IDistributedCache _distributedCache;

        public UpdateStockPriceUseCaseAsync(ILogger<UpdateStockPriceUseCaseAsync> logger,
                                            IStockPriceRepositoryAsync stockPriceRepositoryAsync,
                                            IFinnhubService finnhubService,
                                            IDistributedCache distributedCache)
        {
            _logger = logger;
            _stockPriceRepositoryAsync = stockPriceRepositoryAsync;
            _finnhubService = finnhubService;
            _distributedCache = distributedCache;
        }

        public async Task UpdateStockPrices(CancellationToken stoppingToken)
        {
            try
            {
                AWSXRayRecorder.Instance.BeginSegment("UpdateStockPrices-Job");

                var stockPrices = await _stockPriceRepositoryAsync.GetAll();

                var shouldUpdate = await UpdateStockPrice(stockPrices);
                if (shouldUpdate)
                {
                    await _stockPriceRepositoryAsync.SaveChanges();
                    await _distributedCache.RemoveAsync(CacheKeys.LIST_ALL_STOCKS);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                AWSXRayRecorder.Instance.AddException(ex);
            }
            finally
            {
                AWSXRayRecorder.Instance.EndSegment();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockPrices"></param>
        /// <returns></returns>
        private async Task<bool> UpdateStockPrice(List<StockPrice> stockPrices)
        {
            var shouldUpdate = false;
            foreach (var sp in stockPrices)
            {
                var finnhubModel = await _finnhubService.GetQuote(sp.Id);
                if (finnhubModel == null)
                    continue;

                if (sp.LatestPrice != finnhubModel.CurrentPrice)
                {
                    var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    dtDateTime = dtDateTime.AddSeconds(finnhubModel.UnixTimestamp).ToUniversalTime();

                    var changePercent = finnhubModel.PreviousClose > 0 ?
                                            (finnhubModel.CurrentPrice / finnhubModel.PreviousClose - 1) * 100.0M : 0M;

                    sp.SetPrice(finnhubModel.Open,
                                finnhubModel.High,
                                finnhubModel.Low,
                                finnhubModel.CurrentPrice,
                                dtDateTime,
                                finnhubModel.PreviousClose,
                                changePercent);

                    shouldUpdate = true;
                }
            }

            return shouldUpdate;
        }

    }
}
