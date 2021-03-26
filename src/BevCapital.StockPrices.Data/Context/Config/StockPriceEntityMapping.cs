﻿using BevCapital.StockPrices.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BevCapital.StockPrices.Data.Context.Config
{
    public class StockPriceEntityMapping : IEntityTypeConfiguration<StockPrice>
    {
        public void Configure(EntityTypeBuilder<StockPrice> builder)
        {
            builder.ToTable("StockPrice");

            builder.HasKey(r => r.Symbol);

            builder.Property(e => e.Open)
                .IsRequired();
            builder.Property(e => e.Close)
                .IsRequired();
            builder.Property(e => e.High)
                .IsRequired();
            builder.Property(e => e.Low)
                .IsRequired();
            builder.Property(e => e.LatestPrice)
                .IsRequired();
            builder.Property(e => e.LatestPriceTime)
                .IsRequired();
            builder.Property(e => e.DelayedPrice)
                .IsRequired();
            builder.Property(e => e.DelayedPriceTime)
                .IsRequired();
            builder.Property(e => e.PreviousClosePrice)
                .IsRequired();
            builder.Property(e => e.ChangePercent);
            builder.Property(e => e.CreatedAt)
                .IsRequired();
            builder.Property(e => e.UpdatedAt)
                .IsRequired();

            builder.Property(e => e.RowVersion)
                .IsRowVersion();

            builder.Ignore(e => e.Valid);
            builder.Ignore(e => e.Invalid);
            builder.Ignore(e => e.ValidationResult);
        }
    }
}