# Quick PostgreSQL Setup Script
# This script helps you update the connection string and test the connection

param(
    [Parameter(Mandatory=$false)]
    [string]$Password,
    
    [Parameter(Mandatory=$false)]
    [string]$Username = "postgres",
    
    [Parameter(Mandatory=$false)]
    [string]$Database = "nearu_db_dev",
    
    [Parameter(Mandatory=$false)]
    [string]$Host = "localhost",
    
    [Parameter(Mandatory=$false)]
    [int]$Port = 5432
)

Write-Host "================================" -ForegroundColor Cyan
Write-Host "PostgreSQL Setup Helper" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

# If password not provided, ask for it
if (-not $Password) {
    $Password = Read-Host "Enter PostgreSQL password"
}

# Build connection string
$connectionString = "Host=$Host;Port=$Port;Database=$Database;Username=$Username;Password=$Password"

Write-Host "Connection String: Host=$Host;Port=$Port;Database=$Database;Username=$Username;Password=****" -ForegroundColor Yellow
Write-Host ""

# Update appsettings.Development.json
Write-Host "1. Updating appsettings.Development.json..." -ForegroundColor Yellow

$appsettingsPath = "appsettings.Development.json"
$config = Get-Content $appsettingsPath | ConvertFrom-Json

# Ensure ConnectionStrings exists
if (-not $config.ConnectionStrings) {
    $config | Add-Member -MemberType NoteProperty -Name "ConnectionStrings" -Value ([PSCustomObject]@{})
}

# Update PostgreSQL connection string
$config.ConnectionStrings.PostgreSQL = $connectionString
$config.DatabaseProvider = "PostgreSQL"

# Save updated configuration
$config | ConvertTo-Json -Depth 10 | Set-Content $appsettingsPath
Write-Host "   ✓ Configuration updated!" -ForegroundColor Green
Write-Host ""

# Test connection
Write-Host "2. Testing connection to PostgreSQL..." -ForegroundColor Yellow
$env:ConnectionStrings__PostgreSQL = $connectionString

try {
    $output = dotnet ef database update 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✓ Database migration successful!" -ForegroundColor Green
        Write-Host "   ✓ PostgreSQL database is ready!" -ForegroundColor Green
    } else {
        Write-Host "   ⚠ Migration failed. See error below:" -ForegroundColor Red
        Write-Host $output -ForegroundColor Red
    }
} catch {
    Write-Host "   ⚠ Error occurred: $_" -ForegroundColor Red
}

Write-Host ""
Write-Host "================================" -ForegroundColor Cyan
Write-Host "Setup Complete!" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "  1. Run: dotnet run" -ForegroundColor White
Write-Host "  2. Your API will connect to PostgreSQL automatically" -ForegroundColor White
Write-Host ""
