using BevCapital.StockPrices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BevCapital.StockPrices.Domain.Repositories
{
    public interface IStockPriceRepositoryAsync : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<StockPrice>> GetAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task SaveChanges();

    }
}
