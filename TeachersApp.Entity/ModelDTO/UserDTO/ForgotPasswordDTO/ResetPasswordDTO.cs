using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.UserDTO.ForgotPasswordDTO
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage ="Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Password and Confirm Password does not match")]
        public string? ConfirmPassword { get; set; }

        public string? Email { get; set; }

        public string? Token { get; set; }

    }
}
