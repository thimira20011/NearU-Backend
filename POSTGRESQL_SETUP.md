# PostgreSQL Database Setup Guide

## ✅ PostgreSQL Configuration Complete!

Your backend is now configured to use PostgreSQL. Follow these steps to complete the setup:

---

## 📝 Step 1: Update Connection String

Edit `appsettings.json` or `appsettings.Development.json` with your PostgreSQL credentials:

```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=nearu_db;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
  },
  "DatabaseProvider": "PostgreSQL"
}
```

**Common PostgreSQL credentials:**
- Username: `postgres` (default)
- Password: Check your PostgreSQL installation
- Port: `5432` (default)
- Host: `localhost` or `127.0.0.1`

---

## 🔧 Step 2: Create Database

You have two options:

### Option A: Auto-create via EF Core (Recommended)
```bash
dotnet ef database update
```
This will automatically create the database and apply all migrations.

### Option B: Manual creation via psql
```bash
psql -U postgres
CREATE DATABASE nearu_db;
\q
```
Then run: `dotnet ef database update`

---

## 🔄 Step 3: Apply Migrations

```bash
# Apply all migrations to PostgreSQL
dotnet ef database update

# Or create a new migration if needed
dotnet ef migrations add MigrationName
```

---

## 🔀 Switching Between SQLite and PostgreSQL

Change the `DatabaseProvider` in `appsettings.json`:

```json
"DatabaseProvider": "PostgreSQL"  // Use PostgreSQL
"DatabaseProvider": "SQLite"      // Use SQLite
```

No code changes needed - it switches automatically!

---

## 🗄️ Current Configuration

**Production** (`appsettings.json`):
- Database: `nearu_db`
- Provider: `PostgreSQL`

**Development** (`appsettings.Development.json`):
- Database: `nearu_db_dev`
- Provider: `PostgreSQL`
- SQL logging enabled for debugging

---

## 🚀 PostgreSQL Features Enabled

✅ **Dynamic provider switching** (PostgreSQL/SQLite)  
✅ **Connection pooling** (built-in with Npgsql)  
✅ **Proper indexing** (Token and UserId indexes)  
✅ **Foreign key relationships** (User ↔ RefreshTokens)  
✅ **Cascade deletes** (Delete user → delete tokens)  
✅ **Production ready** with proper constraints  

---

## 🧪 Test Connection

Run this command to verify PostgreSQL connection:

```bash
dotnet ef dbcontext info
```

This shows the database provider and connection details.

---

## 📊 Database Schema

### Users Table
- Id (PK)
- Username
- Email
- PasswordHash
- Role

### RefreshTokens Table
- Id (PK)
- Token (Unique Index)
- UserId (FK to Users, Index)
- CreatedDate
- ExpiryDate
- RevokedDate
- ReplacedByToken
- ReasonRevoked
- IsRevoked (computed)

---

## ⚠️ Troubleshooting

### Authentication Failed Error
```
28P01: password authentication failed for user "postgres"
```
**Solution:** Update password in connection string or reset PostgreSQL password.

### Database Not Found
```
3D000: database "nearu_db" does not exist
```
**Solution:** Run `dotnet ef database update` to auto-create database.

### Port Already in Use
```
Cannot connect to port 5432
```
**Solution:** Check if PostgreSQL is running: `pg_ctl status`

---

## 🔐 Security Best Practices

1. **Never commit real passwords** to source control
2. Use **environment variables** for production:
   ```bash
   $env:ConnectionStrings__PostgreSQL="Host=prod.example.com;..."
   ```
3. Use **User Secrets** for development:
   ```bash
   dotnet user-secrets set "ConnectionStrings:PostgreSQL" "your-connection-string"
   ```

---

## 📦 Installed Packages

✅ `Npgsql.EntityFrameworkCore.PostgreSQL` - Version 10.0.0  
✅ `Microsoft.EntityFrameworkCore.Sqlite` - Version 10.0.5 (fallback)  
✅ `Microsoft.EntityFrameworkCore.Tools` - Version 10.0.5 (migrations)  

---

## Next Steps

1. Update the PostgreSQL password in your appsettings file
2. Run `dotnet ef database update` to create database
3. Test the connection
4. Start your application: `dotnet run`

Your backend will now use PostgreSQL! 🎉
