using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeachersApp.Entity.ModelDTO.UserDTO.ForgotPasswordDTO;
using TeachersApp.Services.Interfaces;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly IEmailSenderService _emailSenderService;

        public PasswordResetController(IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _emailSenderService.ForgotPasswordAsync(forgotPasswordDTO);
                return Ok(new { message = "Password reset email sent successfully.", statusCode = 200 });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

     
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _emailSenderService.ResetPasswordAsync(resetPasswordDTO);
                return Ok("Password has been reset successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
