using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Application.Dtos;
using PurchaseOrders.Application.Interfaces;
using PurchaseOrders.Domain.Entities;
using PurchaseOrders.Domain.Interfaces;

namespace PurchaseOrders.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;

        public ProductService(IProductRepository productRepository, ISupplierRepository supplierRepository)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToDto).ToList();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : MapToDto(product);
        }

        public async Task<ProductDto?> CreateAsync(CreateProductDto dto)
        {
            var supplier = await _supplierRepository.GetByIdAsync(dto.SupplierId);
            if (supplier is null)
            {
                return null; // el controller traduce esto a un 400
            }
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                ReferencePrice = dto.ReferencePrice,
                SupplierId = dto.SupplierId
            };
            var createdProduct = await _productRepository.AddAsync(product);
            return MapToDto(createdProduct);
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.ReferencePrice = dto.ReferencePrice;
            await _productRepository.UpdateAsync(product);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }
            await _productRepository.DeactivateAsync(product);
            return true;
        }




        // metodo privado
        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ReferencePrice = product.ReferencePrice,
                SupplierId = product.SupplierId,
                IsActive = product.IsActive,
            };
        }

    }
}
