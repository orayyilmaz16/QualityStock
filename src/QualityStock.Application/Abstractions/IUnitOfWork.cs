using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Application.Abstractions
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
