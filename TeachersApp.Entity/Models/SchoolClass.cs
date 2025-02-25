using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class SchoolClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchoolClassID { get; set; }

        [ForeignKey("School")]
        public int SchoolID { get; set; }

        public int Class { get; set; }



        public School School { get; set; } = null!;

        public ICollection<SchoolDivisionCount> SchoolDivisionCounts { get; set; } = new List<SchoolDivisionCount>();
    }
}
