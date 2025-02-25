using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class EmployeeReligion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReligionID { get; set; }

        [Required, MaxLength(10)]
        public string ReligionName { get; set; } = null!;


        public ICollection<EmployeePersonalDetails> PersonalDetailsReligionID { get; set; } = new List<EmployeePersonalDetails>();

        public ICollection<EmployeePersonalDetails> SpousePersonalDetailsReligionID { get; set; } = new List<EmployeePersonalDetails>();

    }
}
