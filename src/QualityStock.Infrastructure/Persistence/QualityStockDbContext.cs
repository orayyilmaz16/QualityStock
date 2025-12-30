using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using QualityStock.Domain.Common;
using QualityStock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Infrastructure.Persistence
{
    public sealed class QualityStockDbContext : DbContext
    {
        public QualityStockDbContext(DbContextOptions<QualityStockDbContext> options) : base(options) { }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<StockBatch> StockBatches => Set<StockBatch>();
        public DbSet<QualityInspection> QualityInspections => Set<QualityInspection>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).HasMaxLength(200).IsRequired();
                b.Property(x => x.Description).HasMaxLength(1000);
            });

            modelBuilder.Entity<Product>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).HasMaxLength(250).IsRequired();
                b.Property(x => x.Sku).HasMaxLength(80).IsRequired();
                b.HasIndex(x => x.Sku).IsUnique();
                b.Property(x => x.Barcode).HasMaxLength(80);

                b.HasOne(x => x.Category)
                 .WithMany()
                 .HasForeignKey(x => x.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StockBatch>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.LotNumber).HasMaxLength(80).IsRequired();
                b.HasIndex(x => new { x.ProductId, x.LotNumber }).IsUnique();

                b.Property(x => x.QuantityOnHand).IsRequired();
                b.Property(x => x.ReservedQuantity).IsRequired();

                b.HasOne(x => x.Product)
                 .WithMany()
                 .HasForeignKey(x => x.ProductId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<QualityInspection>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.InspectedBy).HasMaxLength(200).IsRequired();
                b.Property(x => x.Notes).HasMaxLength(2000);

                b.HasOne(x => x.StockBatch)
                 .WithMany()
                 .HasForeignKey(x => x.StockBatchId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTimeOffset.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = now;

                if (entry.State == EntityState.Modified)
                    entry.Entity.UpdatedAt = now;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
