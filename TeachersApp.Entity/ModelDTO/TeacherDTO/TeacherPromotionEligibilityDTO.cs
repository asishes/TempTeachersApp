using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.Models;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class TeacherPromotionEligibilityDTO
    {

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Subject { get; set; } = null!;

        public int ExperienceYear { get; set; }

        public int? Age { get; set; }    

        public string? FromDesignation { get; set; }


        public string? ToDesignation { get; set; }


        public string? SchoolName { get; set; }

        public string? QualifiedExam {  get; set; }

        public bool PromotionRelinquishment { get; set; } // the employee has any PromotionRelinquishment make true otherwise false 

        public TeacherPopUpDTO TeacherPopUpDTO { get; set; } = new TeacherPopUpDTO();


    }
}
