using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Domain.Enums;

namespace PurchaseOrders.Domain.Entities
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;

        public int EmployeeId { get; set; }
        public User Employee { get; set; } = null!;

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        public OrderStatus Status { get; set; } = OrderStatus.Created;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string? RejectionReason { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public ICollection<StatusHistory> StatusHistory { get; set; } = new List<StatusHistory>();
    }
}
