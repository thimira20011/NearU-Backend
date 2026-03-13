# Test PostgreSQL Connection
# Run this script to test your PostgreSQL connection

Write-Host "Testing PostgreSQL Connection..." -ForegroundColor Cyan
Write-Host ""

# Test 1: Check if PostgreSQL is running
Write-Host "1. Checking if PostgreSQL service is running..." -ForegroundColor Yellow
$pgService = Get-Service -Name "postgresql*" -ErrorAction SilentlyContinue
if ($pgService) {
    Write-Host "   ✓ PostgreSQL service found: $($pgService.Name) - Status: $($pgService.Status)" -ForegroundColor Green
} else {
    Write-Host "   ⚠ PostgreSQL service not found or not running" -ForegroundColor Red
}
Write-Host ""

# Test 2: Check if port 5432 is listening
Write-Host "2. Checking if PostgreSQL is listening on port 5432..." -ForegroundColor Yellow
$connection = Test-NetConnection -ComputerName localhost -Port 5432 -WarningAction SilentlyContinue -ErrorAction SilentlyContinue
if ($connection.TcpTestSucceeded) {
    Write-Host "   ✓ PostgreSQL is listening on port 5432" -ForegroundColor Green
} else {
    Write-Host "   ⚠ Cannot connect to port 5432" -ForegroundColor Red
}
Write-Host ""

# Test 3: Try to get database info from EF Core
Write-Host "3. Testing database connection via EF Core..." -ForegroundColor Yellow
$result = dotnet ef dbcontext info 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "   ✓ Database connection successful!" -ForegroundColor Green
    Write-Host $result
} else {
    Write-Host "   ⚠ Database connection failed" -ForegroundColor Red
    Write-Host "   Error: $result" -ForegroundColor Red
}
Write-Host ""

Write-Host "================================" -ForegroundColor Cyan
Write-Host "Connection String Configuration" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Current configuration expects:" -ForegroundColor Yellow
Write-Host "  Host: localhost"
Write-Host "  Port: 5432"
Write-Host "  Database: nearu_db_dev (Development) or nearu_db (Production)"
Write-Host "  Username: postgres"
Write-Host "  Password: [Update in appsettings.json]"
Write-Host ""
Write-Host "To update password:" -ForegroundColor Green
Write-Host "  1. Open appsettings.Development.json"
Write-Host "  2. Update the PostgreSQL connection string Password"
Write-Host "  3. Run: dotnet ef database update"
Write-Host ""
