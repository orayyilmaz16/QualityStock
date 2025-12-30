using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QualityStock.Application.Abstractions;
using QualityStock.Domain.Enums;
using QualityStock.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Infrastructure.Automation
{
    public sealed class StockQualityAutomationWorker : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<StockQualityAutomationWorker> _logger;

        public StockQualityAutomationWorker(IServiceProvider sp, ILogger<StockQualityAutomationWorker> logger)
        {
            _sp = sp;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Basit periyodik çalıştırma: 10 dakikada bir
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<QualityStockDbContext>();
                    var notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
                    var clock = scope.ServiceProvider.GetRequiredService<IClock>();

                    var today = clock.TodayUtcDateOnly;
                    var expiringThresholdDays = 14;
                    var lowStockThreshold = 10;

                    // 1) SKT yaklaşanlar
                    var expiring = await db.StockBatches
                        .AsNoTracking()
                        .Include(x => x.Product)
                        .Where(x => x.ExpirationDate != null)
                        .Where(x => x.ExpirationDate.Value <= today.AddDays(expiringThresholdDays))
                        .Where(x => x.QuantityOnHand > 0)
                        .ToListAsync(stoppingToken);

                    foreach (var b in expiring)
                    {
                        await notifier.NotifyAsync(
                            "EXPIRY",
                            $"Product={b.Product.Sku} Lot={b.LotNumber} SKT={b.ExpirationDate} Qty={b.QuantityOnHand}",
                            stoppingToken);
                    }

                    // 2) Düşük stok (available)
                    var lowStock = await db.StockBatches
                        .AsNoTracking()
                        .Include(x => x.Product)
                        .Where(x => (x.QuantityOnHand - x.ReservedQuantity) < lowStockThreshold)
                        .Where(x => x.QuantityOnHand > 0)
                        .ToListAsync(stoppingToken);

                    foreach (var b in lowStock)
                    {
                        await notifier.NotifyAsync(
                            "LOW_STOCK",
                            $"Product={b.Product.Sku} Lot={b.LotNumber} Available={(b.QuantityOnHand - b.ReservedQuantity)}",
                            stoppingToken);
                    }

                    // 3) QC otomasyonu: QC periyodu dolmuş ve hâlâ Passed değilse uyar
                    // Basit kural: Ürün QC interval varsa ve batch ProductionDate varsa;
                    // ProductionDate + interval <= today ise ve status Pending/Unknown ise uyar.
                    var dueQc = await db.StockBatches
                        .AsNoTracking()
                        .Include(x => x.Product)
                        .Where(x => x.Product.QualityControlIntervalDays != null)
                        .Where(x => x.ProductionDate != null)
                        .Where(x => x.QualityStatus == QualityStatus.Pending || x.QualityStatus == QualityStatus.Unknown)
                        .ToListAsync(stoppingToken);

                    foreach (var b in dueQc)
                    {
                        var dueDate = b.ProductionDate!.Value.AddDays(b.Product.QualityControlIntervalDays!.Value);
                        if (dueDate <= today)
                        {
                            await notifier.NotifyAsync(
                                "QC_DUE",
                                $"Product={b.Product.Sku} Lot={b.LotNumber} QC Due since={dueDate}",
                                stoppingToken);
                        }
                    }

                    _logger.LogInformation("Automation cycle completed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Automation cycle failed.");
                }

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}
