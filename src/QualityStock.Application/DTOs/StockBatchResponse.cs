using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Application.DTOs
{
    public sealed record StockBatchResponse(
    Guid Id,
    Guid ProductId,
    string LotNumber,
    int QuantityOnHand,
    int ReservedQuantity,
    DateOnly? ProductionDate,
    DateOnly? ExpirationDate,
    int AvailableQuantity,
    string QualityStatus);
}
