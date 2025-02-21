using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface ISchoolService
    {
        // Get Open School Count
        Task<int> GetTotalOpenSchoolCountAsync();

        Task<SchoolPopUpDTO> GetSchoolPopUpAsync(int schoolId);

        Task<List<School>> GetSchoolListAsync();

        Task<List<SchoolDTO>> GetSchoolListIdAsync(int? schoolId = null);

        Task<School> AddSchoolAsync(School school);

        Task<School> GetSchoolHomePageAsync(int schoolId);

        Task<IEnumerable<GetAllSchoolsWithCityDTO>> GetSchoolsWithCityAsync();

        Task<GetSchoolDTO> GetSchoolAsync(int schoolId);

        Task<School?> UpdateSchoolAsync(int Id, School schoolUpdate);

        Task<List<School>> GetSchoolWithAuthorityListAsync();


    }
}
