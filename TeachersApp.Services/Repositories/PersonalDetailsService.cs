using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;

namespace TeachersApp.Services.Repositories
{
    public class PersonalDetailsService : IPersonalDetailsService
    {

        private readonly TeachersAppDbcontext _context;


        public PersonalDetailsService(TeachersAppDbcontext context)
        {

            _context = context;
        }


        public async Task<List<GetEducationTypeDTO>> GetAllEducationTypeAsync()
        {
            return await _context.EducationTypes
                .Select(s => new GetEducationTypeDTO
                {
                    // Map properties as needed
                    EducationTypeID = s.EducationTypeID,
                    EducationTypeName = s.EductionTypeName,
                })
                .ToListAsync();
        }
        public async Task<List<GetPhotoDTO>> GetAllPhotosAsync()
        {
            return await _context.Photos
                .Select(p => new GetPhotoDTO
                {
                    // Map properties as needed
                    PhotoID = p.PhotoID,
                    PhotoName = p.PhotoImageName,
                })
                .ToListAsync();
        }

        public async Task<List<GetSubjectDTO>> GetAllSubjectsAsync()
        {
            return await _context.Subjects
                .Select(s => new GetSubjectDTO
                {
                    // Map properties as needed
                    SubjectID = s.SubjectID,
                    SubjectName = s.SubjectText
                })
                .ToListAsync();
        }

        public async Task<List<GetStatusDTO>> GetAllStatusesAsync()
        {
            return await _context.Statuses
                .Select(s => new GetStatusDTO
                {
                    // Map properties as needed
                    StatusID = s.StatusID,
                    StatusName = s.StatusText,
                    StatusType = s.StatusType
                })
                .ToListAsync();
        }



        public async Task<List<GetSchoolDTO>> GetAllSchoolsAsync()
        {
            return await _context.Schools
                .Include(s => s.City)
                .Select(s => new GetSchoolDTO
                {
                    // Map properties as needed
                    SchoolId = s.SchoolID,
                    SchoolName = s.SchoolName,
                    City = s.City.CityName ?? "N/A"
                    
                })
                .ToListAsync();
        }

        public async Task<List<GetReligionDTO>> GetAllEmployeeReligionsAsync()
        {
            return await _context.EmployeeReligions
                .Select(r => new GetReligionDTO
                {
                    // Map properties as needed
                    ReligionID = r.ReligionID,
                    ReligionName = r.ReligionName
                })
                .ToListAsync();
        }

        public async Task<List<GetMaritalStatusDTO>> GetAllEmployeeMaritalStatusesAsync()
        {
            return await _context.employeeMaritalStatuses
                .Select(ms => new GetMaritalStatusDTO
                {
                    // Map properties as needed
                    MaritalStatusID = ms.MaritalStatusID,
                    MaritalStatusName = ms.MaritalStatusName
                })
                .ToListAsync();
        }

        public async Task<List<GetGenderDTO>> GetAllEmployeeGendersAsync()
        {
            return await _context.EmployeeSexes
                .Select(g => new GetGenderDTO
                {
                    // Map properties as needed
                    GenderID = g.SexID,
                    GenderName = g.Sex
                })
                .ToListAsync();
        }

        public async Task<List<GetEmployeeTypeDTO>> GetAllEmployeeTypesAsync()
        {
            return await _context.EmployeeTypes
                .Select(et => new GetEmployeeTypeDTO
                {
                    // Map properties as needed
                    EmployeeTypeID = et.EmployeeTypeID,
                    EmployeeTypeName = et.Employeetype
                })
                .ToListAsync();
        }

        public async Task<List<GetEmployeeDTO>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Select(e => new GetEmployeeDTO
                {
                    // Map properties as needed
                    EmployeeID = e.EmployeeID,
                    EmployeeName = e.FirstName
                    // LastName = e.LastName,
                    // Other properties
                })
                .ToListAsync();
        }

        public async Task<List<GetEmployeeCategoryDTO>> GetAllEmployeeCategoriesAsync()
        {
            return await _context.EmployeeCategories
                .Select(ec => new GetEmployeeCategoryDTO
                {
                    // Map properties as needed
                    EmployeeCategoryId = ec.CategoryID,
                    EmployeeCategoryName = ec.CategoryName
                })
                .ToListAsync();
        }

        public async Task<List<GetDistrictDTO>> GetAllDistrictsAsync()
        {
            return await _context.Districts
                .Select(d => new GetDistrictDTO
                {
                    // Map properties as needed
                    DistrictID = d.DistrictID,
                    DistrictName = d.DistrictName
                })
                .ToListAsync();
        }

