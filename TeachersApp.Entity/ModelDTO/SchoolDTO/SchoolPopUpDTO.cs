using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.SchoolDTO
{
    public class SchoolPopUpDTO
    {
        public string? Photo { get; set; }

        public int? PhotoId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty ;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty; 

        public string? Principal { get; set; } = string.Empty;

        public string? PrincipalPhotoPath { get; set; }

        public string? VicePrincipal { get; set; } = string.Empty;

        public string? VicePrincipalPhotoPath { get; set; }


    }
}
