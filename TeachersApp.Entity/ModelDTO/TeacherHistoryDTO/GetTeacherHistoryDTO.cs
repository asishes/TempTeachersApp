using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherHistoryDTO
{
    public class GetTeacherHistoryDTO
    {
        public int HistoryID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime? RetireDate { get; set; } = DateTime.MinValue;
        public DateTime? DateofJoin { get; set; } = DateTime.MinValue;
        public DateTime? ChangeDate { get; set; } = DateTime.MinValue;
        public int? ApprovalType { get; set; }
        public string ApprovalTypeText { get; set; } = string.Empty ;
        public int? ChangedByID { get; set; }
        public string? Username { get; set; } = string.Empty;
        public string? ChangeDescription { get; set; } = string.Empty;
        public int? ChangeTypeID { get; set; }
        public string? ChangedtypeName { get; set; } = string.Empty;
        public int? ChangeFromSchoolID { get; set; }
        public string FromSchoolName { get; set; } = string.Empty;
        public int? ChangeToSchoolID { get; set; }
        public string ToSchoolName { get; set; } = string.Empty;
        public int? PromotedFromPositionID { get; set; }
        public string FromPosition { get; set; } = string.Empty;
        public int? PromotedToPositionID { get; set; }
        public string ToPosition { get; set; } = string.Empty;
    }
}
