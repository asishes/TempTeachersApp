using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.NonTeacherDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface INonTeacherInterface
    {
        Task<int> GetTotalActiveNonTeachersCountAsync();

        Task<IEnumerable<NonTeacherList>> GetNonTeachersListAsync();

        Task<IEnumerable<Employee>> GetDynamicListNonTeachersDataAsync(List<string>? statusTexts, int? schoolId);

        Task<Employee> GetNonTeacherHomePageAsync(int EmployeeID);

        Task<Employee> CreateNonTeacherAsync(Employee employee, CreateNonTeacherDTO employeeDto);

        Task<Employee?> UpdateNonTeacherAsync(int Id, Employee employee);

        Task<List<Employee>> GetAllRetiredNonTeacherAsync();

        Task<List<Employee>> GetAllNonTeacherOnLeaveAsync();

        Task<List<Employee>> GetNonTeachersBySchoolIDAsync(int schoolID);

        Task<IEnumerable<TeacherListDTO>> GetFilterListNonTeachersDataAsync(
       int? subjectId = null,
       int? retiringInMonths = null,
       int? schoolId = null,
       string? uniqueId = null,
       bool? DocumentsWithError = null,
       int? minExperienceYear = null,
       int? maxExperienceYear = null);

        Task<IEnumerable<Employee>> GetFilterDynamicListNonTeachersDataAsync(
       int? retiringInMonths = null,
       int? schoolId = null,
       string? uniqueId = null,
       int? minExperienceYear = null,
       int? maxExperienceYear = null,
       List<string>? statusTexts = null,
       int? additionalSchoolId = null);



        Task<List<Employee>> GetApprovedNonTeacherAsync();

        Task<List<Employee>> GetNonApprovedNonTeacherAsync();

        Task<TeacherStatusCountDTO> GetTotalNonTeachersStatusCountAsync();

        Task<List<Employee>> GetOnLeaveNonTeachersBySchoolIDAsync(int schoolID);

    }
}

