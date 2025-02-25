using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class EmployeeSex
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SexID { get; set; }

        [Required,MaxLength(10)]
        public string Sex { get; set; } = null!;
        
        public ICollection<EmployeePersonalDetails> PersonalDetails { get; set; } = new List<EmployeePersonalDetails>();
    }
}
