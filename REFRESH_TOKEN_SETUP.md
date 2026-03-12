# Refresh Token Repository - Setup Guide

## Overview
This guide shows how to integrate the RefreshTokenRepository into your application.

## Files Created
1. **Models/RefreshToken.cs** - Entity model for refresh tokens
2. **Models/Usee.cs** - Updated user model with refresh token relationship
3. **Data/ApplicationDbContext.cs** - Database context
4. **Repositories/IRefreshTokenRepository.cs** - Repository interface
5. **Repositories/RefreshTokenRepository.cs** - Repository implementation
6. **Examples/RefreshTokenRepositoryUsageExample.cs** - Usage examples

## Setup Instructions

### Step 1: Update Program.cs

Add the following to your `Program.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using NearU_Backend_Revised.Data;
using NearU_Backend_Revised.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Register Repository
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

### Step 2: Update appsettings.json

Add your database connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NearUDb;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Step 3: Install Required NuGet Packages

Run these commands in Package Manager Console:

```powershell
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
```

Or using .NET CLI:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### Step 4: Create Database Migration

Run in Package Manager Console:

```powershell
Add-Migration InitialCreateWithRefreshTokens
Update-Database
```

Or using .NET CLI:

```bash
dotnet ef migrations add InitialCreateWithRefreshTokens
dotnet ef database update
```

## Repository Methods

### 1. SaveRefreshTokenAsync
Saves a new refresh token to the database.

```csharp
var token = new RefreshToken
{
    Token = "your-secure-token",
    UserId = "user-id",
    ExpiryDate = DateTime.UtcNow.AddDays(7)
};
await _tokenRepository.SaveRefreshTokenAsync(token);
```

### 2. GetRefreshTokenByTokenStringAsync
Retrieves a refresh token by its token string.

```csharp
var token = await _tokenRepository.GetRefreshTokenByTokenStringAsync("token-string");
if (token != null && token.IsActive)
{
    // Token is valid
}
```

### 3. RevokeRefreshTokenAsync
Revokes a refresh token.

```csharp
bool revoked = await _tokenRepository.RevokeRefreshTokenAsync(
    "token-string", 
    "User logged out"
);
```

### 4. ReplaceRefreshTokenAsync
Replaces an old token with a new one (for token refresh flow).

```csharp
var newToken = new RefreshToken
{
    Token = "new-token",
    UserId = "user-id",
    ExpiryDate = DateTime.UtcNow.AddDays(7)
};
var replaced = await _tokenRepository.ReplaceRefreshTokenAsync("old-token", newToken);
```

### 5. GetRefreshTokensByUserIdAsync
Gets all refresh tokens for a user.

```csharp
var userTokens = await _tokenRepository.GetRefreshTokensByUserIdAsync("user-id");
```

### 6. GetActiveRefreshTokensByUserIdAsync
Gets all active (non-revoked, non-expired) tokens for a user.

```csharp
var activeTokens = await _tokenRepository.GetActiveRefreshTokensByUserIdAsync("user-id");
```

### 7. RevokeAllUserTokensAsync
Revokes all tokens for a user (e.g., on password change).

```csharp
int revokedCount = await _tokenRepository.RevokeAllUserTokensAsync(
    "user-id", 
    "Password changed"
);
```

### 8. DeleteExpiredTokensAsync
Deletes expired and revoked tokens (cleanup operation).

```csharp
int deletedCount = await _tokenRepository.DeleteExpiredTokensAsync();
```

## Usage in Controllers

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IRefreshTokenRepository _tokenRepository;

    public AuthController(IRefreshTokenRepository tokenRepository)
    {
        _tokenRepository = tokenRepository;
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        // Validate old token
        var oldToken = await _tokenRepository.GetRefreshTokenByTokenStringAsync(request.Token);

        if (oldToken == null || !oldToken.IsActive)
            return Unauthorized("Invalid token");

        // Create new token
        var newToken = new RefreshToken
        {
            Token = GenerateSecureToken(),
            UserId = oldToken.UserId,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        // Replace old with new
        await _tokenRepository.ReplaceRefreshTokenAsync(request.Token, newToken);

        return Ok(new { refreshToken = newToken.Token });
    }
}
```

## Database Schema

The RefreshToken table will have these columns:
- `Id` (int, PK, auto-increment)
- `Token` (nvarchar(500), unique, required)
- `ExpiryDate` (datetime2, required)
- `CreatedDate` (datetime2, required)
- `RevokedDate` (datetime2, nullable)
- `ReplacedByToken` (nvarchar(500), nullable)
- `ReasonRevoked` (nvarchar(200), nullable)
- `UserId` (nvarchar, FK, required)

## Security Best Practices

1. **Token Generation**: Use cryptographically secure random generators
   ```csharp
   var randomBytes = RandomNumberGenerator.GetBytes(64);
   var token = Convert.ToBase64String(randomBytes);
   ```

2. **Token Expiry**: Set reasonable expiry times (7-30 days)

3. **Token Rotation**: Always replace tokens on refresh (prevents token replay)

4. **Revocation**: Revoke all tokens on password change or security events

5. **Cleanup**: Run scheduled jobs to delete expired tokens

## Next Steps

1. Update your `Program.cs` with the code above
2. Add connection string to `appsettings.json`
3. Install NuGet packages
4. Run migrations
5. Integrate into your authentication flow
