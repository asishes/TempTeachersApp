using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class DesignationQualification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DesQuaID { get; set; }

        [ForeignKey("Designation")]
        public int DesignationID { get; set; }

        [ForeignKey("Course")]
        public int QualificationID { get; set; }

        public Designation Designation { get; set; } = null!;

        public Course Course { get; set; } = null!;


    }
}
