using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.SchoolDivisionDTO;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using TeachersApp.Entity.ModelDTO.SchoolTypeDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Mapper
{
    public static class SchoolTypeMapper
    {
        public static SchoolType AddSchoolType(this AddSchoolTypeDTO schoolTypeDto)
        {
            return new SchoolType
            {
                SchoolTypeName = schoolTypeDto.SchoolTypeName,
                Class = schoolTypeDto.Class
            };
        }

        public static GetSchoolTypeDTO GetSchoolType(this SchoolType schoolType)
        {
            return new GetSchoolTypeDTO
            {
                SchoolTypeID = schoolType.SchoolTypeID,
                SchoolTypeName = schoolType.SchoolTypeName,
                Classes = schoolType.Class
            };
        }

        public static GetAllSchoolTypesDTO ToGetAllSchoolTypes(this SchoolType schoolType)
        {
            return new GetAllSchoolTypesDTO
            {
                SchoolTypeID = schoolType.SchoolTypeID,
                SchoolTypeName = schoolType.SchoolTypeName,
            };
        }
        public static GetClassesBySchoolTypeDTO ToGetClasses(this SchoolType schoolType)
        {
            
                // Determine the range of classes based on the SchoolType's Class property
                int startClass = 1; // Default start class
                int endClass = schoolType.Class; // End class as defined in the `Class` property

                // Adjust the start class based on the total number of classes
                if (endClass <= 4) startClass = 1;     // LP School
                    else if (endClass <= 7) startClass = 5; // UP School
                    else if (endClass <= 10) startClass = 8; // High School
                    else if (endClass <= 12) startClass = 11; // Higher Secondary School

            // Generate the class range
            return new GetClassesBySchoolTypeDTO
            {
                Classes = Enumerable.Range(startClass, endClass - startClass + 1).ToList()
            };
        
        }
    }
}
