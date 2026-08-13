using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Application.Dtos;

namespace PurchaseOrders.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto?> CreateAsync(CreateProductDto dto);
        Task<bool> UpdateAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
