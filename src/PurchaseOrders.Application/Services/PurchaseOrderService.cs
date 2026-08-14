using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Application.Dtos;
using PurchaseOrders.Application.Interfaces;
using PurchaseOrders.Application.Utils;
using PurchaseOrders.Domain.Entities;
using PurchaseOrders.Domain.Enums;
using PurchaseOrders.Domain.Interfaces;

namespace PurchaseOrders.Application.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;

        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository, IUserRepository userRepository, ISupplierRepository supplierRepository, IProductRepository productRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _userRepository = userRepository;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
        }

        public async Task<List<PurchaseOrderDto>> GetAllAsync()
        {
            var purchaseOrders = await _purchaseOrderRepository.GetAllAsync();

            return purchaseOrders.Select(ToDto).ToList();
        }

        public async Task<PurchaseOrderDto?> GetByIdAsync(int id)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(id);
            if (purchaseOrder == null) return null;

            return ToDto(purchaseOrder);
        }

        public async Task<PurchaseOrderDto?> CreateAsync(CreatePurchaseOrderDto dto)
        {
            // verifico existencia del empleado
            var employee = await _userRepository.GetByIdAsync(dto.EmployeeId);
            if (employee == null) return null;
            
            // verifico existencia del supplier
            var supplier = await _supplierRepository.GetByIdAsync(dto.SupplierId);
            if (supplier == null) return null;

            // verifico existencia de los productos
            if(dto.Items.Count < 1) return null;

            // Creo el detalle con los items
            var items = new List<OrderItem>();

            foreach (var itemDto in dto.Items)
            {
                if (itemDto.Quantity < 1) return null;

                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if(product == null) return null;

                // regla de negocio: el producto tiene que pertenecer al proveedor
                if (product.SupplierId != dto.SupplierId) return null;

                items.Add(new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.ReferencePrice // snapshot del precio
                });
            }

            var totalAmount = items.Sum(i => i.Quantity * i.UnitPrice);
            
            var order = new PurchaseOrder
            {
                Number = "------", // se reemplaza después de guardar
                EmployeeId = dto.EmployeeId,
                SupplierId = dto.SupplierId,
                Status = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Items = items
            };

            var created = await _purchaseOrderRepository.AddAsync(order);
            // guardo y creo el numero de orden (con el id)
            created.Number = $"PO-{created.Id:D6}"; // ej: PO-000042
            await _purchaseOrderRepository.UpdateAsync(created);

            await _purchaseOrderRepository.AddStatusHistoryAsync(new StatusHistory
            {
                PurchaseOrderId = created.Id,
                PreviousStatus = OrderStatus.Created, // no tengo estado previo a la creacion
                NewStatus = OrderStatus.Created,
                UserId = dto.EmployeeId,
                ChangedAt = DateTime.UtcNow,
                Comment = "Orden creada"
            });

            // ya esta creada. la tomo de la BD y la devuelvo como dto
            var fullOrder = await _purchaseOrderRepository.GetByIdAsync(created.Id);
            return ToDto(fullOrder!);
        }

        public async Task<PurchaseOrderDto?> ChangeStatusAsync(int id, ChangeOrderStatusDto dto)
        {
            // busco la orden
            var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(id);
            if (purchaseOrder == null) return null;

            if (!Enum.TryParse<OrderStatus>(dto.NewStatus, ignoreCase: true, out var newStatus))
                return null; // el string no corresponde a ningún valor válido del enum

            if (!OrderStatusValidator.IsValidTransition(purchaseOrder.Status, newStatus))
                return null; // transición no permitida


            var previousStatus = purchaseOrder.Status;
            purchaseOrder.Status = newStatus;

            if (newStatus == OrderStatus.Rejected)
            {
                purchaseOrder.RejectionReason = dto.Comment;
            }

            await _purchaseOrderRepository.UpdateAsync(purchaseOrder);

            await _purchaseOrderRepository.AddStatusHistoryAsync(new StatusHistory
            {
                PurchaseOrderId = purchaseOrder.Id,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                UserId = dto.ChangedByUserId,
                ChangedAt = DateTime.UtcNow,
                Comment = dto.Comment
            });

            var updated = await _purchaseOrderRepository.GetByIdAsync(purchaseOrder.Id);
            return ToDto(updated!);

        }


        // Metodos privados

        private static PurchaseOrderDto ToDto(PurchaseOrder order) => new()
        {
            Id = order.Id,
            Number = order.Number,
            EmployeeId = order.EmployeeId,
            EmployeeName = order.Employee.Name,
            SupplierId = order.SupplierId,
            SupplierName = order.Supplier.Name,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            RejectionReason = order.RejectionReason,
            Items = order.Items.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };


    }
}
