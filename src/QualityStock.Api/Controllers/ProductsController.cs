using Microsoft.AspNetCore.Mvc;
using QualityStock.Application.DTOs;
using QualityStock.Application.Services;

namespace QualityStock.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly ProductService _service;
        public ProductsController(ProductService service) => _service = service;

        [HttpPost]
        public async Task<ActionResult<ProductResponse>> Create(CreateProductRequest req, CancellationToken ct)
        {
            var created = await _service.CreateAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:guid}")]
        public ActionResult GetById(Guid id) => Ok(new { id, note = "GetById için query/repository genişletilecek." });
    }
}
