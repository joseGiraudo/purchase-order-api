using Microsoft.AspNetCore.Mvc;
using PurchaseOrders.Application.Dtos;
using PurchaseOrders.Application.Interfaces;

namespace PurchaseOrders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;

        public PurchaseOrdersController(IPurchaseOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<PurchaseOrderDto>>> GetAll()
        {
            var orders = await _service.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrderDto>> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            return order is null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseOrderDto>> Create(CreatePurchaseOrderDto dto)
        {
            var created = await _service.CreateAsync(dto);
            if (created is null)
                return BadRequest("No se pudo crear la orden. Verificá que el empleado, el proveedor y los productos existan, que los productos pertenezcan al proveedor indicado, y que las cantidades sean válidas.");

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<PurchaseOrderDto>> ChangeStatus(int id, ChangeOrderStatusDto dto)
        {
            var updated = await _service.ChangeStatusAsync(id, dto);
            if (updated is null)
                return BadRequest("No se pudo cambiar el estado. Verificá que la orden exista y que la transición sea válida.");

            return Ok(updated);
        }
    }
}
