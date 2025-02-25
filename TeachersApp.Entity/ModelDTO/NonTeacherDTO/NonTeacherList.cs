using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.TeacherDTO;

namespace TeachersApp.Entity.ModelDTO.NonTeacherDTO
{
    public class NonTeacherList
    {

        public int TeacherId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? SchoolId { get; set; }
        public string SchoolName { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public int ExperienceYear { get; set; }
        public int Age { get; set; }
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;

        public TeacherPopUpDTO TeacherPopUpDTO { get; set; } = new TeacherPopUpDTO();
    }
}
