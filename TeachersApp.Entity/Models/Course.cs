using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseID { get; set; }

        [Required, MaxLength(100)]
        public string CourseName { get; set; } = null!;

        [ForeignKey("EducationType")]
        public int EducationTypeID { get; set; }




        public EducationType EducationType { get; set; } = null!;


        public ICollection<EmployeeEducation> EmployeeEducations { get; set; } = new List<EmployeeEducation>();

        public ICollection<DesignationQualification> DesignationQualifications { get; set; } = new List<DesignationQualification>();
    }
}
