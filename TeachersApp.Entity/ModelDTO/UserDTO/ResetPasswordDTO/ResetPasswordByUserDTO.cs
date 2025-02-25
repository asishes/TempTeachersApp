using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.UserDTO.ResetPasswordDTO
{
    public class ResetPasswordByUserDTO
    {

        [Required(ErrorMessage = "Password is required")]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Password and Confirm Password does not match")]
        public string? ConfirmNewPassword { get; set; }

    }
}
