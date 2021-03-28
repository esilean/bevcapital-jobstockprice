using BevCapital.StockPrices.Domain.Entities;
using System;
using Xunit;

namespace BevCapital.StockPrices.Domain.Tests.Entities
{
    public class StockPriceTests
    {
        [Fact(DisplayName = "It should create a stock successfully")]
        public void StockPrice_ShouldCreate_A_StockWithPrice()
        {
            // ARRANGE
            string symbol = "symbol";
            decimal open = 10;
            decimal close = 10;
            decimal high = 10;
            decimal low = 10;
            decimal latestPrice = 10;
            DateTime latestPriceTime = DateTime.Now;
            decimal delayedPrice = 10;
            DateTime delayedPriceTime = DateTime.Now;
            decimal previousClosePrice = 10;
            decimal changePercent = 10;

            // ACT
            var stockPrice = StockPrice.Create(symbol,
                                               open, close, high, low,
                                               latestPrice, latestPriceTime,
                                               delayedPrice, delayedPriceTime,
                                               previousClosePrice, changePercent);

            // ASSERT
            Assert.Equal(symbol, stockPrice.Id);
            Assert.Equal(open, stockPrice.Open);
            Assert.Equal(close, stockPrice.Close);
            Assert.Equal(high, stockPrice.High);
            Assert.Equal(low, stockPrice.Low);
            Assert.Equal(latestPrice, stockPrice.LatestPrice);
            Assert.Equal(latestPriceTime, stockPrice.LatestPriceTime);
            Assert.Equal(delayedPrice, stockPrice.DelayedPrice);
            Assert.Equal(delayedPriceTime, stockPrice.DelayedPriceTime);
            Assert.Equal(previousClosePrice, stockPrice.PreviousClosePrice);
            Assert.Equal(changePercent, stockPrice.ChangePercent);
        }

    }
}
