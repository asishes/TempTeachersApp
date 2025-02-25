using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectID { get; set; }

        [Required, MaxLength(100)]
        public string SubjectText { get; set; } = null!;




        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<SchoolPosition> SchoolPositions { get; set; } = new List<SchoolPosition>();
    }
}
