using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.PromotionDTO.PromotionRelinquishment
{
    public class GetPromotionRelinquishmentDTO
    {
        public int RelinquishmentID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get;  set; } = string.Empty;
        public int RelinquishmentYear { get; set; }
        public string PromotedDesignation { get; set; } = string.Empty;
        public int? DocumentID { get; set; }
        public string DocumentFile { get; set; } = string.Empty;
        public bool ApprovalStatus { get; set; }
    }
}
