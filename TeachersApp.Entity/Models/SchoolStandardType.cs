using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class SchoolStandardType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchoolStandardTypeID { get; set; }

        [ForeignKey("School")]
        public int SchoolID { get; set; }

        [ForeignKey("SchoolType")]
        public int SchoolTypeID { get; set; }


        public School School { get; set; } = null!;

        public SchoolType SchoolType { get; set; } = null!;
    }
}
