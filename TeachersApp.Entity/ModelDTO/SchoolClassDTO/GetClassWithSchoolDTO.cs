using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.SchoolClassDTO
{
    public class GetClassWithSchoolDTO
    {
        public int Class { get; set; }

        public List<GetDivitionWithClassDTO> getDivisions { get; set; } = new List<GetDivitionWithClassDTO>();
        public int TotalDivisions { get; set; }
        public int TotalStudents { get; set; }
    }
}
