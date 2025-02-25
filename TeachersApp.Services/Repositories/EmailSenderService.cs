using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.UserDTO.ForgotPasswordDTO;
using TeachersApp.Services.Interfaces;

namespace TeachersApp.Services.Repositories
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly TeachersAppDbcontext _context;

        public EmailSenderService(IConfiguration configuration, TeachersAppDbcontext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == forgotPasswordDTO.Email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Generate reset token and expiry (not stored in the User model)
            var token = GenerateResetToken();
            var tokenExpiryDate = DateTime.UtcNow.AddHours(1); // Example expiry

            // Construct the reset URL
            var resetUrl = $"{forgotPasswordDTO.ClientURl}?token={token}&email={user.Email}";

            // Send email
            await SendEmailAsync(user.Email, "Password Reset", $"Please reset your password using this link:{resetUrl}");

            
        }

        public async Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordDTO.Email);

            // Check if the user exists and validate the token here (you could use a cache/store)
            // For demonstration, assume token validation logic is implemented here

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (resetPasswordDTO.Password != resetPasswordDTO.ConfirmPassword)
            {
                throw new Exception("Passwords do not match.");
            }

            // Hash and update the new password
            user.PasswordHash = HashPassword(resetPasswordDTO.Password);

            // Note: No need to clear the token in the User model since it is not stored there

            await _context.SaveChangesAsync();
        }

        private async Task SendEmailAsync(string email, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, // Use 587 for TLS
                Credentials = new NetworkCredential(
            _configuration["EmailConfiguration:UserMail"],
            _configuration["EmailConfiguration:Password"]), // Use App Password if 2FA is enabled
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailConfiguration:FromMail"], _configuration["EmailConfiguration:DisplayName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true, // Set to true if your email body contains HTML
            };

            mailMessage.To.Add(email);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                // Log the exception or handle it
                throw new Exception("SMTP error occurred: " + smtpEx.Message);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it
                throw new Exception("Failed to send email: " + ex.Message);
            }
        }

        private string HashPassword(string password)
        {
            var salt = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{salt}:{hashed}";
        }

        private string GenerateResetToken()
        {
            return Guid.NewGuid().ToString(); // Generate a simple GUID token
        }
    }
}

