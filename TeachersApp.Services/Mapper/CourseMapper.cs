using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.CourseDTO;
using TeachersApp.Entity.ModelDTO.SchoolDivisionDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Mapper
{
    public static class CourseMapper
    {
        public static GetEmployeeCourseDTO ToetCoursesByEducationType(this Course course)
        {
            return new GetEmployeeCourseDTO
            {
               CourseID=course.CourseID,
               CourseName=course.CourseName,
            };
        }
    }
}
