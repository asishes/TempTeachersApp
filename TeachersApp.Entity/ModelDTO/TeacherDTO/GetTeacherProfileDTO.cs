using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class GetTeacherProfileDTO
    {
        public string? PhotoPath { get; set; }

        public string TeacherName { get; set; } = string.Empty;

        public string SchoolName { get; set; } = string.Empty ;

        public string UniqueID { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty ;

        public string Religion { get; set; } = string.Empty ;

        public string Caste { get; set; } = string.Empty;

        public string CasteCategory { get; set;} = string.Empty ;

        public string BloodGroup { get; set;  } = string.Empty ;

        public string IdentificationMark1 { get; set;} = string.Empty ;

        public string IdentificationMark2 { get; set;} = string.Empty ;

        public double Height { get; set;} 

        public string FatherName { get; set;} = string.Empty ;

        public string MotherName { get; set;} =string.Empty ;

        public bool InterReligion { get; set; }

        public string MaritalStatus { get; set;} = string.Empty ;   

        public string SpouseName { get; set; } = string.Empty;

        public string SpouseReligion { get; set;} = string.Empty ;

        public string SpouseCaste { get;set;} = string.Empty ;

        public string PanID { get; set;} = string.Empty ;

        public string VoterID { get; set; } = string.Empty;

    }
}
