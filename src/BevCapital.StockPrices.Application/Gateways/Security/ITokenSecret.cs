using System;
using System.Threading.Tasks;

namespace BevCapital.StockPrices.Application.Gateways.Security
{
    public interface ITokenSecret : IDisposable
    {
        Task<string> GetSecretAsync(string secretName);
    }
}
