using Microsoft.AspNetCore.Mvc;
using NearU_Backend_Revised.Services;
using NearU_Backend_Revised.DTOs.Auth;


namespace NearU_Backend_Revised.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController :ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = await _userService.Register(request);
            return Ok(new{User = user});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userService.Login(request);
            return Ok(new { User = user });
        }
    }
}


