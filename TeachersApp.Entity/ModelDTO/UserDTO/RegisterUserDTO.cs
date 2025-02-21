using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.UserDTO
{
    public class RegisterUserDTO
    {
        [Required]
        [StringLength(255, ErrorMessage = "Username cannot be longer than 255 characters.")]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(255, ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [StringLength(255, ErrorMessage = "First Name cannot be longer than 255 characters.")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(255, ErrorMessage = "Last Name cannot be longer than 255 characters.")]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int RoleID { get; set; }
        public int? EmployeeID { get; set; }

    }
}
