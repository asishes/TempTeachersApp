using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.SchoolClassDTO;
using TeachersApp.Entity.ModelDTO.SchoolDivisionDTO;
using TeachersApp.Entity.ModelDTO.SchoolTypeDTO;

namespace TeachersApp.Entity.ModelDTO.SchoolDTO
{
    public class SchoolListDTO
    {

        public int SchoolId { get; set; }

        public string Name { get; set; } = string.Empty;  

        public string Contact { get; set; } = string.Empty;

        public string City {  get; set; } = string.Empty; 

        public string Principal {  get; set; } = string.Empty; 
        
        public int NoOfTeachers { get; set; } 

        public int NoOfStudents { get; set; }

        public List<GetSchoolTypeWithSchoolDTO> getSchoolTypes { get; set; } = new List<GetSchoolTypeWithSchoolDTO>();
        public List<GetClassWithSchoolDTO> getClasses { get; set; } = new List<GetClassWithSchoolDTO>();


    }
}
