using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class TransferRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransferRequestID { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }

        [ForeignKey("FromSchool")]
        public int FromSchoolID { get; set; } 

        [ForeignKey("ToSchool_One")]
        public int ToSchoolID_One { get; set; }
        [ForeignKey("ToSchool_Two")]
        public int ToSchoolID_Two { get; set; }
        [ForeignKey("ToSchool_Three")]
        public int ToSchoolID_Three { get; set; }
        [ForeignKey("ToSchoolApproved")]
        public int? ToSchoolIDApproved { get; set; }

        public DateTime RequestDate { get; set; }

        [ForeignKey("Status")]
        public int StatusID { get; set; }
        public DateTime? StatusChangeDate { get; set; }

        public DateTime? TransferDate { get; set; }

        [ForeignKey("RequestedByUser")]
        public int? RequestedByID { get; set; }

        [ForeignKey("ApprovedByUser")]
        public int? ApprovedByID { get; set; }

        [MaxLength(255)]
        public string? RequestorComment { get; set; }

        [MaxLength(255)]
        public string? ApproverComment { get; set; }

        [MaxLength(255)]
        public string? FilePath { get; set; }

        // Navigation properties
        public Employee Employee { get; set; } = null!;
        public School FromSchool { get; set; } = null!;
        public School ToSchool_One { get; set; } = null!;
        public School ToSchool_Two { get; set; } = null!;
        public School ToSchool_Three { get; set; } = null!;
        public School ToSchoolApproved { get; set; } = null!;

        public Status Status { get; set; } = null!;
        public User RequestedByUser { get; set; } = null!;
        public User ApprovedByUser { get; set; } = null !;
    }
}

