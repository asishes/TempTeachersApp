using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.CourseDTO;
using TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface ITeacherService
    {

        // GetTotalActiveTeachersCount 
        Task<int> GetTotalActiveTeachersCountAsync();


        Task<int> GetPromotionEligibleTeachersCountAsync();

        Task<IEnumerable<Employee>> GetListTeachersDataAsync();


        Task<IEnumerable<TeacherListDTO>> GetFilterListTeachersDataAsync(
        int? subjectId = null,
        int? retiringInMonths = null,
        int? schoolId = null,
        string? uniqueId = null,
        bool? DocumentsWithError = null,
        int? minExperienceYear = null,
        int? maxExperienceYear = null);

        Task<IEnumerable<Employee>> GetFilterDynamicListTeachersDataAsync(
       int? subjectId = null,
       int? retiringInMonths = null,
       int? schoolId = null,
       string? uniqueId = null,
       int? minExperienceYear = null,
       int? maxExperienceYear = null,
       List<string>? statusTexts = null,
       int? additionalSchoolId = null);




        //GetTeacherPopUP
        Task<TeacherPopUpDTO> GetTeacherPopUpAsync(int teacherId);


        Task<Employee> CreateEmployeesAsync(Employee employee, CreateEmployeeDTO employeeDto);


        Task<Employee?> GetEmployeeByIdAsync (int EmployeeID);


        Task<Employee> GetEmployeeByUniqueIDAsync(string UniqueID);


        Task<Employee?> UpdateEmployeeAsync(int Id, Employee employee);

        Task<List<Employee>> GetAllRetiredTeacherAsync();

        Task<List<Employee>> GetAllTeacherOnLeaveAsync();

        Task<List<Employee>> GetTeachersBySchoolIDAsync(int schoolID);

        Task<List<Employee>> GetEmployeesBySchoolIDAndSchoolTypeIdAndEmployeeTypeIDAsync(int? schoolID = null, List<int?> schooltypeIDs = null,List< int?> employeeTypeID = null);

        Task ProcessRetirementsAsync();

        Task<List<Employee>> GetPromotionEligibleEmployeeList();

        Task<List<Employee>> GetApprovedTeacherAsync();

        Task<List<Employee>> GetNonApprovedTeacherAsync();

        Task<Employee> GetPromotedDesignationByEmployeeIDAsync(int employeeID);

        Task<IEnumerable<Employee>> GetDynamicListTeachersDataAsync(List<string>? statusTexts, int? schoolId);

        Task<Employee?> ApproveEmployeeByHeadMasterAsync(int Id);

        Task<Employee?> ApproveEmployeeByManagerAsync(int Id);

        Task<TeacherStatusCountDTO> GetTotalTeachersStatusCountAsync();

        Task<List<Employee>> GetEmployeeOrderByPromotionSeniorityBySchoolIDAsync(int schoolID);

        Task<List<Employee>> GetOnLeaveTeachersBySchoolIDAsync(int schoolID);












    }


}
