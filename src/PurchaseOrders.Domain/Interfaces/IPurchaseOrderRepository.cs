using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Domain.Entities;

namespace PurchaseOrders.Domain.Interfaces
{
    public interface IPurchaseOrderRepository
    {
        Task<List<PurchaseOrder>> GetAllAsync(int? employeeId, int? supervisorId);
        Task<PurchaseOrder?> GetByIdAsync(int id);
        Task<PurchaseOrder> AddAsync(PurchaseOrder order);
        Task UpdateAsync(PurchaseOrder order);
        Task AddStatusHistoryAsync(StatusHistory history);
    }
}
