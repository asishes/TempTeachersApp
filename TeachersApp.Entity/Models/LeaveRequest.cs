using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class LeaveRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveRequestID { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime RequestDate { get; set; }

        [ForeignKey("Status")]
        public int StatusID { get; set; }
        public DateTime? StatusChangeDate { get; set; }

        [ForeignKey("RequestedByUser")]
        public int? RequestedByID { get; set; }

        [ForeignKey("ApprovedByUser")]
        public int? ApprovedByID { get; set; }
        [MaxLength(255)]
        public string? RequestorComment { get; set; }
        [MaxLength(255)]
        public string? ApproverComment { get; set; }
        [ForeignKey("Document")]
        public int? DocumentID { get; set; }

        public Document? Document { get; set; }
        public Employee Employee { get; set; } = null!;
        public Status Status { get; set; } = null!;
        public User RequestedByUser { get; set; } = null!;
        public User ApprovedByUser { get; set; } = null!;

    }
}
