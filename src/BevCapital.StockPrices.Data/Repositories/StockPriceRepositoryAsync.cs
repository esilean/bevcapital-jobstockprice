using BevCapital.StockPrices.Data.Context;
using BevCapital.StockPrices.Domain.Entities;
using BevCapital.StockPrices.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BevCapital.StockPrices.Data.Repositories
{
    public class StockPriceRepositoryAsync : IStockPriceRepositoryAsync
    {
        private readonly StockPriceContext _stockPriceContext;

        public StockPriceRepositoryAsync(IServiceProvider serviceProvider)
        {
            IServiceScope scope = serviceProvider.CreateScope();
            _stockPriceContext = scope.ServiceProvider.GetService<StockPriceContext>();
        }

        public async Task<List<StockPrice>> GetAll()
        {
            return await _stockPriceContext.StockPrices.ToListAsync();
        }

        public async Task SaveChanges()
        {
            try
            {
                await _stockPriceContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is StockPrice)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            proposedValues[property] = proposedValue;
                        }

                        // Refresh original values to bypass next concurrency check
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _stockPriceContext.DisposeAsync();
        }
    }
}
