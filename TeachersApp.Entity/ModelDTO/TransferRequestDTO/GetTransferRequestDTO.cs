using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TransferRequestDTO
{
    public class GetTransferRequestDTO
    {
        public int TransferRequestID { get; set; }

        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public int? DesignationID { get; set; }

        public string? DesignationName { get; set; } = string.Empty;

        public int FromSchoolID { get; set; }

        public string FromSchoolName { get; set; } = string.Empty;

        public int ToSchoolID_One { get; set; }

        public string ToSchoolOneName { get; set; } = string.Empty;
        public int ToSchoolID_Two { get; set; }

        public string ToSchoolTwoName { get; set; } = string.Empty;
        public int ToSchoolID_Three { get; set; }

        public string ToSchoolThreeName { get; set; } = string.Empty;
        public int? ToApprovedSchoolID { get; set; }

        public string ToApprovedSchoolName { get; set; } = string.Empty;

        public DateTime RequestDate { get; set; }

        public DateTime? StatusChangeDate { get; set; }

        public DateTime? TransferDate { get; set; }

        public int ApprovalStatus { get; set; }

        public string Status { get; set; } = string.Empty;

        public int? RequestedByID { get; set; }

        public string RequestedByUser { get; set; } = string.Empty;

        public int? ApprovedByID { get; set; }

        public string ApprovedByUser { get; set; } = string.Empty;

        public string? RequestorComment { get; set; }

        public string? ApproverComment { get; set; }
        
        public string? FilePath { get; set; }
    }
}
