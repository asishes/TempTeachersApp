using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.SchoolClassDTO
{
    public class UpdateClassWithSchoolDTO
    {
        public int Class { get; set; }

        public List<UpdateDivisionAndStudentCountDTO> updateDivisions { get; set; } = new List<UpdateDivisionAndStudentCountDTO>();
    }
}
