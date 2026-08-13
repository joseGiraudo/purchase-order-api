using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PurchaseOrders.Domain.Entities;
using PurchaseOrders.Domain.Interfaces;
using PurchaseOrders.Infrastructure.Persistence;

namespace PurchaseOrders.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                        .Include(p => p.Supplier)
                        .Where(p => p.IsActive)
                        .AsNoTracking()
                        .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                        .Include(p => p.Supplier)
                        .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateAsync(Product product)
        {
            product.IsActive = false;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
