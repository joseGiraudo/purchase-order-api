using Microsoft.AspNetCore.Mvc;
using PurchaseOrders.Application.Dtos;
using PurchaseOrders.Application.Interfaces;

namespace PurchaseOrders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAll([FromQuery] int? supplierId)
        {
            var products = await _productService.GetAllAsync(supplierId);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto)
        {
            var created = await _productService.CreateAsync(dto);
            if(created != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            return BadRequest("El supplier no existe");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDto dto)
        {
            var updated = await _productService.UpdateAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
