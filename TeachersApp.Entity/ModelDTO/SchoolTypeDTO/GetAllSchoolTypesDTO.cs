using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.SchoolTypeDTO
{
    public class GetAllSchoolTypesDTO
    {
        public int SchoolTypeID { get; set; }

        public string SchoolTypeName { get; set; } = string.Empty;
    }
}
