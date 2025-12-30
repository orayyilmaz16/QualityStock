using Microsoft.AspNetCore.Mvc;
using QualityStock.Application.DTOs;
using QualityStock.Application.Services;

namespace QualityStock.Api.Controllers
{
    [ApiController]
    [Route("api/stock-batches")]
    public sealed class StockBatchesController : ControllerBase
    {
        private readonly StockService _service;
        public StockBatchesController(StockService service) => _service = service;

        [HttpPost]
        public async Task<ActionResult<StockBatchResponse>> Create(CreateStockBatchRequest req, CancellationToken ct)
        {
            var created = await _service.CreateBatchAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:guid}")]
        public ActionResult GetById(Guid id) => Ok(new { id, note = "GetById için query/repository genişletilecek." });
    }
}
