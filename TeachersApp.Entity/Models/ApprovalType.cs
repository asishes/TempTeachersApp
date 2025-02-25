using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class ApprovalType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApprovalTypeID { get; set; }

        [Required, MaxLength(50)]
        public string Approvaltype { get; set; } = null!;



        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
