# API Endpoints - Postman Testing Guide

## ✅ All Endpoints Working with PostgreSQL!

Base URL: `http://localhost:5059`

---

## 1️⃣ Register User

**Endpoint:** `POST /api/auth/register`

**Request Body:**
```json
{
  "username": "johndoe",
  "email": "john@test.com",
  "password": "Password123!"
}
```

**Success Response (200):**
```json
{
  "message": "User registered successfully",
  "userId": "f64ea073-0fbb-49cd-9168-162ef92a9887",
  "username": "johndoe"
}
```

**Error Response (400):**
```json
{
  "message": "User already exists"
}
```

---

## 2️⃣ Login

**Endpoint:** `POST /api/auth/login`

**Request Body:**
```json
{
  "email": "john@test.com",
  "password": "Password123!"
}
```

**Success Response (200):**
```json
{
  "userId": "f64ea073-0fbb-49cd-9168-162ef92a9887",
  "username": "johndoe",
  "email": "john@test.com",
  "role": "User",
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "obr/lEBfEEDlLioZ1VMuX59MkjgQI7xpp96sPxuGtu52...",
  "accessTokenExpiry": "2026-03-13T09:29:05.1824262Z",
  "refreshTokenExpiry": "2026-03-20T09:14:05.1127945Z"
}
```

**Error Response (401):**
```json
{
  "message": "Invalid credentials"
}
```

**What you get:**
- ✅ Access Token (JWT) - valid for 15 minutes
- ✅ Refresh Token - valid for 7 days
- ✅ User info (userId, username, email, role)
- ✅ Token expiry times

---

## 3️⃣ Refresh Token

**Endpoint:** `POST /api/auth/refresh`

**Request Body:**
```json
{
  "refreshToken": "obr/lEBfEEDlLioZ1VMuX59MkjgQI7xpp96sPxuGtu52..."
}
```

**Success Response (200):**
```json
{
  "userId": "f64ea073-0fbb-49cd-9168-162ef92a9887",
  "username": "johndoe",
  "email": "john@test.com",
  "role": "User",
  "accessToken": "NEW_ACCESS_TOKEN...",
  "refreshToken": "NEW_REFRESH_TOKEN...",
  "accessTokenExpiry": "2026-03-13T09:30:00Z",
  "refreshTokenExpiry": "2026-03-20T09:15:00Z"
}
```

**Error Response (401):**
```json
{
  "message": "Invalid or expired refresh token"
}
```

**What happens:**
- ✅ Old refresh token is revoked
- ✅ New refresh token is generated
- ✅ New access token is generated
- ✅ Old refresh token cannot be reused

---

## 4️⃣ Logout

**Endpoint:** `POST /api/auth/logout`

**Request Body:**
```json
{
  "refreshToken": "S1pquS6HuRpLGWXBX7lNHXmYyDfU7TT7uBfPn7my6986..."
}
```

**Success Response (200):**
```json
{
  "message": "Logged out successfully"
}
```

**Error Response (400):**
```json
{
  "message": "Refresh token is required"
}
```

**What happens:**
- ✅ Refresh token is revoked
- ✅ Cannot be used to get new access tokens
- ✅ Current access token still works until expiry

---

## 🔐 Using Access Token in Protected Endpoints

For endpoints that require authentication, add the Authorization header:

**Header:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Example in Postman:**
1. Go to **Headers** tab
2. Add key: `Authorization`
3. Add value: `Bearer <your-access-token>`

---

## 🧪 Test Flow Example

### Complete Authentication Flow:
1. **Register** → Get userId
2. **Login** → Get accessToken + refreshToken
3. Use **accessToken** for API calls (valid 15 min)
4. When expired → **Refresh** → Get new tokens
5. **Logout** → Revoke refresh token

### Multi-Device Scenario:
- Login on Device A → Token A
- Login on Device B → Token B
- Logout Device A → Only Token A revoked
- Device B still works

---

## 📊 PostgreSQL Database

**Tables:**
- `Users` - Stores user accounts
- `RefreshTokens` - Stores refresh tokens with rotation tracking

**Connection:**
- Host: localhost:5432
- Database: nearu_db_dev (Development)
- Provider: Npgsql.EntityFrameworkCore.PostgreSQL

**Features:**
- ✅ Auto-increment primary keys
- ✅ Unique token index
- ✅ Foreign key constraints
- ✅ Cascade deletes

---

## ✨ Everything Working!

Your backend is now fully functional with:
- ✅ PostgreSQL database
- ✅ JWT authentication
- ✅ Token refresh/rotation
- ✅ Secure logout
- ✅ User registration/login

Ready to test in Postman! 🚀
