using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.StockPrices.Application.UseCases.Gateways
{
    public interface IUpdateStockPriceUseCaseAsync
    {
        Task UpdateStockPrices(CancellationToken stoppingToken);
    }
}
