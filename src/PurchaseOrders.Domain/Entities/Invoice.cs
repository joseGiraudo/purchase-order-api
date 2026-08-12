using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOrders.Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public string InvoiceNumber { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
