using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO
{
    public class GetEducationTypeDTO
    {
        public int EducationTypeID { get; set; }

        public string EducationTypeName { get; set; } = string.Empty;
    }
}
