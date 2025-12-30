using QualityStock.Domain.Common;
using QualityStock.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Domain.Entities
{
    public sealed class QualityInspection : AuditableEntity
    {
        public Guid StockBatchId { get; private set; }
        public StockBatch StockBatch { get; private set; } = default!;

        public DateTimeOffset InspectedAt { get; private set; }
        public string InspectedBy { get; private set; } = default!;
        public QualityStatus Result { get; private set; }
        public string? Notes { get; private set; }

        private QualityInspection() { } // EF

        public QualityInspection(Guid stockBatchId, DateTimeOffset inspectedAt, string inspectedBy, QualityStatus result, string? notes)
        {
            StockBatchId = stockBatchId;
            InspectedAt = inspectedAt;
            InspectedBy = string.IsNullOrWhiteSpace(inspectedBy) ? throw new ArgumentException("Inspector required.") : inspectedBy.Trim();
            Result = result;
            Notes = notes?.Trim();
        }
    }
}
