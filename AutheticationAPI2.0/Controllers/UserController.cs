using AutheticationAPI2._0.Data;
using AutheticationAPI2._0.Model.Dto;
using AutheticationAPI2._0.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutheticationAPI2._0.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public UserController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authenticationService.LoginAsync(request);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(new { message = response.Message });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _authenticationService.RegisterAsync(request);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(new { message = response.Message });
        }

        [HttpPost("makeAdmin")]
        [Authorize(Policy = "ElevatedRights")]
        public async Task<IActionResult> MakeAdmin([FromBody] string user)
        {
            var operationResult = await _authenticationService.MakeAdminAsync(user);

            if (operationResult.IsSuccess)
                return Ok(operationResult);

            return BadRequest(new { message = operationResult.Message });
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] string request)
        {
            var result = await _authenticationService.ValidateRefreshTokenAsync(request);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(new { message = result.Message });
        }

        [HttpPost("logout")]
        [Authorize(Policy = "StandardRights")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            var result = await _authenticationService.LogoutAsync(refreshToken);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(new { message = result.Message });
        }
    }
}
