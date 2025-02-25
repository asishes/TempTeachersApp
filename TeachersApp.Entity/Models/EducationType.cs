using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class EducationType
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EducationTypeID { get; set; }

        [Required, MaxLength(50)]
        public string EductionTypeName { get; set; } = null!;



        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
