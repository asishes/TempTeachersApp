using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.PromotionDTO.PromotionRelinquishment
{
    public class CreatePromotionRelinquishmentDTO
    {
        public int EmployeeID { get; set; }
        public int RelinquishmentYear { get;  set; }
        public int DocumentID { get; set; }

    }
}
