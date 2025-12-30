using QualityStock.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Domain.Entities
{
    public sealed class Product : AuditableEntity
    {
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = default!;

        public string Name { get; private set; } = default!;
        public string Sku { get; private set; } = default!;
        public string? Barcode { get; private set; }

        // QC otomasyonu için periyod (ör: her 30 günde bir kontrol)
        public int? QualityControlIntervalDays { get; private set; }

        private Product() { } // EF

        public Product(Guid categoryId, string name, string sku, string? barcode = null, int? qcIntervalDays = null)
        {
            CategoryId = categoryId;
            SetName(name);
            SetSku(sku);
            Barcode = barcode?.Trim();
            QualityControlIntervalDays = qcIntervalDays;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.", nameof(name));

            Name = name.Trim();
        }

        public void SetSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("SKU cannot be empty.", nameof(sku));

            Sku = sku.Trim().ToUpperInvariant();
        }

        public void SetQualityControlIntervalDays(int? days)
        {
            if (days is not null && days <= 0)
                throw new ArgumentOutOfRangeException(nameof(days), "QC interval must be positive.");
            QualityControlIntervalDays = days;
        }
    }
}
