using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOrders.Application.Dtos
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => Quantity * UnitPrice; // prop calculada
    }

    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public string? RejectionReason { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();

        public List<StatusHistoryDto> StatusHistory { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreatePurchaseOrderDto
    {
        public int EmployeeId { get; set; }
        public int SupplierId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }

    public class ChangeOrderStatusDto
    {
        public string NewStatus { get; set; } = string.Empty;
        public int ChangedByUserId { get; set; }
        public string? Comment { get; set; } // obligatorio si NewStatus es "Rejected", ver nota abajo
    }

    public class StatusHistoryDto
    {
        public string PreviousStatus { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
        public string ChangedByName { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
        public string? Comment { get; set; }
    }
}
