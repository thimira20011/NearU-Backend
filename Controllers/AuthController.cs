using Microsoft.AspNetCore.Mvc;
using NearU_Backend_Revised.Services;
using NearU_Backend_Revised.DTOs.Auth;
using NearU_Backend_Revised.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.OpenApi;


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
            try
            {
                var user = await _userService.Register(request);
                var data = new { userId = user.Id, username = user.UserName };

                return ok(ApiResponse<object>.SuccessResponse("User registered successfully", data));
            }
            catch (Exception ex)
            {
                return BadRequest(IOpenApiResponse<object>.FailResponse(ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var authResponse = await _userService.Login(request);
                return Ok(ApiResponse<object>.SuccessResponse("Login successful", authResponse));
            }
            catch (Exception ex)
            {
                return Unauthorized(ApiResponse<object>.FailResponse(ex.Message));
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                var authResponse = await _userService.RefreshToken(request);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var success = await _userService.Logout(request.RefreshToken);
                if (success)
                    return Ok(new { message = "Logged out successfully" });
                else
                    return BadRequest(new { message = "Logout failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}



