using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class EmployeeEducationDTO
    {
        public int? EducationTypeID { get; set; }
        public int? CourseID { get; set; }
        public string? CourseName { get; set; }
        public string? SchoolName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DocumentID { get; set; }

       
    }
}
