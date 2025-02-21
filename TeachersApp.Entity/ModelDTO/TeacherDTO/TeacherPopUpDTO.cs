using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class TeacherPopUpDTO
    {
        public string? Photo { get; set; } 

        public string Name { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string SchoolName { get; set; } = string.Empty;

        public string? Email { get; set; } = string.Empty;
        
        public string? PhoneNumber { get; set; } = string.Empty;

        public DateTime? DateofJoin { get; set; }

        public string ReportedTo { get; set; } = null!;

    }
}
