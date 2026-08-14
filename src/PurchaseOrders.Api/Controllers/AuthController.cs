using Microsoft.AspNetCore.Mvc;
using PurchaseOrders.Application.Dtos;
using PurchaseOrders.Application.Interfaces;

namespace PurchaseOrders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result is null)
                return Unauthorized("Email o contraseña incorrectos.");

            return Ok(result);
        }
    }
}
