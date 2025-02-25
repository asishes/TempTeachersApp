using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class GetEmployeeEducationDTO
    {
        public int? EducationTypeID { get; set; }

        public string? EducationTypeName { get; set; }
        public int? CourseID { get; set; }
        public string? CourseName { get; set; }
        public string? CourseText { get; set; }
        public string? SchoolName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DocumentID { get; set; }
        public string? Documentpath { get; set; }
    }
}
