using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.PositionDTO
{
    public class CreateNewPositionDTO
    {
        public int DesignationID { get; set; }

        public int SubjectID { get; set; }

        public int SchoolID { get; set; }
    }
}
