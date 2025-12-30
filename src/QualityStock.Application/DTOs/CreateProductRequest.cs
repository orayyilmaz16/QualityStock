using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Application.DTOs
{
    public sealed record CreateProductRequest(
     Guid CategoryId,
     string Name,
     string Sku,
     string? Barcode,
     int? QualityControlIntervalDays);

}
