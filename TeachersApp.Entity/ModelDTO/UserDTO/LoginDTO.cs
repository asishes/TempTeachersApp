using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.UserDTO
{
    public class LoginDTO
    {
        [Required]
        [StringLength(50)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
