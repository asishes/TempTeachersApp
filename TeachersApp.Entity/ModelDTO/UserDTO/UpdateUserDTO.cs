using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.UserDTO
{
    public class UpdateUserDTO
    {
        [Required]
        [StringLength(255, ErrorMessage = "First Name cannot be longer than 255 characters.")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(255, ErrorMessage = "Last Name cannot be longer than 255 characters.")]
        public string LastName { get; set; } = null!;


        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}
