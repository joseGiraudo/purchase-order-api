using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Application.Dtos;

namespace PurchaseOrders.Application.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<List<PurchaseOrderDto>> GetAllAsync(int currentUserId);
        Task<PurchaseOrderDto?> GetByIdAsync(int id);
        Task<PurchaseOrderDto?> CreateAsync(CreatePurchaseOrderDto dto);
        Task<PurchaseOrderDto?> ChangeStatusAsync(int id, ChangeOrderStatusDto dto);
    }
}
