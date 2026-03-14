# Token Flow → User Flow Integration Guide

## ✅ Integration Complete!

The **TokenService** is now fully integrated with **UserService** and ready to use in controllers.

---

## 🔄 Flow Overview

### **Login Flow**
```csharp
// In your controller:
var authResponse = await _userService.Login(loginRequest);
// Returns: AuthResponse with AccessToken, RefreshToken, and user info
```

**What happens internally:**
1. UserService validates credentials (email + password)
2. TokenService generates JWT access token
3. TokenService generates refresh token
4. Refresh token saved to database
5. Returns AuthResponse with both tokens

---

### **Refresh Token Flow**
```csharp
// In your controller:
var authResponse = await _userService.RefreshToken(refreshRequest);
// Returns: New AuthResponse with new AccessToken and RefreshToken
```

**What happens internally:**
1. TokenService validates old refresh token
2. Old token revoked and new refresh token generated (rotation)
3. New JWT access token generated for the user
4. Returns AuthResponse with new tokens

---

### **Logout Flow**
```csharp
// Single device logout:
var success = await _userService.Logout(refreshToken);
// Returns: true if token revoked successfully

// All devices logout:
var revokedCount = await _userService.LogoutAllDevices(userId);
// Returns: number of tokens revoked
```

**What happens internally:**
1. TokenService marks refresh token(s) as revoked in database
2. User can no longer use that token to get new access tokens
3. Current access token remains valid until expiry (design choice)

---

## 📦 DTOs Ready to Use

### **AuthResponse** (returned by Login and RefreshToken)
```csharp
{
    "userId": "string",
    "username": "string", 
    "email": "string",
    "role": "string",
    "accessToken": "string",
    "refreshToken": "string",
    "accessTokenExpiry": "DateTime",
    "refreshTokenExpiry": "DateTime"
}
```

### **RefreshTokenRequest** (input for RefreshToken)
```csharp
{
    "refreshToken": "string"
}
```

---

## 🎯 Simple Controller Example

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var response = await _userService.Login(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        try
        {
            var response = await _userService.RefreshToken(request);
            return Ok(response);
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
            return Ok(new { message = "Logged out successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize] // Requires valid JWT
    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAllDevices()
    {
        try
        {
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var count = await _userService.LogoutAllDevices(userId);
            return Ok(new { message = $"Logged out from {count} devices" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
```

---

## 🔐 JWT Claims Available

When a user is authenticated, these claims are available in controllers via `User` property:

```csharp
var userId = User.FindFirst("userId")?.Value;
var email = User.FindFirst(ClaimTypes.Email)?.Value;
var username = User.FindFirst(ClaimTypes.Name)?.Value;
var role = User.FindFirst(ClaimTypes.Role)?.Value;
```

Or use authorization attributes:
```csharp
[Authorize] // Requires any authenticated user
[Authorize(Policy = "RequireUserId")] // Requires userId claim
[Authorize(Roles = "Admin")] // Requires Admin role
```

---

## ✨ Key Features

✅ **Clean separation**: Token logic isolated in TokenService  
✅ **Easy integration**: UserService provides high-level methods  
✅ **Token rotation**: Refresh tokens automatically rotated on use  
✅ **Multi-device support**: Each device gets unique refresh token  
✅ **Logout options**: Single device or all devices  
✅ **Secure**: BCrypt passwords + JWT tokens + refresh token rotation  

---

## 📝 Notes for Controller Developer

- **Login**: Call `_userService.Login()` - returns full AuthResponse
- **Refresh**: Call `_userService.RefreshToken()` - returns new tokens
- **Logout**: Call `_userService.Logout()` - revokes refresh token
- **Error handling**: All methods throw exceptions on failure (wrap in try-catch)
- **No direct TokenService access needed**: Everything goes through UserService
