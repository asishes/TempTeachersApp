using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.SchoolDTO
{
    public class SchoolDTO
    {
        public int SchoolID { get; set; }

        public string SchoolName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string CityName { get; set; } = null!;

        public string State { get; set; } = null!;

        public string Pincode { get; set; } = null!;

        public string Email { get; set; } = string.Empty;

        public string ContactNumber { get; set; } = null!;

        public string? PrincipalName { get; set; }

        public string? VicePrincipalName { get; set; }
    }
}
