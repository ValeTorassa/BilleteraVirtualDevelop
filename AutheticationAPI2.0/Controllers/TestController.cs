using AutheticationAPI2._0.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutheticationAPI2._0.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("GetUserRole")]
        [Authorize(Policy = "StandardRights")]
        public IActionResult GetUserRole()
        {
            var username = User.Identity.Name;
            return Ok(new { Message = $"Hola {username} tu rol es el de usuario" });
        }

        [HttpGet]
        [Route("GetAdminRole")]
        [Authorize(Policy = "ElevatedRights")]
        public IActionResult GetAdminRole()
        {
            var username = User.Identity.Name;
            return Ok(new { Message = $"Hola {username} tu rol es el de administrador." });
        }
    }
}
