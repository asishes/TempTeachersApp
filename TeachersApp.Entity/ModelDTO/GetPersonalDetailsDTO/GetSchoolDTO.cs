using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO
{
    public class GetSchoolDTO
    {
        public int SchoolId { get; set; }

        public string? SchoolName { get; set; } = string.Empty;

        public string? City { get; set; } = string.Empty;
    }
}
