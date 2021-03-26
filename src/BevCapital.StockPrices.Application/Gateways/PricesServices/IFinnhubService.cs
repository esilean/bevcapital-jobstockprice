using BevCapital.StockPrices.Application.Gateways.PricesServices.Model;
using System.Threading.Tasks;

namespace BevCapital.StockPrices.Application.Gateways.PricesServices
{
    public interface IFinnhubService
    {
        Task<FinnhubModel> GetQuote(string symbol);
    }
}
