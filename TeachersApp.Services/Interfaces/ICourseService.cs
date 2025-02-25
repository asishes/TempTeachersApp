using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.CourseDTO;

namespace TeachersApp.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<GetEmployeeCourseDTO>> GetCoursesByEducationTypeIDAsync(int educationTypeId);
    }
}
