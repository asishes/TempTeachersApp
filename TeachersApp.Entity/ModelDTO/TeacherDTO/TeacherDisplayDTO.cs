using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class TeacherDisplayDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int ExperienceYear { get; set; }
        public int Age { get; set; }
        public string Address { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
        public int? DocumentUpload { get; set; }
        public string? Error { get; set; } // Accepts null values
    }
}
