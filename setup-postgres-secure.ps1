# Alternative PostgreSQL Setup using User Secrets (More Secure)

Write-Host "================================" -ForegroundColor Cyan
Write-Host "PostgreSQL User Secrets Setup" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "This method stores your password securely (not in source control)" -ForegroundColor Yellow
Write-Host ""

# Initialize user secrets if not already done
Write-Host "1. Initializing User Secrets..." -ForegroundColor Yellow
dotnet user-secrets init --project . 2>&1 | Out-Null

# Prompt for password
$password = Read-Host "Enter your PostgreSQL password"
$username = Read-Host "Enter PostgreSQL username (default: postgres)" 
if ([string]::IsNullOrWhiteSpace($username)) {
    $username = "postgres"
}

$database = Read-Host "Enter database name (default: nearu_db_dev)"
if ([string]::IsNullOrWhiteSpace($database)) {
    $database = "nearu_db_dev"
}

# Build connection string
$connectionString = "Host=localhost;Port=5432;Database=$database;Username=$username;Password=$password"

# Store in user secrets
Write-Host ""
Write-Host "2. Storing connection string in User Secrets..." -ForegroundColor Yellow
dotnet user-secrets set "ConnectionStrings:PostgreSQL" $connectionString 2>&1 | Out-Null
Write-Host "   ✓ Connection string stored securely!" -ForegroundColor Green

Write-Host ""
Write-Host "3. Setting database provider..." -ForegroundColor Yellow
dotnet user-secrets set "DatabaseProvider" "PostgreSQL" 2>&1 | Out-Null
Write-Host "   ✓ Database provider set to PostgreSQL!" -ForegroundColor Green

# Test connection
Write-Host ""
Write-Host "4. Testing database connection..." -ForegroundColor Yellow
$output = dotnet ef database update 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "   ✓ Database migration successful!" -ForegroundColor Green
    Write-Host "   ✓ PostgreSQL database is ready!" -ForegroundColor Green
} else {
    Write-Host "   ⚠ Migration failed:" -ForegroundColor Red
    Write-Host ($output | Select-Object -Last 10) -ForegroundColor Red
}

Write-Host ""
Write-Host "================================" -ForegroundColor Cyan
Write-Host "Setup Complete!" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Your PostgreSQL credentials are stored securely in User Secrets" -ForegroundColor Green
Write-Host "They will NOT be committed to source control" -ForegroundColor Green
Write-Host ""
Write-Host "To view your secrets: dotnet user-secrets list" -ForegroundColor Yellow
Write-Host "To run your app: dotnet run" -ForegroundColor Yellow
Write-Host ""
