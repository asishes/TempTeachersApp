using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.SchoolDivisionDTO;
using TeachersApp.Entity.ModelDTO.SchoolTypeDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface ISchoolDivisionService
    {
        Task<SchoolDivisionCount> AddSchoolDivisionAsync(SchoolDivisionCount schoolDivision);


        Task<GetSchoolDivisionDTO> GetSchoolDivisionByIdAsync(int schoolDivisionId);
    }
}
