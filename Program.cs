using Microsoft.EntityFrameworkCore;
using NearU_Backend_Revised.Data;
using NearU_Backend_Revised.Services;
using NearU_Backend_Revised.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register ApplicationDbContext with PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=NearU.db"));

// Register repositories and services
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
