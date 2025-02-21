using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class SchoolType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchoolTypeID { get; set; }

        [Required, MaxLength(50)]
        public string SchoolTypeName { get; set; } = null!;

        public int Class { get; set; }



        public ICollection<SchoolStandardType> SchoolStandardTypes { get; set; } = new List<SchoolStandardType>();

        public ICollection<SchoolTypeDesignation> SchoolTypeDesignations { get; set; } = new List<SchoolTypeDesignation>();
    }
}
