using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class SchoolPosition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PositionID { get; set; }

        [ForeignKey("Designation")]
        public int DesignationID { get; set; }

        [ForeignKey("Subject")]
        public int SubjectID { get; set; }

        [ForeignKey("School")]
        public int SchoolID { get; set; }

        [ForeignKey("Status")]
        public int StatusID { get; set; }

        public DateTime Date { get; set; }



        public Designation Designation { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
        public School School { get; set; } = null!;
        public Status Status { get; set; } = null!;

        public ICollection<Employee> EmployeesPosition { get; set; } = new List<Employee>();
    }
}
