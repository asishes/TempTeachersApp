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
    public class GetSchoolDTO
    {
        public int SchoolID { get; set; }
        public string SchoolName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int CityID { get; set; }
        public string State { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public int? PhotoID { get; set; }
        public int StatusID { get; set; }
        public int? PrincipalID { get; set; }
        public int? VicePrincipalID { get; set; }

        public string? CityName { get; set; }
        public string? PrincipalName { get; set; }

        public string? PrincipalPhone { get; set; }
        public string? VicePrincipalName { get; set; }

        public string? VicePrincipalPhone { get; set; }
        public string? StatusName { get; set; }

        public List<GetSchoolTypeWithSchoolDTO> getSchoolTypes { get; set; } = new List<GetSchoolTypeWithSchoolDTO>();
        public List<GetClassWithSchoolDTO> getClasses { get; set; } = new List<GetClassWithSchoolDTO>();
    }
}
