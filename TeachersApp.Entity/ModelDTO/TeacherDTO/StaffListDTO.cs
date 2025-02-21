using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class StaffListDTO
    {
        public string? Name { get; set; } = string.Empty;

        public DateTime? DOB { get; set; }

        public string? Phone { get; set; } = string.Empty;

        public DateTime? JoinDate { get; set; }

        public string? Position { get; set; } = string.Empty;

        public string? Subject { get; set;} = string.Empty;

        public string? EmployeeType { get; set; } = string.Empty;

        public TeacherPopUpDTO TeacherPopUpDTO { get; set; } = new TeacherPopUpDTO();
    }
}
