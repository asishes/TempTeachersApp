using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class SchoolTypeDesignation
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchoolTypeDesignationID { get; set; }

        [ForeignKey("SchoolType")]
        public int SchoolTypeID { get; set; }

        [ForeignKey("Designation")]
        public int DesignationID { get; set; }


        public SchoolType SchoolType { get; set; } = null!;

        public Designation Designation { get; set; } = null!;
    }
}
