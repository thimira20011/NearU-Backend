using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using NearU_Backend_Revised.DTOs.Auth;
using NearU_Backend_Revised.Repositories;
using NearU_Backend_Revised.Models;
using BCrypt.Net;

namespace NearU_Backend_Revised.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;

        public UserService(UserRepository userrepo)
        {
            _userRepo = userrepo;
        }

        public async Task<User> Register(RegisterRequest request)
        {
            var existingUser = await _userRepo.GetUserByEmail(request.Email);
            if (existingUser != null) throw new Exception("User already exists");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword,
                Role = "User"
            };

            await _userRepo.AddUser(user);
            return user;
        }

        public async Task<User> Login(LoginRequest request)
        {
            var user = await _userRepo.GetUserByEmail(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            return user;
        }
    }
}



