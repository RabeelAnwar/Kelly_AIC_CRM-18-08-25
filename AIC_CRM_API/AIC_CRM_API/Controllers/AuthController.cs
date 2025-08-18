using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Auth;
using Service.Services.Auth.DTOs;

namespace AIC_CRM_API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

 
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> UserLogin(UserLoginDto user)
        {
            var result = await _auth.UserLogin(user);
            return Ok(result);
        }

    }
}
