using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Domain.Enums;

namespace PurchaseOrders.Application.Utils
{
    public static class OrderStatusValidator
    {
        private static readonly Dictionary<OrderStatus, OrderStatus[]> ValidTransitions = new()
        {
            [OrderStatus.Created] = new[] { OrderStatus.Approved, OrderStatus.Rejected, OrderStatus.Cancelled },
            [OrderStatus.Approved] = new[] { OrderStatus.Sent, OrderStatus.Cancelled },
            [OrderStatus.Sent] = new[] { OrderStatus.Delivered },
            [OrderStatus.Delivered] = Array.Empty<OrderStatus>(),
            [OrderStatus.Rejected] = Array.Empty<OrderStatus>(),
            [OrderStatus.Cancelled] = Array.Empty<OrderStatus>()
        };

        public static bool IsValidTransition(OrderStatus from, OrderStatus to)
            => ValidTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);
    }
}
