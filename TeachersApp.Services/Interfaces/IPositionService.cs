using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.PositionDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface IPositionService
    {
        
        Task<int> GetTotalVacantPositionCountAsync();

        Task<List<SchoolPosition>> GetAllVacantAndNewSchoolPositionsAsync();

        Task<SchoolPosition> CreateVacancyAsync(SchoolPosition schoolPosition);

        Task<int> HandleVacanciesForEmployeesAsync();

        Task<SchoolPosition> CreateNewPositionAsync(SchoolPosition schoolPosition);

        Task<bool> DeleteSchoolPositionAsync(int id);


    }
}
