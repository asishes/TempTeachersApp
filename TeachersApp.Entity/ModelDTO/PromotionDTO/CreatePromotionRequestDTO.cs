using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.PromotionDTO
{
    public class CreatePromotionRequestDTO
    {
        public int EmployeeID { get; set; }
        public string? RequestorComment { get; set; }
        public string? FilePath { get; set; }
    }
}
