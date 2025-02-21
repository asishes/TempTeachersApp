using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class EmployeeEducation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeecourseID { get; set; }

        [ForeignKey("Employee")]
        public int? EmployeeID { get; set; }


        [ForeignKey("Course")]
        public int? CourseID { get; set; }


        [MaxLength(255)]
        public string? CourseName { get; set; }

        [MaxLength(255)]
        public string? University { get; set; } 

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        [ForeignKey("Document")]
        public int? DocumentID { get; set; }




        public Employee Employee { get; set; } = null!;
        public Course Course { get; set; } = null!;
        public Document? Document { get; set; } 
    }
}
