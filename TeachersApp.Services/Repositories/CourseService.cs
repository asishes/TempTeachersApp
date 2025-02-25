using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.CourseDTO;
using TeachersApp.Entity.ModelDTO.SchoolTypeDTO;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;

namespace TeachersApp.Services.Repositories
{
    public class CourseService : ICourseService
    {
        private readonly TeachersAppDbcontext _context;

        public CourseService(TeachersAppDbcontext context)
        {
            _context = context;
        }

        public async Task<List<GetEmployeeCourseDTO>> GetCoursesByEducationTypeIDAsync(int educationTypeId)
        {
            var courses = await _context.Courses
                                        .Where(c => c.EducationType.EducationTypeID == educationTypeId)
                                        .ToListAsync();

            if (!courses.Any())
            {
                throw new Exception("No courses found for the given EducationTypeId");
            }

            return courses.Select(course => course.ToetCoursesByEducationType()).ToList();
        }
    }
}
