using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Domain.Enums;

namespace PurchaseOrders.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        public bool IsActive { get; set; } = true;

        public int? SupervisorId { get; set; }
        public User? Supervisor { get; set; }
        public ICollection<User> Subordinates { get; set; } = new List<User>();

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    }
}
