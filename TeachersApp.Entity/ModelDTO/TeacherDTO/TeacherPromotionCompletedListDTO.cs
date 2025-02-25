using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class TeacherPromotionCompletedListDTO
    {
        public int TeacherId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? SchoolId { get; set; }
        public string SchoolName { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int ExperienceYear { get; set; }
        public int Age { get; set; }
        public DateTime? PromotionDate { get; set; }
        public string? PhoneNumber { get; set; } = string.Empty;

        //Only Count Document with StatusText=Approved
        public int DocumentCount { get; set; }

        //When any Document with StatusText= Pending return true
        public bool Error { get; set; }


        public TeacherPopUpDTO TeacherPopUpDTO { get; set; } = new TeacherPopUpDTO();
    }
}
