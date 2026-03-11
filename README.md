"""
# NearU – Backend API

## Overview
NearU is a University Lifestyle Hub and Local Business Marketplace designed to connect students with nearby businesses and services.  
This repository contains the backend RESTful API that powers the NearU platform.

The backend is built using **ASP.NET Core Web API** and provides secure endpoints for user authentication, business management, service discovery, orders, delivery coordination, job postings, and notifications.

It serves as the **core business logic layer** of the system and communicates with the frontend application and the database hosted on Azure.

---

# Features

## User Management
- User registration and authentication
- Role-based access control (Student, Business Owner, Rider, Admin)
- Profile management
- Secure password hashing and token-based authentication

## Business Management
- Create and manage business listings
- Upload photos and menus
- Business verification system
- Business dashboard support

## Search & Discovery
- Search businesses using keywords
- Filter by category, rating, and location
- Google Maps API integration for location services

## Order Management
- Place and track orders
- Order status updates
- Order history management

## Delivery Coordination
- Businesses can create delivery jobs
- Riders can accept and complete delivery tasks
- Delivery tracking and history

## Review & Rating System
- Users can submit reviews with ratings
- Photo attachments for reviews
- Business owners can respond to reviews

## Job Board
- Businesses can post part-time jobs
- Students can apply for jobs
- Application tracking

## Notifications
- Email notifications
- In-app notification support
- Event-based alerts (orders, jobs, reviews)

## Admin Panel Support
- User management
- Business verification
- Content moderation
- System monitoring

---

# Technology Stack

| Layer | Technology |
|------|-------------|
| Backend Framework | ASP.NET Core Web API |
| Language | C# |
| Database | Azure SQL Database |
| ORM | Entity Framework Core |
| Authentication | ASP.NET Identity + JWT |
| Cloud Platform | Microsoft Azure |
| File Storage | Azure Blob Storage |
| API Documentation | Swagger / OpenAPI |
| Email Service | SendGrid |

---

# System Architecture

Client (React Frontend)
        |
        v
ASP.NET Core Web API
        |
        v
Service Layer (Business Logic)
        |
        v
Repository Layer (Data Access)
        |
        v
Azure SQL Database

---

# Project Structure

NearU-Backend
│
├── Controllers        # API endpoints
├── Services           # Business logic
├── Repositories       # Database access
├── Models             # Entity models
├── DTOs               # Data transfer objects
├── Data               # DbContext and migrations
├── Middleware         # Authentication & error handling
├── Configurations     # App configurations
└── Program.cs         # Application entry point

---

# API Documentation

Swagger is used for API documentation.

After running the project, open:

http://localhost:5000/swagger

This interface allows developers to:
- View API endpoints
- Test requests
- Understand request/response formats

---

# Getting Started

## Prerequisites

Install the following tools:

- .NET 7 SDK
- Visual Studio or VS Code
- SQL Server or Azure SQL Database
- Git

---

# Installation

### Clone the repository

git clone https://github.com/yourusername/nearu-backend.git

### Navigate to the project directory

cd nearu-backend

### Restore dependencies

dotnet restore

### Configure the database

Update the **appsettings.json** file with your database connection string.

Example:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=NearUDB;User Id=USERNAME;Password=PASSWORD;"
}

---

# Run the Application

dotnet run

The API will run at:

https://localhost:5001

---

# Database Migration

Run the following command to apply migrations and create database tables:

dotnet ef database update

---

# Security Features

- HTTPS with TLS encryption
- JWT authentication
- Role-Based Access Control (RBAC)
- Password hashing
- SQL Injection protection
- Input validation and sanitization
- Rate limiting for API requests

---

# Future Enhancements

- Online payment integration
- Advanced analytics dashboard
- AI-based service recommendations
- Real-time delivery tracking
- Native mobile application support

---

# Contributors

**Group 11 – Faculty of Computing**  
Sabaragamuwa University of Sri Lanka

- K.W.T.N. Keerthiwansha – Full Stack Developer & System Architect  
- W.T.M.B. Wijesuriya – Frontend Developer & UI/UX Designer  
- K.V.P. Pahasara – Cloud & DevOps Engineer  
- M.U. Heshan – QA Engineer & Project Manager  

---

# License

This project is developed for **academic purposes as part of a university capstone project**.
"""
