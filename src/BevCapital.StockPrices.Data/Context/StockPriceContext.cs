using BevCapital.StockPrices.Data.Context.Config;
using BevCapital.StockPrices.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BevCapital.StockPrices.Data.Context
{

    public class StockPriceContext : DbContext
    {
        public StockPriceContext(DbContextOptions<StockPriceContext> options)
                : base(options)
        { }

        public DbSet<StockPrice> StockPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new StockPriceEntityMapping());
        }
    }
}
