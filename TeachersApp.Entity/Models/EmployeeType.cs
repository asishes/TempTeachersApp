using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class EmployeeType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeTypeID { get; set; }

        [Required, MaxLength(50)]
        public string Employeetype { get; set; } = null!;



        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
