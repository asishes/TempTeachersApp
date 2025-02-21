using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.LeaveRequestDTO
{
    public class GetLeaveRequestDTO
    {
        public int LeaveRequestID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;    
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? StatusChangeDate { get; set; }
        public int ApprovalStatus { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? RequestedByID { get; set; }
        public string RequestedByUser { get; set; } = string.Empty;
        public int? ApprovedByID { get; set; }
        public string ApprovedByUser { get; set; } = string.Empty;
        public string? RequestorComment { get; set; }
        public string? ApproverComment { get; set; }
        public int? DocumentID { get; set; }
        public string? Documentpath { get; set; } 

    }
}
