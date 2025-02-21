using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class PromotionRelinquishment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RelinquishmentID { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeID { get;  set; }
        public DateTime yearOfRelinquishment {  get; set; }

        [ForeignKey("Document")]
        public int? DocumentID { get; set; }

        public bool ApprovalStatus { get; set; }

        public Employee Employee { get; set; } = null!;

        public Document Document { get; set; } = null!;





    }
}
