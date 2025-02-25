using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class EmployeeDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeDocumentID { get; set; }
        [ForeignKey("Employee")]
        public int? EmployeeID { get; set; }
        [ForeignKey("Document")]
        public int? DocumentID { get; set; }

        public Document Document { get; set; } = null! ;

        public Employee Employee { get; set; } = null!;
    }
}
