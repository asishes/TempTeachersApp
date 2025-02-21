using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.SchoolTypeDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface ISchoolTypeService
    {
        Task<SchoolType> CreateSchoolTypeAsync(SchoolType schoolType);


        Task<GetSchoolTypeDTO> GetSchoolTypeByIdAsync(int schoolTypeId);


        Task<List<GetAllSchoolTypesDTO>> GetAllSchoolTypesAsync();

        Task<List<SchoolType>> GetClassesBySchoolTypeAsync(List<int> schooltypeIDs);
    }
}
