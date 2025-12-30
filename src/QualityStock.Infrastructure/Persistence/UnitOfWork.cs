using QualityStock.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly QualityStockDbContext _db;
        public UnitOfWork(QualityStockDbContext db) => _db = db;
        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
    }
}
