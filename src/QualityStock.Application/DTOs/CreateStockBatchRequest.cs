using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Application.DTOs
{
    public sealed record CreateStockBatchRequest(
    Guid ProductId,
    string LotNumber,
    int Quantity,
    DateOnly? ProductionDate,
    DateOnly? ExpirationDate);

}
