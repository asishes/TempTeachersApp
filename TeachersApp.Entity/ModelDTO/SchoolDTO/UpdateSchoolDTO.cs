using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.SchoolClassDTO;
using TeachersApp.Entity.ModelDTO.SchoolDivisionDTO;
using TeachersApp.Entity.ModelDTO.SchoolTypeDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;

namespace TeachersApp.Entity.ModelDTO.SchoolDTO
{
    public class UpdateSchoolDTO
    {
        public string SchoolName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int CityID { get; set; }
        public string State { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int? PhotoID { get; set; }
        public int? PrincipalID { get; set; }
        public int? VicePrincipalID { get; set; }

        

        public List<UpdateSchoolTypeWithSchoolDTO?> updateSchoolTypes { get; set; } = new List<UpdateSchoolTypeWithSchoolDTO?>();
        public List<UpdateClassWithSchoolDTO?> updateClasses {  get; set; } = new List<UpdateClassWithSchoolDTO?>();
    }
}
