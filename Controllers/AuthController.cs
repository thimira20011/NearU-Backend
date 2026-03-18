using Microsoft.AspNetCore.Mvc;
using NearU_Backend_Revised.Services;
using NearU_Backend_Revised.DTOs.Auth;
using NearU_Backend_Revised.Models;


namespace NearU_Backend_Revised.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
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
                var data = new { userId = user.Id, username = user.Username }; 

                return Created(string.Empty, ApiResponse<object>.SuccessResponse("User registered successfully", data));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailResponse(ex.Message)); 
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
                return Ok(ApiResponse<object>.SuccessResponse("Token refreshed successfully", authResponse));
            }
            catch (Exception ex)
            {
                return Unauthorized(ApiResponse<object>.FailResponse(ex.Message));
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var success = await _userService.Logout(request.RefreshToken);
                if (success)
                    return Ok(ApiResponse<object>.SuccessResponse("Logged out successfully", default!));
                else
                    return BadRequest(ApiResponse<object>.FailResponse("Logout failed"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailResponse(ex.Message));
            }
        }
    }
}