using QualityStock.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Infrastructure.Persistence
{
    public sealed class Repository<T> : IRepository<T> where T : class
    {
        private readonly QualityStockDbContext _db;

        public Repository(QualityStockDbContext db) => _db = db;

        public Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => _db.Set<T>().FindAsync([id], ct).AsTask();

        public Task AddAsync(T entity, CancellationToken ct = default)
            => _db.Set<T>().AddAsync(entity, ct).AsTask();

        public void Update(T entity) => _db.Set<T>().Update(entity);
        public void Remove(T entity) => _db.Set<T>().Remove(entity);
    }
}
