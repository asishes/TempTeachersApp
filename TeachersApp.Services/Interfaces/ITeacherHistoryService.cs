using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface ITeacherHistoryService
    {
        Task<List<TeacherHistory>> GetHistoriesByIdAsync(int EmployeeID);

        Task<TeacherHistory> CreateTeacherHistoryAsync(TeacherHistory teacherHistory);

        Task<int> GetChangeTypeIdAsync(string changeTypeName);

        Task HandleTeacherHistoryAsync(Employee employee);
    }
}
