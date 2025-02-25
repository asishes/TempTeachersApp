using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class District
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DistrictID { get; set; }

        [Required, MaxLength(50)]
        public string DistrictName { get; set; } = null!;


        public ICollection<EmployeePersonalDetails> PersonalDetails { get; set; } = new List<EmployeePersonalDetails>();
    }
}
