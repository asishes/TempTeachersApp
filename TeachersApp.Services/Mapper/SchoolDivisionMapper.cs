using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.SchoolDivisionDTO;
using TeachersApp.Entity.ModelDTO.SchoolTypeDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Mapper
{
    public static class SchoolDivisionMapper
    {
        
        public static SchoolDivisionCount AddSchoolDivision(this AddSchoolDivisionDTO schoolDivisionDto)
        {
            return new SchoolDivisionCount
            {
                SchoolClassID = schoolDivisionDto.SchoolClassID, 
                Division = schoolDivisionDto.Division,
                StudentCount = schoolDivisionDto.StudentCount
            };
        }

        
        public static GetSchoolDivisionDTO GetSchoolDivision(this SchoolDivisionCount schoolDivision)
        {
            return new GetSchoolDivisionDTO
            {
                SchoolDivisionID = schoolDivision.DivisionCountID, 
                SchoolID = schoolDivision.SchoolClass.SchoolID, 
                Division = schoolDivision.Division,
                StudentCount = schoolDivision.StudentCount
            };
        }





    }

}

