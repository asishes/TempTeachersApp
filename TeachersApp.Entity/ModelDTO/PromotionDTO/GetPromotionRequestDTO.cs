using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.PromotionDTO
{
    public class GetPromotionRequestDTO
    {
        public int PromotionID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime? PromotionDate { get; set; }
        public int PromotedFromID { get; set; } 
        public string? PromotedFromDesignation { get; set; } 
        public int PromotedToID { get; set; } 
        public string? PromotedToDesignation { get; set; }
        public int? FromSchoolID { get; set; }
        public string? PromotedFromSchool { get; set; }
        public int? ApprovedSchoolID { get; set; }
        public string? PromotedToSchool { get; set; }

        public string? RequestorCommand { get; set; } = string.Empty ;

        public string? ApproverCommand { get; set; } = string.Empty;

        public string? FilePath { get; set; }

        public DateTime RequestDate { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? StatusChangeDate { get; set; } 
        public int? RequestedBy { get; set; }
        public string? RequestedByUser { get; set; } 
        public int? ApprovedBy { get; set; } 
        public string? ApprovedByUser { get; set; } 
    }
}
