using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PromotionID { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        [ForeignKey("FromSchool")]
        public int? FromSchoolID { get; set; }
        [ForeignKey("ToSchoolApproved")]
        public int? ToSchoolIDApproved { get; set; }

        public DateTime? PromotionDate { get; set; }

        [ForeignKey("Designation")]
        public int PromotedFromDesignationID { get; set; }


        [ForeignKey("Designation")]
        public int PromotedToDesignationID { get; set; }


        public DateTime RequestDate { get; set; }

        [ForeignKey("Status")]
        public int StatusID { get; set; }

        public DateTime? StatusChangeDate { get; set; }

        [ForeignKey("User")]
        public int? RequestedByID { get; set; }


        [ForeignKey("User")]
        public int? ApprovedByID { get; set; }
        [MaxLength(255)]
        public string? RequestorComment { get; set; }

        [MaxLength(255)]
        public string? ApproverComment { get; set; }

        [MaxLength(255)]
        public string? FilePath { get; set; }



        public Employee Employee { get; set; } = null!;
        public Designation PromotedFromDesignation { get; set; } = null!;
        public Designation PromotedToDesignation { get; set; } = null!;
        public School FromSchool { get; set; } = null!;
        public School ToSchoolApproved { get; set; } = null!;
        public Status Status { get; set; } = null!;
        public User RequestedByUser { get; set; } = null!;
        public User ApprovedByUser { get; set; } = null!;
    }
}
