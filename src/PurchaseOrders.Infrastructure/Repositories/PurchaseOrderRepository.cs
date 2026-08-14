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
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly AppDbContext _context;

        public PurchaseOrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseOrder>> GetAllAsync() =>
        await _context.PurchaseOrders
            .Include(o => o.Employee)
            .Include(o => o.Supplier)
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .AsNoTracking()
            .ToListAsync();

        public async Task<PurchaseOrder?> GetByIdAsync(int id) =>
            await _context.PurchaseOrders
                .Include(o => o.Employee)
                .Include(o => o.Supplier)
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<PurchaseOrder> AddAsync(PurchaseOrder order)
        {
            _context.PurchaseOrders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateAsync(PurchaseOrder order)
        {
            _context.PurchaseOrders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task AddStatusHistoryAsync(StatusHistory history)
        {
            _context.StatusHistory.Add(history);
            await _context.SaveChangesAsync();
        }
    }
}
