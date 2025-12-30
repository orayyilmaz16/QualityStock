using QualityStock.Domain.Common;
using QualityStock.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Domain.Entities
{
    public sealed class StockBatch : AuditableEntity
    {
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; } = default!;

        public string LotNumber { get; private set; } = default!;
        public DateOnly? ProductionDate { get; private set; }
        public DateOnly? ExpirationDate { get; private set; } // SKT
        public int QuantityOnHand { get; private set; }
        public int ReservedQuantity { get; private set; }

        public QualityStatus QualityStatus { get; private set; } = QualityStatus.Pending;

        private StockBatch() { } // EF

        public StockBatch(Guid productId, string lotNumber, int quantity, DateOnly? productionDate, DateOnly? expirationDate)
        {
            ProductId = productId;
            SetLotNumber(lotNumber);
            SetDates(productionDate, expirationDate);
            AdjustQuantity(quantity);
        }

        public int AvailableQuantity => QuantityOnHand - ReservedQuantity;

        public void SetLotNumber(string lotNumber)
        {
            if (string.IsNullOrWhiteSpace(lotNumber))
                throw new ArgumentException("Lot number cannot be empty.", nameof(lotNumber));
            LotNumber = lotNumber.Trim().ToUpperInvariant();
        }

        public void SetDates(DateOnly? productionDate, DateOnly? expirationDate)
        {
            if (productionDate is not null && expirationDate is not null && expirationDate < productionDate)
                throw new ArgumentException("ExpirationDate cannot be earlier than ProductionDate.");

            ProductionDate = productionDate;
            ExpirationDate = expirationDate;
        }

        public void AdjustQuantity(int delta)
        {
            var newQty = QuantityOnHand + delta;
            if (newQty < 0) throw new InvalidOperationException("Stock cannot go negative.");
            QuantityOnHand = newQty;
        }

        public void Reserve(int qty)
        {
            if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));
            if (qty > AvailableQuantity) throw new InvalidOperationException("Not enough available stock to reserve.");
            ReservedQuantity += qty;
        }

        public void Unreserve(int qty)
        {
            if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));
            if (qty > ReservedQuantity) throw new InvalidOperationException("Reserved quantity is insufficient.");
            ReservedQuantity -= qty;
        }

        public void SetQualityStatus(QualityStatus status) => QualityStatus = status;
    }
}
