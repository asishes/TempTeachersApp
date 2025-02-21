using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class EmployeeBloodGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BloodGroupID { get; set; }

        [Required, MaxLength(50)]
        public string BloodGroupName { get; set; } = null!;


        public ICollection<EmployeePersonalDetails> PersonalDetails { get; set; } = new List<EmployeePersonalDetails>();



    }
}
