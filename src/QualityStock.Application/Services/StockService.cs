using QualityStock.Application.Abstractions;
using QualityStock.Application.DTOs;
using QualityStock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Application.Services
{
    public sealed class StockService
    {
        private readonly IRepository<StockBatch> _batches;
        private readonly IUnitOfWork _uow;

        public StockService(IRepository<StockBatch> batches, IUnitOfWork uow)
        {
            _batches = batches;
            _uow = uow;
        }

        public async Task<StockBatchResponse> CreateBatchAsync(CreateStockBatchRequest req, CancellationToken ct = default)
        {
            var batch = new StockBatch(req.ProductId, req.LotNumber, req.Quantity, req.ProductionDate, req.ExpirationDate);
            await _batches.AddAsync(batch, ct);
            await _uow.SaveChangesAsync(ct);

            return new StockBatchResponse(
                batch.Id,
                batch.ProductId,
                batch.LotNumber,
                batch.QuantityOnHand,
                batch.ReservedQuantity,
                batch.ProductionDate,
                batch.ExpirationDate,
                batch.AvailableQuantity,
                batch.QualityStatus.ToString());
        }
    }
}
