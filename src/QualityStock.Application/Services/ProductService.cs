using QualityStock.Application.Abstractions;
using QualityStock.Application.DTOs;
using QualityStock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Application.Services
{
    public sealed class ProductService
    {
        private readonly IRepository<Product> _products;
        private readonly IUnitOfWork _uow;

        public ProductService(IRepository<Product> products, IUnitOfWork uow)
        {
            _products = products;
            _uow = uow;
        }

        public async Task<ProductResponse> CreateAsync(CreateProductRequest req, CancellationToken ct = default)
        {
            var product = new Product(req.CategoryId, req.Name, req.Sku, req.Barcode, req.QualityControlIntervalDays);
            await _products.AddAsync(product, ct);
            await _uow.SaveChangesAsync(ct);

            return new ProductResponse(product.Id, product.CategoryId, product.Name, product.Sku, product.Barcode, product.QualityControlIntervalDays);
        }
    }
}
