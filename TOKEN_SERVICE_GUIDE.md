# Token Service Implementation - Complete Guide

## ✅ What Was Created

### 1. **Core Token Service Files**

#### `Services/ITokenService.cs`
Interface defining token operations:
- ✅ `GenerateAccessToken(User user)` - Generate JWT access tokens
- ✅ `GenerateRefreshToken(string userId)` - Generate secure refresh tokens
- ✅ `ValidateAccessToken(string token)` - Validate and extract claims from JWT

#### `Services/TokenService.cs`
Complete implementation with:
- JWT token generation with user claims (userId, email, username, role)
- Cryptographically secure refresh token generation
- Token validation with proper error handling
- Configurable expiry times from appsettings.json

### 2. **Configuration**

#### `Configuration/JwtSettings.cs`
JWT configuration model with:
- `SecretKey` - Signing key (min 32 characters)
- `Issuer` - Token issuer
- `Audience` - Token audience
- `AccessTokenExpiryInMinutes` - Default 15 minutes
- `RefreshTokenExpiryInDays` - Default 7 days

#### `appsettings.json` (Updated)
Added JWT configuration:
```json
"JwtSettings": {
  "SecretKey": "YourSuperSecretKeyForJWT_MustBeAtLeast32CharactersLong!!!",
  "Issuer": "NearU-Backend",
  "Audience": "NearU-Client",
  "AccessTokenExpiryInMinutes": 15,
  "RefreshTokenExpiryInDays": 7
}
```

### 3. **User Roles**

#### `Models/UserRoles.cs`
Role constants for the four user types:
- ✅ **Student** - Student users
- ✅ **Rider** - Delivery riders
- ✅ **Business** - Business accounts
- ✅ **Admin** - System administrators

Helper methods:
- `AllRoles` - Array of all available roles
- `IsValidRole(string)` - Validate role names

### 4. **Updated Models**

#### `Models/Usee.cs` (Updated)
Added role property:
```csharp
[Required]
[MaxLength(50)]
public string Role { get; set; } = UserRoles.Student; // Default role
```

### 5. **Documentation & Examples**

#### `Examples/TokenServiceUsageExample.cs`
Complete working examples for:
- Generating tokens for all role types
- Token validation
- Complete login flow
- Token refresh scenario

## 🎯 JWT Token Claims

The access token includes these claims:

| Claim Type | Description | Example |
|------------|-------------|---------|
| `ClaimTypes.NameIdentifier` | User's unique ID | `"guid-123-456"` |
| `ClaimTypes.Email` | User's email | `"student@edu.com"` |
| `ClaimTypes.Name` | Username | `"john_doe"` |
| `ClaimTypes.Role` | User's role | `"Student"`, `"Rider"`, etc. |
| `JwtRegisteredClaimNames.Sub` | Subject (User ID) | `"guid-123-456"` |
| `JwtRegisteredClaimNames.Jti` | Token ID (unique) | `"guid-789-012"` |

## 📋 Setup Instructions

### Step 1: Update Program.cs

Add JWT authentication and register the service:

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NearU_Backend_Revised.Configuration;
using NearU_Backend_Revised.Data;
using NearU_Backend_Revised.Repositories;
using NearU_Backend_Revised.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure JWT Settings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);

// Get JWT settings for authentication configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings!.SecretKey);

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Add Authorization
builder.Services.AddAuthorization();

// Add Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Register Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add Authentication & Authorization Middleware
app.UseAuthentication();  // Must be before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
```

### Step 2: Update appsettings.json

**⚠️ IMPORTANT: Change the SecretKey in production!**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NearUDb;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "JwtSettings": {
    "SecretKey": "CHANGE_THIS_TO_A_SECURE_RANDOM_KEY_AT_LEAST_32_CHARACTERS",
    "Issuer": "NearU-Backend",
    "Audience": "NearU-Client",
    "AccessTokenExpiryInMinutes": 15,
    "RefreshTokenExpiryInDays": 7
  }
}
```

