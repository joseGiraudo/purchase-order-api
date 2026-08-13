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
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _context;

        public SupplierRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Supplier> AddAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task DeactivateAsync(Supplier supplier)
        {
            supplier.IsActive = false;
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers.AsNoTracking().ToListAsync();
            // no necesita trackear cambios sobre esas entidades porque solo las estamos leyendo, no modificando.
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }
    }
}
