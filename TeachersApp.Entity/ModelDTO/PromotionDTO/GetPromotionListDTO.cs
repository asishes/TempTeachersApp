using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.PromotionDTO
{
    public class GetPromotionListDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;

        public string School { get; set; } = string.Empty;

        public int? SchoolID { get; set; }

        public int? SubjectID { get; set; }
        public string Subject { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;
        
        public int? DesignationID { get; set; } 

        public int? SeniorityNumber { get; set; }    

    }
}