### Step 3: Usage in Authentication Controller

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _tokenRepository;

    public AuthController(
        ITokenService tokenService, 
        IRefreshTokenRepository tokenRepository)
    {
        _tokenService = tokenService;
        _tokenRepository = tokenRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // 1. Validate user credentials (implement your logic)
        var user = await ValidateUserCredentials(request.Email, request.Password);

        if (user == null)
            return Unauthorized("Invalid credentials");

        // 2. Generate tokens
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

        // 3. Save refresh token to database
        await _tokenRepository.SaveRefreshTokenAsync(refreshToken);

        // 4. Return tokens
        return Ok(new
        {
            accessToken = accessToken,
            refreshToken = refreshToken.Token,
            expiresIn = 15 * 60, // 15 minutes in seconds
            user = new
            {
                id = user.Id,
                email = user.Email,
                username = user.Username,
                role = user.Role
            }
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        // 1. Validate refresh token
        var storedToken = await _tokenRepository
            .GetRefreshTokenByTokenStringAsync(request.RefreshToken);

        if (storedToken == null || !storedToken.IsActive)
            return Unauthorized("Invalid or expired refresh token");

        // 2. Generate new tokens
        var user = storedToken.User; // Load user from navigation property
        var newAccessToken = _tokenService.GenerateAccessToken(user!);
        var newRefreshToken = _tokenService.GenerateRefreshToken(user!.Id);

        // 3. Replace old refresh token
        await _tokenRepository.ReplaceRefreshTokenAsync(
            request.RefreshToken, 
            newRefreshToken
        );

        // 4. Return new tokens
        return Ok(new
        {
            accessToken = newAccessToken,
            refreshToken = newRefreshToken.Token
        });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
    {
        // Revoke the refresh token
        await _tokenRepository.RevokeRefreshTokenAsync(
            request.RefreshToken, 
            "User logged out"
        );

        return Ok(new { message = "Logged out successfully" });
    }
}
```

## 🔒 Protecting Endpoints with Roles

### Require Authentication
```csharp
[Authorize]
[HttpGet("profile")]
public IActionResult GetProfile()
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    // Return user profile
}
```

### Require Specific Role
```csharp
[Authorize(Roles = UserRoles.Student)]
[HttpGet("student-only")]
public IActionResult StudentOnly()
{
    return Ok("This is only for students");
}
```

### Require Multiple Roles
```csharp
[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Business}")]
[HttpGet("admin-or-business")]
public IActionResult AdminOrBusiness()
{
    return Ok("Admins or Businesses can access this");
}
```

### Custom Authorization
```csharp
[Authorize]
[HttpGet("riders-and-businesses")]
public IActionResult RidersAndBusinesses()
{
    var role = User.FindFirst(ClaimTypes.Role)?.Value;

    if (role != UserRoles.Rider && role != UserRoles.Business)
        return Forbid();

    return Ok("Riders and Businesses only");
}
```

## 📊 Token Structure

### Access Token (JWT)
```json
{
  "header": {
    "alg": "HS256",
    "typ": "JWT"
  },
  "payload": {
    "nameid": "user-guid-123",
    "email": "student@edu.com",
    "unique_name": "john_doe",
    "role": "Student",
    "sub": "user-guid-123",
    "jti": "token-guid-456",
    "exp": 1234567890,
    "iss": "NearU-Backend",
    "aud": "NearU-Client"
  }
}
```

### Refresh Token
```
Token: "base64-encoded-random-64-bytes"
ExpiryDate: DateTime (7 days from creation)
CreatedDate: DateTime (UTC)
UserId: "user-guid-123"
```

## 🧪 Testing with Postman/Insomnia

### 1. Login Request
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "student@edu.com",
  "password": "password123"
}
```

### 2. Access Protected Endpoint
```http
GET /api/protected-endpoint
Authorization: Bearer <your-access-token>
```

### 3. Refresh Token
```http
POST /api/auth/refresh
Content-Type: application/json

{
  "refreshToken": "your-refresh-token-string"
}
```

## 🔐 Security Best Practices

1. **Secret Key**: Use a strong, random secret key (minimum 32 characters)
   ```bash
   # Generate secure key in PowerShell:
   [Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(64))
   ```

2. **HTTPS Only**: Always use HTTPS in production

3. **Token Storage**: 
   - Store access tokens in memory (never localStorage)
   - Store refresh tokens in httpOnly cookies or secure storage

4. **Token Expiry**: 
   - Keep access tokens short-lived (15-30 minutes)
   - Refresh tokens can be longer (7-30 days)

5. **Revocation Events**:
   - Password change → Revoke all tokens
   - Logout → Revoke specific token
   - Account deletion → Revoke all tokens

## 📝 Files Created/Modified

1. ✅ `Services/ITokenService.cs` - Created
2. ✅ `Services/TokenService.cs` - Implemented
3. ✅ `Configuration/JwtSettings.cs` - Created
4. ✅ `Models/UserRoles.cs` - Created
5. ✅ `Models/Usee.cs` - Updated with Role property
6. ✅ `appsettings.json` - Updated with JWT settings
7. ✅ `NearU_Backend_Revised.csproj` - Added JWT packages
8. ✅ `Examples/TokenServiceUsageExample.cs` - Created

## ✅ Build Status

- **Build**: ✅ Successful
- **Packages**: ✅ Restored (JWT Bearer, System.IdentityModel.Tokens.Jwt)
- **Compilation**: ✅ No Errors

Your Token Service is ready to use! 🎉
