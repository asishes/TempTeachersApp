using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.UserDTO;
using TeachersApp.Entity.ModelDTO.UserDTO.ResetPasswordDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;

namespace TeachersApp.Services.Repositories
{
    public class UserService : IUserService
    {
        private readonly TeachersAppDbcontext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public UserService(TeachersAppDbcontext context, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        public async Task RegisterUserAsync(RegisterUserDTO dto)
        {
            // Validate role existence
            var role = await _context.Roles.FindAsync(dto.RoleID);
            if (role == null)
            {
                throw new Exception("Role does not exist");
            }

            // Check if username or email already exists
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                throw new Exception("Username is already taken");
            }

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                throw new Exception("Email is already registered");
            }

            // Hash the password
            var passwordHash = HashPassword(dto.Password);

            // Create new user
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = passwordHash,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                RoleID = dto.RoleID,
                EmployeeID = dto.EmployeeID,
                DateOfBirth = dto.DateOfBirth,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        private string HashPassword(string password)
        {
            // Use a strong hash function such as PBKDF2 with a unique salt
            var salt = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{salt}:{hashed}";
        }


        public async Task<string> AuthenticateAsync(LoginDTO loginDTO)
        {
            var user = await _context.Users
                .Include(u =>u.Role).FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
            if (user == null || !VerifyPassword(user.PasswordHash, loginDTO.Password))
            {
                throw new Exception("Invalid credentials");
            }

            return _jwtTokenService.GenerateToken(user, loginDTO.RememberMe);
        }

        private bool VerifyPassword(string storedHash, string password)
        {
            var parts = storedHash.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            var hashToVerify = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return hash.SequenceEqual(hashToVerify);
        }


        public async Task<User?> GetUserByTokenAsync(string token)
        {
            // Decode the token to extract email
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                throw new ArgumentException("Invalid token.");
            }

            var userIDClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIDClaim))
            {
                throw new ArgumentException("UserID not found in token.");
            }

            // Parse the userIDClaim (string) to an integer
            if (!int.TryParse(userIDClaim, out int userId))
            {
                throw new ArgumentException("Invalid UserID format in token.");
            }

            var user = await _context.Users
                .Include(u => u.Role)
               .Include(u => u.Employee)
                .ThenInclude(e => e.School)
               .Include(u => u.Employee)
                .ThenInclude(e => e.EmployeeType)
                .FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null)
            {
                throw new ArgumentException("User with the provided ID not found.");
            }

            return user;
        }


        public async Task<User?> UpdateUserAsync(int userId, UpdateUserDTO updateUserDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            // Update the existing user with the new values from DTO
            updateUserDto.ToUpdateUser(user);

            // Update the UpdatedAt timestamp
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(user);
            // Check if the user has an associated employee
            if (user.EmployeeID != null)
            {
                var employee = await _context.Employees.FindAsync(user.EmployeeID);
                if (employee != null)
                {
                    // Update employee details if needed
                    if (updateUserDto.FirstName != null)
                    {
                        employee.FirstName = updateUserDto.FirstName;
                    }

                    if (updateUserDto.LastName != null)
                    {
                        employee.LastName = updateUserDto.LastName;
                    }

                    if (updateUserDto.DateOfBirth != default(DateTime))
                    {
                        employee.DateOfBirth = updateUserDto.DateOfBirth;
                    }

                    // Update any other employee fields as necessary

                    _context.Employees.Update(employee);
                }
            }

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> ResetPasswordByUserAsync(ResetPasswordByUserDTO resetPasswordDto, string token)
        {
            if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
            {
                throw new ArgumentException("Password and Confirm Password do not match.");
            }

            // Decode the token to extract email
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                throw new ArgumentException("Invalid token.");
            }

            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(emailClaim))
            {
                throw new ArgumentException("Email not found in token.");
            }


            // Find user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == emailClaim);
            if (user == null)
            {
                throw new ArgumentException("User with the provided email not found.");
            }

            // Check if old password is correct
            if (!VerifyNewPassword(resetPasswordDto.OldPassword, user.PasswordHash))
            {
                throw new ArgumentException("Old password is incorrect.");
            }

            // Update the password
            user.PasswordHash = HashNewPassword(resetPasswordDto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        private bool VerifyNewPassword(string inputPassword, string storedPasswordHash)
        {
            try
            {
                // Split the stored hash into salt and hash parts
                var parts = storedPasswordHash.Split(':');
                if (parts.Length != 2)
                {
                    throw new Exception("Invalid password hash format.");
                }

                var salt = Convert.FromBase64String(parts[0]);  // Decode salt from Base64
                var storedHash = Convert.FromBase64String(parts[1]);  // Decode hash from Base64

                // Generate the hash to compare with the stored hash
                var hashToVerify = KeyDerivation.Pbkdf2(
                    password: inputPassword,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8);

                return storedHash.SequenceEqual(hashToVerify);
            }
            catch (FormatException ex)
            {
                throw new Exception("The stored password hash or salt is in an invalid format.", ex);
            }
        }

        private string HashNewPassword(string password)
        {
            // Generate a new salt for hashing
            var salt = Convert.ToBase64String(Guid.NewGuid().ToByteArray());  // Ensure proper Base64 encoding

            // Generate the hashed password using PBKDF2
            var hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt), // Convert the salt back from Base64 to byte array
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            // Return the salt and hash in the format "salt:hash"
            return $"{salt}:{Convert.ToBase64String(hashed)}";  // Return as a concatenated Base64 string
        }


        public async Task<User?> GetUserByUserIDAsync(int userID)
        {
            return await _context.Users
               .Include(u => u.Role)
               .Include(u => u.Employee)
                .ThenInclude(e => e.School)
               .Include(u => u.Employee)
                .ThenInclude(e => e.EmployeeType)
               .FirstOrDefaultAsync(u => u.UserID == userID);
        }


    }
}
