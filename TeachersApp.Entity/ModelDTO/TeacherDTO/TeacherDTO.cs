using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class TeacherDTO
    {
        public int TeacherID { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "First Name cannot be longer than 255 characters.")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(255, ErrorMessage = "Last Name cannot be longer than 255 characters.")]
        public string LastName { get; set; } = null!;

        [StringLength(255, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255, ErrorMessage = "Phone cannot be longer than 255 characters.")]
        public string Phone { get; set; } = null!;

        [Required]
        [StringLength(255, ErrorMessage = "Address cannot be longer than 255 characters.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Hire Date is required.")]
        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? RetirementDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Designation ID must be a positive integer.")]
        public string? Designation { get; set; }

        [Required]
        public string? School { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public string? Supervisor { get; set; }
    }
}