        public async Task<List<GetDesignationDTO>> GetAllDesignationsAsync()
        {
            return await _context.Designations
                .Select(d => new GetDesignationDTO
                {
                    // Map properties as needed
                    DesignationID = d.DesignationID,
                    DesignationName = d.DesignationText
                })
                .ToListAsync();
        }

        public async Task<List<GetCourseDTO>> GetAllCoursesAsync()
        {
            return await _context.Courses
                .Select(c => new GetCourseDTO
                {
                    // Map properties as needed
                    CourseID = c.CourseID,
                    CourseName = c.CourseName
                })
                .ToListAsync();
        }

        public async Task<List<GetCityDTO>> GetAllCitiesAsync()
        {
            return await _context.Cities
                .Select(c => new GetCityDTO
                {
                    // Map properties as needed
                    CityID = c.CityID,
                    City = c.CityName
                })
                .ToListAsync();
        }

        public async Task<List<GetCasteCategoryDTO>> GetAllCasteCategoriesAsync()
        {
            return await _context.EmployeeCasteCategories
                .Select(cc => new GetCasteCategoryDTO
                {
                    // Map properties as needed
                    CasteCategoryID = cc.CasteCategoryID,
                    CasteCategoryName = cc.CasteCategoryName
                })
                .ToListAsync();
        }

        public async Task<List<GetBloodGroupDTO>> GetAllBloodGroupsAsync()
        {
            return await _context.EmployeeBloodGroups
                .Select(bg => new GetBloodGroupDTO
                {
                    // Map properties as needed
                    BloodGroupID = bg.BloodGroupID,
                    BloodGroupName = bg.BloodGroupName
                })
                .ToListAsync();
        }

        public async Task<List<GetApprovalTypeDTO>> GetAllApprovalTypesAsync()
        {
            return await _context.ApprovalTypes
                .Select(at => new GetApprovalTypeDTO
                {
                    // Map properties as needed
                    ApprovalTypeID = at.ApprovalTypeID,
                    ApprovalTypeName = at.Approvaltype
                })
                .ToListAsync();
        }

        public async Task<Subject> GetSubjectByIdAsync(int id)
        {
            return await _context.Subjects.FindAsync(id) ?? null;
        }

        public async Task<Status> GetStatusByIdAsync(int id)
        {
            return await _context.Statuses.FindAsync(id) ?? null;
        }

        public async Task<School> GetSchoolByIdAsync(int id)
        {
            return await _context.Schools.FindAsync(id) ?? null;
        }

        public async Task<EmployeeReligion> GetEmployeeReligionByIdAsync(int id)
        {
            return await _context.EmployeeReligions.FindAsync(id) ?? null;
        }

        public async Task<EmployeeMaritalStatus> GetEmployeeMaritalStatusByIdAsync(int id)
        {
            return await _context.employeeMaritalStatuses.FindAsync(id) ?? null;
        }

        public async Task<EmployeeSex> GetEmployeeGenderByIdAsync(int id)
        {
            return await _context.EmployeeSexes.FindAsync(id) ?? null;
        }

        public async Task<EmployeeType?> GetEmployeeTypeByIdAsync(int id)
        {
            return await _context.EmployeeTypes.FindAsync(id) ?? null;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id) ?? null;
        }

        public async Task<EmployeeCategory> GetEmployeeCategoryByIdAsync(int id)
        {
            return await _context.EmployeeCategories.FindAsync(id) ?? null;
        }

        public async Task<District> GetDistrictByIdAsync(int id)
        {
            return await _context.Districts.FindAsync(id) ?? null;
        }

        public async Task<Designation> GetDesignationByIdAsync(int id)
        {
            return await _context.Designations.FindAsync(id) ?? null;
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id) ?? null;
        }

        public async Task<City> GetCityByIdAsync(int id)
        {
            return await _context.Cities.FindAsync(id) ?? null;
        }

        public async Task<EmployeeCasteCategory> GetCasteCategoryByIdAsync(int id)
        {
            return await _context.EmployeeCasteCategories.FindAsync(id) ?? null;
        }

        public async Task<EmployeeBloodGroup> GetBloodGroupByIdAsync(int id)
        {
            return await _context.EmployeeBloodGroups.FindAsync(id) ?? null;
        }

        public async Task<ApprovalType> GetApprovalTypeByIdAsync(int id)
        {
            return await _context.ApprovalTypes.FindAsync(id) ?? null;
        }

        public async Task<SchoolType> GetSchoolTypeByIdAsync(int id)
        {
            return await _context.SchoolTypes.FindAsync(id) ?? null;
        

        }

        public async Task<Photo> GetPhotoByIdAsync(int id)
        {
            return await _context.Photos.FindAsync(id) ?? null;


        }
    }
}

