using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeachersApp.Entity.ModelDTO.UserDTO;
using TeachersApp.Entity.ModelDTO.UserDTO.ResetPasswordDTO;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using TeachersApp.Filters;
using TeachersApp.Services.Repositories;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
    

        public UserController(IUserService userService)
        {
            _userService = userService;
            
        }



        #region Register

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO dto)
        {
            try
            {
                await _userService.RegisterUserAsync(dto);
                return Ok(new { Message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                // Return a meaningful error message based on the exception
                if (ex.Message.Contains("Username is already taken"))
                {
                    return Conflict(new { Message = ex.Message }); // 409 Conflict
                }
                if (ex.Message.Contains("Email is already registered"))
                {
                    return Conflict(new { Message = ex.Message }); // 409 Conflict
                }
                return BadRequest(new { Message = ex.Message }); // 400 Bad Request for other errors
            }
        }

        #endregion



        #region Login

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Authenticate and get token
                var token = await _userService.AuthenticateAsync(loginDTO);

                // Handle "Remember Me" functionality
                if (loginDTO.RememberMe)
                {
                    // Set cookies for auto-fill and token
                    Response.Cookies.Append("Email", loginDTO.Email, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(30),
                        HttpOnly = true,
                        Secure = true
                    });

                    Response.Cookies.Append("Password", loginDTO.Password, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(30),
                        HttpOnly = true,
                        Secure = true
                    });

                    Response.Cookies.Append("AuthToken", token, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(2),
                        HttpOnly = true,
                        Secure = true
                    });
                }
                else
                {
                    Response.Cookies.Delete("Email");
                    Response.Cookies.Delete("Password");
                    Response.Cookies.Delete("AuthToken");
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        #endregion



        #region Logout

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("Password");
            Response.Cookies.Delete("AuthToken");

            return Ok(new { Message = "Logged out successfully." });
        }

        #endregion



        #region GetUserByToken

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUserById()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the token from the Authorization header
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required" });
            }

            try
            {
                var user = await _userService.GetUserByTokenAsync(token);
                if (user == null)
                {
                    return NotFound(new { message = "No data found" });
                }

                var userDTO = user.ToGetUserDTO();
                return Ok(userDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        #endregion


        [Authorize]
        [HttpGet("get-user/{userID}")]
        public async Task<IActionResult> GetUserByUserId(int userID)
        {
            try
            {
       

                var getuser = await _userService.GetUserByUserIDAsync(userID);
                if (getuser == null)
                {
                    return NotFound(new { message = "No data found" });
                }

                var userDTO = getuser.ToGetUserDTO();
                return Ok(userDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error retrieving data: {ex.Message}" });
            }
        }


        [Authorize]
        [HttpPatch("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO userUpdate)
        {
            if (userUpdate == null)
            {
                return BadRequest("Invalid user data.");
            }

            var updatedUser = await _userService.UpdateUserAsync(id, userUpdate);

            if (updatedUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            return StatusCode(200, new { message = "Updated successfully", status = 200 });
        }

        [Authorize]
        [HttpPost("ResetPasswordByUser")]
        public async Task<IActionResult> ResetPasswordByUser([FromBody] ResetPasswordByUserDTO resetPasswordDto)
        {
            if (resetPasswordDto == null)
            {
                return BadRequest("Invalid request.");
            }

            try
            {
                // Get the token from the request headers
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

                // Call the service to reset the password
                var result = await _userService.ResetPasswordByUserAsync(resetPasswordDto, token);

                if (result)
                {
                    return Ok(new { message = "Password reset successfully." });
                }
                else
                {
                    return BadRequest(new { message = "Password reset failed." });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }



        [HttpGet("admin")]
        [Authorize(Policy = "AdminOnly")] // Require Admin role for this action
        public IActionResult GetAdminData()
        {
            return Ok("This is protected admin data.");
        }
    }
}
