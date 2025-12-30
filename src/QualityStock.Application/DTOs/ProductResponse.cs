using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Application.DTOs
{
    public sealed record ProductResponse(
    Guid Id,
    Guid CategoryId,
    string Name,
    string Sku,
    string? Barcode,
    int? QualityControlIntervalDays);
}
