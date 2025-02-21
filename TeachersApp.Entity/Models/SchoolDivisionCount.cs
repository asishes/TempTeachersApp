using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class SchoolDivisionCount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DivisionCountID { get; set; }

        [ForeignKey("SchoolClass")]
        public int SchoolClassID { get; set; }
        [Required]
        public string Division { get; set; } = string.Empty;
        [Required]
        public int StudentCount { get; set; }


        public SchoolClass SchoolClass { get; set; } = null!;
    }
}
