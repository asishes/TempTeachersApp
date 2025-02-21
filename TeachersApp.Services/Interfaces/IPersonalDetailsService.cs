using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface IPersonalDetailsService
    {
        Task<List<GetSubjectDTO>> GetAllSubjectsAsync();

        Task<List<GetPhotoDTO>> GetAllPhotosAsync();

        Task<List<GetEducationTypeDTO>> GetAllEducationTypeAsync();

        Task<List<GetStatusDTO>> GetAllStatusesAsync();

        Task<List<GetSchoolDTO>> GetAllSchoolsAsync();

        Task<List<GetReligionDTO>> GetAllEmployeeReligionsAsync();


        Task<List<GetMaritalStatusDTO>> GetAllEmployeeMaritalStatusesAsync();

        Task<List<GetGenderDTO>> GetAllEmployeeGendersAsync();

        Task<List<GetEmployeeTypeDTO>> GetAllEmployeeTypesAsync();

        Task<List<GetEmployeeDTO>> GetAllEmployeesAsync();

        Task<List<GetEmployeeCategoryDTO>> GetAllEmployeeCategoriesAsync();


        Task<List<GetDistrictDTO>> GetAllDistrictsAsync();

        Task<List<GetDesignationDTO>> GetAllDesignationsAsync();

        Task<List<GetCourseDTO>> GetAllCoursesAsync();

        Task<List<GetCityDTO>> GetAllCitiesAsync();

        Task<List<GetCasteCategoryDTO>> GetAllCasteCategoriesAsync();

        Task<List<GetBloodGroupDTO>> GetAllBloodGroupsAsync();

        Task<List<GetApprovalTypeDTO>> GetAllApprovalTypesAsync();

        Task<Subject> GetSubjectByIdAsync(int id);
        Task<Status> GetStatusByIdAsync(int id);
        Task<School> GetSchoolByIdAsync(int id);
        Task<EmployeeReligion> GetEmployeeReligionByIdAsync(int id);
        Task<EmployeeMaritalStatus> GetEmployeeMaritalStatusByIdAsync(int id);
        Task<EmployeeSex> GetEmployeeGenderByIdAsync(int id);
        Task<EmployeeType?> GetEmployeeTypeByIdAsync(int id);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<EmployeeCategory> GetEmployeeCategoryByIdAsync(int id);
        Task<District> GetDistrictByIdAsync(int id);
        Task<Designation> GetDesignationByIdAsync(int id);
        Task<Course> GetCourseByIdAsync(int id);
        Task<City> GetCityByIdAsync(int id);
        Task<EmployeeCasteCategory> GetCasteCategoryByIdAsync(int id);
        Task<EmployeeBloodGroup> GetBloodGroupByIdAsync(int id);
        Task<ApprovalType> GetApprovalTypeByIdAsync(int id);
        Task<SchoolType> GetSchoolTypeByIdAsync(int id);
        Task<Photo> GetPhotoByIdAsync(int id);




    }
}
