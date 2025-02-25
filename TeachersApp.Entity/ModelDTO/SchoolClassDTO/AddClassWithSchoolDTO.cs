using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.SchoolDivisionDTO;

namespace TeachersApp.Entity.ModelDTO.SchoolClassDTO
{
    public class AddClassWithSchoolDTO
    {
        public int Class { get; set; }

        public List<AddDivisionAndStudentCountWithClassDTO> AddDivisions { get; set; } = new List<AddDivisionAndStudentCountWithClassDTO>();
    }
}
