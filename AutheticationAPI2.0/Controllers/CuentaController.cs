using AutheticationAPI2._0.Model;
using AutheticationAPI2._0.Services;
using AutheticationAPI2._0.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutheticationAPI2._0.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CuentaController : ControllerBase
    {
        private readonly CuentaService _cuentaService;

        public CuentaController(CuentaService cuentaService)
        {
            _cuentaService = cuentaService;
        }

        [HttpPost("crearCuenta")]
        public async Task<IActionResult> CrearCuenta([FromBody] int  userId)
        {
            var response = await _cuentaService.CreateCuentaAsync(userId);

            if (response)
                return Ok(response);

            return BadRequest(new { message = response.Message });
        }

        [HttpPost("actualizarCuenta")]
        public async Task<IActionResult> ActualizarCuenta([FromBody] Cuenta cuenta)
        {
            var response = await _cuentaService.UpdateCuentaAsync(cuenta);

            if (response)
                return Ok(response);

            return BadRequest(new { message = response.Message });
        }

        [HttpPost("eliminarCuenta")]
        public async Task<IActionResult> EliminarCuenta([FromBody] int userId)
        {
            var response = await _cuentaService.DeleteCuentaAsync(userId);

            if (response)
                return Ok(response);

            return BadRequest(new { message = response.Message });
        }

        [HttpPost("obtenerCuenta")]
        public async Task<IActionResult> ObtenerCuenta([FromBody] string numeroCuenta)
        {
            var response = await _cuentaService.GetCuentaByNumeroCuentaAsync(numeroCuenta);

            if (response)
                return Ok(response);

            return BadRequest(new { message = response.Message });
        }

        [HttpPost("obtenerCuentas")]
        public async Task<IActionResult> ObtenerCuentas([FromBody] int userId)
        {
            var response = await _cuentaService.GetCuentaByUserIdAsync(userId);

            if (response)
                return Ok(response);

            return BadRequest(new { message = response.Message });
        }
    }
}
