using Microsoft.AspNetCore.Mvc;
using PurchaseOrders.Application.Dtos;
using PurchaseOrders.Application.Interfaces;
using PurchaseOrders.Application.Services;
using PurchaseOrders.Domain.Entities;

namespace PurchaseOrders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
        {
            var created = await _userService.CreateAsync(dto);
            if (created != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            return BadRequest("El Supervisor no existe");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            var updated = await _userService.UpdateAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _userService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
