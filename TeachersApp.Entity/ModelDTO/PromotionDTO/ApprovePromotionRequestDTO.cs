using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.PromotionDTO
{
    public class ApprovePromotionRequestDTO
    {
        public string? ApproverComment { get; set; }

        public DateTime PromotionDate { get; set; }

        public int? ApprovedSchoolID { get; set; }
    }
}
