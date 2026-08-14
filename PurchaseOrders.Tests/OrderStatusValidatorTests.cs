using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Application.Utils;
using PurchaseOrders.Domain.Enums;

namespace PurchaseOrders.Tests
{
    public class OrderStatusValidatorTests
    {

        [Theory]
        [InlineData(OrderStatus.Created, OrderStatus.Approved)]
        [InlineData(OrderStatus.Created, OrderStatus.Rejected)]
        [InlineData(OrderStatus.Created, OrderStatus.Cancelled)]
        [InlineData(OrderStatus.Approved, OrderStatus.Sent)]
        [InlineData(OrderStatus.Approved, OrderStatus.Cancelled)]
        [InlineData(OrderStatus.Sent, OrderStatus.Delivered)]
        public void ValidTransitions_ReturnTrue(OrderStatus from, OrderStatus to)
        {
            var result = OrderStatusValidator.IsValidTransition(from, to);
            Assert.True(result);
        }

        [Theory]
        [InlineData(OrderStatus.Rejected, OrderStatus.Sent)]
        [InlineData(OrderStatus.Delivered, OrderStatus.Cancelled)]
        [InlineData(OrderStatus.Created, OrderStatus.Sent)]
        [InlineData(OrderStatus.Sent, OrderStatus.Cancelled)]
        public void InvalidTransitions_ReturnFalse(OrderStatus from, OrderStatus to)
        {
            var result = OrderStatusValidator.IsValidTransition(from, to);
            Assert.False(result);
        }
    }
}
