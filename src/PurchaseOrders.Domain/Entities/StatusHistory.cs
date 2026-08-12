using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Domain.Enums;

namespace PurchaseOrders.Domain.Entities
{
    public class StatusHistory
    {
        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public OrderStatus PreviousStatus { get; set; }
        public OrderStatus NewStatus { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public string? Comment { get; set; }
    }
}
